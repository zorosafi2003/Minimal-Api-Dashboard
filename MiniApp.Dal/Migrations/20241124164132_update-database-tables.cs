using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApp.Dal.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabasetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employee");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Project",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Employee",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Employee",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TimeSheet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrackedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsWorking = table.Column<bool>(type: "INTEGER", nullable: false),
                    TotalMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionType = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Identifier = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSheet_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeSheet_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheet_EmployeeId",
                table: "TimeSheet",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheet_ProjectId",
                table: "TimeSheet",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSheet");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employee",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
