using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestIA.Migrations
{
    /// <inheritdoc />
    public partial class Correctingnomenclatureoftablesandcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Subjects_SubjectId",
                table: "Quests");

            migrationBuilder.RenameColumn(
                name: "QuestType",
                table: "Subjects",
                newName: "QuestionType");

            migrationBuilder.RenameColumn(
                name: "QuantityQuests",
                table: "Subjects",
                newName: "QuantityQuestions");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Quests",
                newName: "QuizId");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Quests",
                newName: "QuestionText");

            migrationBuilder.RenameIndex(
                name: "IX_Quests_SubjectId",
                table: "Quests",
                newName: "IX_Quests_QuizId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Subjects",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAttempt",
                table: "Subjects",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeSpent",
                table: "Subjects",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Subjects_QuizId",
                table: "Quests",
                column: "QuizId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Subjects_QuizId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "LastAttempt",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "Subjects",
                newName: "QuestType");

            migrationBuilder.RenameColumn(
                name: "QuantityQuestions",
                table: "Subjects",
                newName: "QuantityQuests");

            migrationBuilder.RenameColumn(
                name: "QuizId",
                table: "Quests",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "Quests",
                newName: "Question");

            migrationBuilder.RenameIndex(
                name: "IX_Quests_QuizId",
                table: "Quests",
                newName: "IX_Quests_SubjectId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Subjects_SubjectId",
                table: "Quests",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
