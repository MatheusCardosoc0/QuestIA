using QuestIA.Core.Models;

namespace QuestIA.Core.Models.DTOs
{
    public class AttemptDTO
    {
        public Guid? Id { get; set; }
        public Guid QuizId { get; set; }
        public List<UserResponseQuestion> UserResponseQuestions { get; set; }
        public Guid? UserId { get; set; }
    }
} 