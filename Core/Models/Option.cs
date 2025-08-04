using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class Option : IEntity<int>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsCheck { get; set; }
        public int QuestId { get; set; }
        [JsonIgnore]
        public Question Quest { get; set; }
        public Guid? UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
