using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class Quest : IEntity<int>
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public Guid SubjectId {  get; set; }
        [JsonIgnore]
        public Subject Subject { get; set; }
        public Guid? UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [JsonIgnore]
        public List<Option> Options { get; set; }
    }
}
