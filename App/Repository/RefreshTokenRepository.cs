using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken, Guid>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(QuizIAContext context) : base(context)
        {
        }
    }
}