using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClientFkIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_client_fk_id",
                table: "users");

            migrationBuilder.Sql("ALTER TABLE users ALTER COLUMN client_fk_id TYPE integer USING NULL;");

            migrationBuilder.CreateIndex(
                name: "IX_users_client_fk_id",
                table: "users",
                column: "client_fk_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_client_fk_id",
                table: "users");

            migrationBuilder.Sql("ALTER TABLE users ALTER COLUMN client_fk_id TYPE uuid USING NULL;");

            migrationBuilder.CreateIndex(
                name: "IX_users_client_fk_id",
                table: "users",
                column: "client_fk_id",
                unique: true);
        }
    }
}
