using Microsoft.EntityFrameworkCore.Migrations;

namespace TechSupportApp.Infrastructure.Migrations
{
    public partial class ticket_solution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Tickets",
                newName: "Solution");

            migrationBuilder.AddColumn<string>(
                name: "Issue",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Issue",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Solution",
                table: "Tickets",
                newName: "Body");
        }
    }
}
