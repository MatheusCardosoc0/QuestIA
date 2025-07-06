using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? RedefinePasswordCode { get; set; }
        [JsonIgnore]
        public List<Subject>? Subjects { get; set; }
        [JsonIgnore]
        public List<Option>? Options { get; set; }
        [JsonIgnore]
        public List<Quest>? Quests { get; set; }
        [JsonIgnore]
        public RefreshToken? RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
