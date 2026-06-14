using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "clients",
                newName: "full_name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_clients_Guid",
                table: "clients",
                column: "Guid");

            migrationBuilder.CreateTable(
                name: "trainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    planned_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trainings_clients_client_guid",
                        column: x => x.client_guid,
                        principalTable: "clients",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_trainings_client_guid",
                table: "trainings",
                column: "client_guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trainings");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_clients_Guid",
                table: "clients");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "clients",
                newName: "fullName");
        }
    }
}
