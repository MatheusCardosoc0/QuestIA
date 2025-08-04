using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class QuestionDto
    {
        public int? Id { get; set; }
        public string QuestionText  { get; set; }
        public string Response { get; set; }
        public Guid? QuizId {  get; set; }
        public Guid? UserId { get; set; }
        public List<Option> Options { get; set; }
    }
}
