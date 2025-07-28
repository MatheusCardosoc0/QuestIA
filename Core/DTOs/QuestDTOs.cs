using System.Text.Json.Serialization;

namespace QuestIA.Core.Models
{
    public class QuestDto
    {
        public int? Id { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public Guid? SubjectId {  get; set; }
        public Guid? UserId { get; set; }
        public List<OptionDto> Options { get; set; }
    }
}
