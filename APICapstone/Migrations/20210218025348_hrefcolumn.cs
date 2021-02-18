using Microsoft.EntityFrameworkCore.Migrations;

namespace APICapstone.Migrations
{
    public partial class hrefcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Favorites");

            migrationBuilder.AddColumn<string>(
                name: "href",
                table: "Favorites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "href",
                table: "Favorites");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Favorites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
