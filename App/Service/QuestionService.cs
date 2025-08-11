using Microsoft.AspNetCore.Mvc.ModelBinding;
using prototipo_ia_api.Infra.Utilities;
using QuestIA.App.Repository;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;
using QuestIA.Infra.Helpers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace QuestIA.App.Service
{
    public class QuestionService : ServiceBase<Question, int>, IQuestionService
    {
        private readonly IQuestionRepository _questRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelName;

        public QuestionService(
            IUnitOfWork unitOfWork,
            IQuestionRepository questRepository,
            IQuizRepository subjectRepository,
            HttpClient httpClient,
            IConfiguration configuration
            ) 
            : base(unitOfWork, questRepository)
        {
            _questRepository = questRepository;
            _quizRepository = subjectRepository;
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
            _modelName = configuration["Gemini:Model"];
        }

        public async Task<IEnumerable<Question>> GetByQuizIdAsync(Guid subjectId)
        {
            return await _questRepository.GetByQuizIdAsync(subjectId);
        }

        public async Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId)
        {
            return await _questRepository.GetByUserIdAsync(userId);
        }

        public class RawQuestionDto
        {
            public string QuestionText { get; set; } = default!;
            public string Response { get; set; } = default!;
            public List<string> Options { get; set; } = new();
        }

        public async Task<IEnumerable<Question>> GenerateQuestionsByQuiz(Guid userId, Guid quizId)
        {
            var verifyQuesstions = await _unitOfWork.Questions.AsQueryable().Include(c => c.Options).Where(c => c.QuizId == quizId).ToListAsync();

            if(verifyQuesstions.Count() > 0)
            {
                return verifyQuesstions;
            }

            var quiz = await _unitOfWork.Quizzes
                .AsQueryable()
                .Include(c => c.Questions)
                  .ThenInclude(c => c.Options)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == quizId);

            if (quiz == null)
                throw new Exception("Assunto não encontrado.");

            if(quiz.Questions.ToList().Count > 0)
            {
                return quiz.Questions.ToList();
            }

            var promptText = BasePrompt.Instruction(
                difficultyLevel: quiz.DifficultyLevel,
                name: quiz.Name,
                description: quiz.Description,
                quantityQuestions: quiz.QuantityQuestions ?? 5
            );

            var genReq = new
            {
                contents = new[] { new { role = "user", parts = new[] { new { text = promptText } } } }
            };
            var genResp = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/{_modelName}:generateContent?key={_apiKey}",
                new StringContent(JsonSerializer.Serialize(genReq), Encoding.UTF8, "application/json")
            );
            var genBody = await genResp.Content.ReadAsStringAsync();
            if (!genResp.IsSuccessStatusCode)
                throw new Exception($"Erro na API Gemini: {genBody}");
            var gemini = JsonSerializer.Deserialize<GeminiResponse>(genBody);
            var json = gemini.Candidates.First().Content.Parts.First().Text;

            static string StripMarkdownJson(string input)
            {
                var match = Regex.Match(input, @"```json\s*(.*?)\s*```", RegexOptions.Singleline);
                return match.Success
                    ? match.Groups[1].Value
                    : input; 
            }

            var raw = gemini.Candidates.First().Content.Parts.First().Text;
            var cleanJson = StripMarkdownJson(raw);
            var rawDtos = JsonSerializer.Deserialize<List<RawQuestionDto>>(cleanJson)
             ?? throw new Exception("Não foi possivel gerar as questões verifique o nome da Quiz e sua descrição.");

            var questions = rawDtos.Select(r =>
            {
                var opts = r.Options.Select(text => new Option
                {
                    Description = text,
                    IsCheck = false,    
                    UserId = userId
                }).ToList();

                return new Question
                {
                    QuizId = quizId,
                    UserId = userId,
                    QuestionText = r.QuestionText,
                    Response = r.Response,
                    Options = opts
                };
            }).ToList();

            await _questRepository.AddRangeAsync(questions);
            await _unitOfWork.SaveChangesAsync();

            return questions;
        }
    }
} 