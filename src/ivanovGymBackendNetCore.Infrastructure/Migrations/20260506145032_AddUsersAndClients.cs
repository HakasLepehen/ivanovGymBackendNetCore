using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddUsersAndClients : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:citext", ",,");

        migrationBuilder.CreateTable(
            name: "clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "now()"),
                fullName = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                Age = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Target = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Experience = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Sleep = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Food = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Pharma = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Activity = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Avatar = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                Limits = table.Column<List<short>>(type: "smallint[]", nullable: true),
                isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                Email = table.Column<string>(type: "citext", nullable: false),
                Password = table.Column<string>(type: "varchar", nullable: false),
                Roles = table.Column<string[]>(type: "text[]", nullable: false, defaultValueSql: "ARRAY['user']::text[]"),
                token = table.Column<string>(type: "varchar", nullable: true),
                client_fk_id = table.Column<Guid>(type: "uuid", nullable: true),
                createdAt = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_users_client_fk_id",
            table: "users",
            column: "client_fk_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_users_Email",
            table: "users",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "clients");

        migrationBuilder.DropTable(
            name: "users");

        migrationBuilder.AlterDatabase()
            .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");
    }
}
