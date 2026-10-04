using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToTrainingExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "training_exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "training_exercises");
        }
    }
}
