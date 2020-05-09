using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuitionApp.Infrastructure.Migrations
{
    public partial class addcapcityoncourseclss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_CourseClasses_CourseClassId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Enrollments");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseClassId",
                table: "Enrollments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "CourseClasses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttendanceStatus = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TimeslotId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Timeslots_TimeslotId",
                        column: x => x.TimeslotId,
                        principalTable: "Timeslots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EnrollmentId",
                table: "Attendances",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_TimeslotId",
                table: "Attendances",
                column: "TimeslotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_CourseClasses_CourseClassId",
                table: "Enrollments",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_CourseClasses_CourseClassId",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "CourseClasses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseClassId",
                table: "Enrollments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Enrollments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_CourseClasses_CourseClassId",
                table: "Enrollments",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
