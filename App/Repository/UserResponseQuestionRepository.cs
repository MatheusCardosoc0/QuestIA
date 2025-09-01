using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class UserResponseQuestionRepository : RepositoryBase<UserResponseQuestion, int>, IUserResponseQuestionRepository
    {
        public UserResponseQuestionRepository(QuizIAContext context) : base(context)
        {
        }

    }
}