using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTrainingExerciseToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_training_exercises_exercise_id",
                table: "training_exercises");

            migrationBuilder.DropIndex(
                name: "IX_training_exercises_training_id",
                table: "training_exercises");

            migrationBuilder.CreateIndex(
                name: "IX_training_exercises_exercise_id",
                table: "training_exercises",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_training_exercises_training_id",
                table: "training_exercises",
                column: "training_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_training_exercises_exercise_id",
                table: "training_exercises");

            migrationBuilder.DropIndex(
                name: "IX_training_exercises_training_id",
                table: "training_exercises");

            migrationBuilder.CreateIndex(
                name: "IX_training_exercises_exercise_id",
                table: "training_exercises",
                column: "exercise_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_training_exercises_training_id",
                table: "training_exercises",
                column: "training_id",
                unique: true);
        }
    }
}
