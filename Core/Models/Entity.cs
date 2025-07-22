namespace QuestIA.Core.Models
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }

        Guid? UserId { get; set; }
    }
}
