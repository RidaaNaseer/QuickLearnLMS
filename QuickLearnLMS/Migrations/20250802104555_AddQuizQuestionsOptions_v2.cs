using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickLearnLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizQuestionsOptions_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizID",
                table: "Questions",
                column: "QuizID");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizID",
                table: "Questions",
                column: "QuizID",
                principalTable: "Quizzes",
                principalColumn: "QuizID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizID",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizID",
                table: "Questions");
        }
    }
}
