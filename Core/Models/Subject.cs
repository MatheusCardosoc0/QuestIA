using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public enum QuestTypes
    {
        Objective, Subjective, Alternate
    }
    public enum DifficultyLevel
    {
        Easy, Hard, Medium
    }
    public class Subject : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double? Score { get; set; }
        public int? TimeLimit { get; set; }
        public int? TimesTaken { get; set; }
        public int? QuantityQuests { get; set; }
        public bool? AutoSubmitOnTimeout { get; set; }
        public DifficultyLevel? DifficultyLevel { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsRandom { get; set; }
        public QuestTypes? QuestType { get; set; }
        public List<string>? Tags { get; set; }
        public Guid? UserId { get; set; }
        [JsonIgnore]
        public List<Quest>? Questions { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
