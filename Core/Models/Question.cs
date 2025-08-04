using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class Question : IEntity<int>
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Response { get; set; }
        public Guid QuizId {  get; set; }
        [JsonIgnore]
        public Quiz Quiz { get; set; }
        public Guid? UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public List<Option> Options { get; set; }
    }
}
