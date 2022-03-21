using Microsoft.EntityFrameworkCore.Migrations;

namespace BulgarianPlacesAPI.Migrations
{
    public partial class AddedPlaceName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Places");
        }
    }
}
