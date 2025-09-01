using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;

namespace QuestIA.Infra.Database
{
    public class QuizIAContext : DbContext
    {
        public QuizIAContext(DbContextOptions<QuizIAContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<UserResponseQuestion> UserResponseQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
