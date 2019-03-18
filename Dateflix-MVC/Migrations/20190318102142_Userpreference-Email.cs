using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DateflixMVC.Migrations
{
    public partial class UserpreferenceEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserPreferenceId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPreference",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MinimumAge = table.Column<int>(nullable: false),
                    MaximumAge = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreference", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserPreferenceId",
                table: "Users",
                column: "UserPreferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserPreference_UserPreferenceId",
                table: "Users",
                column: "UserPreferenceId",
                principalTable: "UserPreference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserPreference_UserPreferenceId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserPreference");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserPreferenceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserPreferenceId",
                table: "Users");
        }
    }
}
