using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;

namespace QuestIA.Infra.Database
{
    public class QuestIAContext : DbContext
    {
        public QuestIAContext(DbContextOptions<QuestIAContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //    entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            //    entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            //    entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            //    entity.HasIndex(e => e.Email).IsUnique();
            //});

            //modelBuilder.Entity<Subject>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //    entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            //    entity.Property(e => e.Description).HasMaxLength(500);
            //    entity.Property(e => e.Score).HasDefaultValue(0);
            //    entity.Property(e => e.QuantityQuests).HasDefaultValue(0);

            //    entity.HasOne(e => e.User)
            //          .WithMany()
            //          .HasForeignKey(e => e.UserId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

            //modelBuilder.Entity<Quest>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //    entity.Property(e => e.Question).IsRequired().HasMaxLength(1000);
            //    entity.Property(e => e.Response).HasMaxLength(2000);
                
            //    entity.HasOne(e => e.Subject)
            //          .WithMany()
            //          .HasForeignKey(e => e.SubjectId)
            //          .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasOne(e => e.User)
            //          .WithMany()
            //          .HasForeignKey(e => e.UserId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

            //modelBuilder.Entity<Option>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //    entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            //    entity.Property(e => e.IsCheck).HasDefaultValue(false);

            //    entity.HasOne(e => e.Quest)
            //          .WithMany(q => q.Options)
            //          .HasForeignKey(e => e.QuestId)
            //          .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasOne(e => e.User)
            //          .WithMany()
            //          .HasForeignKey(e => e.UserId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});
        }
    }
}
