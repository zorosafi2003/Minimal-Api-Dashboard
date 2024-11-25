using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApp.Dal.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabasetables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "TimeSheet");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Employee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Identifier",
                table: "TimeSheet",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Identifier",
                table: "Project",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Identifier",
                table: "Employee",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
