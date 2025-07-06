using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class OptionDto
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public bool? IsCheck { get; set; }
        public int? QuestId { get; set; }
        public Guid? UserId { get; set; }
    }
}
