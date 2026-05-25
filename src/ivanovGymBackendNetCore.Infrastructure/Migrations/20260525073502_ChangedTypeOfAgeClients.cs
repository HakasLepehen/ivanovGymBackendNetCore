using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ivanovGymBackendNetCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTypeOfAgeClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE clients ALTER COLUMN ""Age"" DROP DEFAULT;
                ALTER TABLE clients 
                ALTER COLUMN ""Age"" TYPE int 
                USING CASE 
                    WHEN ""Age"" ~ '^[0-9]+$' THEN ""Age""::integer
                    ELSE NULL
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Age",
                table: "clients",
                type: "text",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
