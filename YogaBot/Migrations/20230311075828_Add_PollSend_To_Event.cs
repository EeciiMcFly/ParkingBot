using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaBot.Migrations
{
    public partial class Add_PollSend_To_Event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PollSend",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PollSend",
                table: "Events");
        }
    }
}
