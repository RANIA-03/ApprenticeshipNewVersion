using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apprenticeship.Data.Migrations
{
    public partial class AddContactMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contactMessages",
                columns: table => new
                {
                    ContactMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Msg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactMessages", x => x.ContactMessageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contactMessages");
        }
    }
}
