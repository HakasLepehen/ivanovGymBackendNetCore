using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetColNameToIdInConsultationReqeustsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "consultation_requests",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "consultation_requests",
                newName: "Id");
        }
    }
}
