using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MorionParkingBot.Migrations.PromoCodeDb
{
    public partial class AddUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "PromoCodes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PromoCodes");
        }
    }
}
