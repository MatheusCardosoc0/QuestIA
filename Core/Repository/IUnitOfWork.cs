// QuestIA.Core/Repository/IUnitOfWork.cs
using System;
using System.Threading.Tasks;

namespace QuestIA.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository    Users { get; }
        IRefreshTokenRepository RefreshToken { get; }
        IQuizRepository Quizzes { get; }
        IQuestionRepository   Questions { get; }
        IOptionRepository  Options { get; }

        IUserResponseQuestionRepository UserResponseQuestions { get; }
        IAttemptRepository Attempts { get; }

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
