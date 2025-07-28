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

namespace QuestIA.App.Service
{
    public class QuestService : ServiceBase<Quest, int>, IQuestService
    {
        private readonly IQuestRepository _questRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelName;

        public QuestService(
            IUnitOfWork unitOfWork,
            IQuestRepository questRepository,
            ISubjectRepository subjectRepository,
            HttpClient httpClient,
            IConfiguration configuration
            ) 
            : base(unitOfWork, questRepository)
        {
            _questRepository = questRepository;
            _subjectRepository = subjectRepository;
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
            _modelName = configuration["Gemini:Model"];
        }

        public async Task<IEnumerable<Quest>> GetBySubjectIdAsync(Guid subjectId)
        {
            return await _questRepository.GetBySubjectIdAsync(subjectId);
        }

        public async Task<IEnumerable<Quest>> GetByUserIdAsync(Guid userId)
        {
            return await _questRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Quest>> GenerateQuestsBySubject(Guid userId, Guid subjectId)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId, userId);
            if (subject == null)
                throw new Exception("Assunto não encontrado.");

            var promptText = BasePrompt.Instruction(
                dificultyLevel: subject.DifficultyLevel.ToString(),
                name: subject.Name,
                description: subject.Description,
                quantityQuestion: subject.QuantityQuests ?? 5
            );

            //var cacheReq = new
            //{
            //    content = new
            //    {
            //        role = "model",
            //        parts = new[] { new { text = promptText } }
            //    },
            //    ttl = "360s"
            //};
            //var cacheResp = await _httpClient.PostAsync(
            //    $"https://generativelanguage.googleapis.com/v1beta/cachedContents?key={_apiKey}",
            //    new StringContent(JsonSerializer.Serialize(cacheReq), Encoding.UTF8, "application/json")
            //);
            //var cacheBody = await cacheResp.Content.ReadAsStringAsync();
            //if (!cacheResp.IsSuccessStatusCode)
            //    throw new Exception($"Erro ao cachear prompt: {cacheBody}");
            //var cacheResult = JsonSerializer.Deserialize<GeminiCacheResponse>(cacheBody);
            //var cacheName = cacheResult.Name;

            var genReq = new
            {
                contents = new[] { new { role = "user", parts = new[] { new { text = promptText } } } },
                //cachedContent = cacheName
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

            var dtos = JsonSerializer.Deserialize<List<QuestDto>>(json);
            if (dtos == null)
                throw new Exception("Não foi possível desserializar as quests.");

            var quests = dtos
                .Select(d => new Quest
                {
                    SubjectId = subjectId,
                    UserId = userId,
                    Question = d.Question,
                    Response = d.Response,
                    Options = d.Options?
                        .Select(oDto => new Option
                        {
                            Description = oDto.Description,
                            IsCheck = oDto.IsCheck,
                            UserId = oDto.UserId
                        })
                        .ToList()
                    ?? new List<Option>()
                })
                .ToList();

            await _questRepository.AddRangeAsync(quests);
            await _unitOfWork.SaveChangesAsync();

            return quests;
        }
    }
} 