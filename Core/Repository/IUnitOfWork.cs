// QuestIA.Core/Repository/IUnitOfWork.cs
using System;
using System.Threading.Tasks;

namespace QuestIA.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository    Users { get; }
        IRefreshTokenRepository RefreshToken { get; }
        ISubjectRepository Subjects { get; }
        IQuestRepository   Quests { get; }
        IOptionRepository  Options { get; }

        Task<int>   SaveChangesAsync();
        Task        BeginTransactionAsync();
        Task        CommitTransactionAsync();
        Task        RollbackTransactionAsync();

        // ATENÇÃO a assinatura genérica:
        IRepositoryBase<T, TKey> Repository<T, TKey>()
            where T    : class, QuestIA.Core.Models.IEntity<TKey>
            where TKey : notnull;
    }
}
