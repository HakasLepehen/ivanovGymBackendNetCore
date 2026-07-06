using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedIntToTextInTrExEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "execution_number",
                table: "training_exercises",
                type: "text",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "execution_number",
                table: "training_exercises",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "");
        }
    }
}
