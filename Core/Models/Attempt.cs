using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class Attempt : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public List<UserResponseQuestion> UserResponseQuestions { get; set; }
        public Guid? UserId { get; set; }

        [JsonIgnore]
        public Quiz Quiz { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
