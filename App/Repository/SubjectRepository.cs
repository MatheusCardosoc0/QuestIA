using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class SubjectRepository : RepositoryBase<Subject, Guid>, ISubjectRepository
    {
        public SubjectRepository(QuestIAContext context) : base(context)
        {
        }

        // Métodos específicos do Subject podem ser implementados aqui
    }
} 