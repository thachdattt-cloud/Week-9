using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassID);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectID);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ClassID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentGrades",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<int>(type: "int", nullable: false),
                    Mark = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGrades", x => new { x.StudentID, x.SubjectID });
                    table.ForeignKey(
                        name: "FK_StudentGrades_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentGrades_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassID", "ClassCode", "ClassName" },
                values: new object[,]
                {
                    { 1, "cntt01", "công nghệ thông tin 1" },
                    { 2, "cntt02", "công nghệ thông tin 2" },
                    { 4, "cntt03", "công nghệ thông tin 3" },
                    { 5, "cntt04", "công nghệ thông tin 4" },
                    { 6, "cntt05", "công nghệ thông tin 5" },
                    { 7, "cntt06", "công nghệ thông tin 6" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "SubjectID", "Credits", "SubjectCode", "SubjectName" },
                values: new object[,]
                {
                    { 3, 4, "CS101", "Lập trình C#" },
                    { 4, 4, "DB201", "Cơ sở dữ liệu SQL Server" },
                    { 5, 3, "WEB301", "Phát triển ứng dụng Web" },
                    { 1002, 2, "html", "lập trình html" },
                    { 1003, 2, "android", "lập trình android" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentID", "BirthDate", "ClassID", "Email", "FullName", "Gender", "StudentCode" },
                values: new object[,]
                {
                    { 1, new DateTime(2005, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "an.nv@uneti.com", "Nguyễn Hữu Quang", true, "SV01" },
                    { 2, new DateTime(2003, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "bich.tt@uneti.com", "Trần Thị Bích", false, "SV02" },
                    { 3, new DateTime(2002, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "cuong.lh@yahoo.com", "Lê Hoàng Cường", true, "SV03" },
                    { 1004, new DateTime(2005, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "tai@gmail.com", "Nguyen trong tai", true, "SV04" },
                    { 1005, new DateTime(2005, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "quan@gmail.com", "Nguyen van quan", true, "SV05" },
                    { 1006, new DateTime(2005, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "lan@gmail.com", "le dieu lan v2", false, "SV06" }
                });

            migrationBuilder.InsertData(
                table: "StudentGrades",
                columns: new[] { "StudentID", "SubjectID", "ExamDate", "Mark" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 8.50m },
                    { 2, 3, new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 7.00m },
                    { 2, 4, new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 9.00m },
                    { 2, 5, new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 6.50m },
                    { 3, 5, new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 5.50m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_SubjectID",
                table: "StudentGrades",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassID",
                table: "Students",
                column: "ClassID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentGrades");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
