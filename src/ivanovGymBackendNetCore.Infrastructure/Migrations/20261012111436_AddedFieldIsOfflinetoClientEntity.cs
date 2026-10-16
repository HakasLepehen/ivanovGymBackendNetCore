using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldIsOfflinetoClientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "clients",
                newName: "is_active");

            migrationBuilder.AddColumn<bool>(
                name: "is_offline",
                table: "clients",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_offline",
                table: "clients");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "clients",
                newName: "isActive");
        }
    }
}
