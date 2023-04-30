using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SessionFinal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumnToSessions5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Sessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
