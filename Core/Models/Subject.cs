using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class Subject : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Score { get; set; }
        public int? QuantityQuests { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public List<Quest>? Questions { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
