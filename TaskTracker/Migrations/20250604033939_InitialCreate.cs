using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedUserId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "CreatedDate", "Description", "EndDate", "ProjectName", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "開發任務追蹤系統，展示 CRUD 功能", new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "TaskTracker 系統開發", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "進行中" },
                    { 2, new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "準備期末報告相關文件和展示", new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "期末報告準備", new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "進行中" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "Department", "Email", "IsActive", "Password", "Position", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "開發部", "ming.zhang@example.com", true, "password123", "前端工程師", "User", "張小明" },
                    { 2, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "開發部", "hua.li@example.com", true, "password123", "後端工程師", "User", "李小華" },
                    { 3, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "設計部", "mei.wang@example.com", true, "password123", "UI設計師", "User", "王小美" },
                    { 4, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IT部", "admin@example.com", true, "password123", "系統管理員", "Admin", "系統管理員" }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskId", "AssignedUserId", "CompletedDate", "CreatedDate", "Description", "DueDate", "Priority", "ProjectId", "Status", "TaskName" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "設計並建立 SQLite 資料庫", new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "高", 1, "已完成", "建立資料庫結構" },
                    { 2, 2, null, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "開發專案、使用者和任務的 CRUD 功能", new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "高", 1, "進行中", "實作 CRUD 功能" },
                    { 3, 3, null, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "使用 Tailwind CSS 設計現代化 UI", new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "中", 1, "待辦", "設計使用者介面" },
                    { 4, 1, null, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "製作期末報告展示投影片", new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "高", 2, "進行中", "準備期末報告投影片" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedUserId",
                table: "Tasks",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
