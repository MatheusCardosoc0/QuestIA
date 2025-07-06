using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class SubjectDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Score { get; set; }
        public int? QuantityQuests { get; set; }
        public Guid? UserId { get; set; }
    }
}
