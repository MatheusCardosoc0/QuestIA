using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class UserResponseQuestion : IEntity<int>
    {
        public int Id { get; set; }
        public Guid AttemptId { get; set; }
        public int QuestionId { get; set; }
        public Guid? UserId { get; set; }
        public int? OptionId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public Question? Question { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Option? Option { get; set; }
        [JsonIgnore]
        public Attempt? Attempt { get; set; }
    }
}
