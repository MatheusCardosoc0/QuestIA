using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestIA.Migrations
{
    /// <inheritdoc />
    public partial class Correctingnomenclatureoftables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Quests_QuestId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Subjects_QuizId",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Users_UserId",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Users_UserId",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quests",
                table: "Quests");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Quizzes");

            migrationBuilder.RenameTable(
                name: "Quests",
                newName: "Questions");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_UserId",
                table: "Quizzes",
                newName: "IX_Quizzes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Quests_UserId",
                table: "Questions",
                newName: "IX_Questions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Quests_QuizId",
                table: "Questions",
                newName: "IX_Questions_QuizId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestId",
                table: "Options",
                column: "QuestId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_UserId",
                table: "Questions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Users_UserId",
                table: "Quizzes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_UserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Users_UserId",
                table: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Quests");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_UserId",
                table: "Subjects",
                newName: "IX_Subjects_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_UserId",
                table: "Quests",
                newName: "IX_Quests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuizId",
                table: "Quests",
                newName: "IX_Quests_QuizId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quests",
                table: "Quests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Quests_QuestId",
                table: "Options",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Subjects_QuizId",
                table: "Quests",
                column: "QuizId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Users_UserId",
                table: "Quests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Users_UserId",
                table: "Subjects",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
