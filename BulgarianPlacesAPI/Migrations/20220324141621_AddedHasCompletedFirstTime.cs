using Microsoft.EntityFrameworkCore.Migrations;

namespace BulgarianPlacesAPI.Migrations
{
    public partial class AddedHasCompletedFirstTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCompletedFirstTime",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCompletedFirstTime",
                table: "Users");
        }
    }
}
