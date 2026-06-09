using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedRequiredStatusOfMuscleGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "muscle_group",
                table: "exercises",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "muscle_group",
                table: "exercises",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
