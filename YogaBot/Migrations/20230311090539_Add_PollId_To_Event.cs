using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaBot.Migrations
{
    public partial class Add_PollId_To_Event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PollId",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PollMessageId",
                table: "Events",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PollId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PollMessageId",
                table: "Events");
        }
    }
}
