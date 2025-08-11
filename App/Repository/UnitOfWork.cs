// QuestIA.App/Repository/UnitOfWork.cs
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace QuestIA.App.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuestIAContext _context;
        private IDbContextTransaction _transaction;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public UnitOfWork(QuestIAContext context)
        {
            _context = context;
        }

        // 1 única definição de Repository<T, TKey>()
        public IRepositoryBase<T, TKey> Repository<T, TKey>()
            where T : class, IEntity<TKey>
            where TKey : notnull
        {
            var type = typeof(T);

            if (_repositories.TryGetValue(type, out var existing))
                return (IRepositoryBase<T, TKey>)existing;

            object instance;

            // repos específicos
            if (type == typeof(User))
                instance = new UserRepository(_context);
            else if (type == typeof(RefreshToken))
                instance = new RefreshTokenRepository(_context);
            else if (type == typeof(Quiz))                  
                instance = new QuizRepository(_context);
            else if (type == typeof(Question))
                instance = new QuestionRepository(_context);
            else
            {
                // fallback para o genérico
                var repoType = typeof(RepositoryBase<,>).MakeGenericType(type, typeof(TKey));
                instance = Activator.CreateInstance(repoType, _context)!;
            }

            _repositories[type] = instance;
            return (IRepositoryBase<T, TKey>)instance;
        }

        // propriedades de acesso
        public IUserRepository Users => (IUserRepository)Repository<User, Guid>();
        public IRefreshTokenRepository RefreshToken => (IRefreshTokenRepository)Repository<RefreshToken, Guid>();
        public IQuizRepository Quizzes => (IQuizRepository)Repository<Quiz, Guid>();
        public IQuestionRepository Questions => (IQuestionRepository)Repository<Question, int>();
        public IOptionRepository Options => (IOptionRepository)Repository<Option, int>();

        // transações
        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
            => await _transaction.RollbackAsync();

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
