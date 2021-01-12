using Microsoft.EntityFrameworkCore.Migrations;

namespace TechSupportApp.Infrastructure.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Issue",
                table: "TicketEntries");

            migrationBuilder.RenameColumn(
                name: "Issuer",
                table: "Tickets",
                newName: "Issue");

            migrationBuilder.RenameColumn(
                name: "Solution",
                table: "TicketEntries",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "IssuerId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "TicketEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IssuerId",
                table: "Tickets",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketEntries_AuthorId",
                table: "TicketEntries",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketEntries_Users_AuthorId",
                table: "TicketEntries",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_IssuerId",
                table: "Tickets",
                column: "IssuerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketEntries_Users_AuthorId",
                table: "TicketEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_IssuerId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_IssuerId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_TicketEntries_AuthorId",
                table: "TicketEntries");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "TicketEntries");

            migrationBuilder.RenameColumn(
                name: "Issue",
                table: "Tickets",
                newName: "Issuer");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "TicketEntries",
                newName: "Solution");

            migrationBuilder.AddColumn<string>(
                name: "Issue",
                table: "TicketEntries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
