using System;
using System.Collections.Generic;
using QuestIA.Core.Models; // para DifficultyLevel e QuestTypes

namespace QuestIA.Core.Models
{
    public class QuizDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Score { get; set; }
        public int? TimeLimit { get; set; }
        public int? TimesTaken { get; set; }
        public int? QuantityQuestions { get; set; }
        public bool? AutoSubmitOnTimeout { get; set; }
        public string? TimeSpent { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsRandom { get; set; }
        public QuestionTypes? QuestionTypes { get; set; }
        public List<string>? Tags { get; set; }
        public Guid? UserId { get; set; }
    }
}
