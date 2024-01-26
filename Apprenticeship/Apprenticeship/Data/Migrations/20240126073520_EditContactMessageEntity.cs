using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apprenticeship.Data.Migrations
{
    public partial class EditContactMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "contactMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "contactMessages");
        }
    }
}
