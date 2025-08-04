using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class QuizRepository : RepositoryBase<Quiz, Guid>, IQuizRepository
    {
        public QuizRepository(QuestIAContext context) : base(context)
        {
        }

        // Métodos específicos do Subject podem ser implementados aqui
    }
} 