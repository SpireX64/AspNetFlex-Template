using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetFlex.App.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    Role = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    pass_hash = table.Column<string>(nullable: false),
                    reg_date = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
