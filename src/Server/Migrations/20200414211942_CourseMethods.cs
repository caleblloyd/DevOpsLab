using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevOpsLab.Server.Migrations
{
    public partial class CourseMethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpiresAfter",
                table: "TrainingCodes",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "TrainingCodeAppUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "TrainingCodeAppUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Expires",
                table: "TrainingCodeAppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Tracks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tracks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Step",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Step",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Scenarios",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Scenarios",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Scenarios",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Courses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Courses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_Alias",
                table: "Tracks",
                column: "Alias");

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_Alias",
                table: "Scenarios",
                column: "Alias");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Alias",
                table: "Courses",
                column: "Alias");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tracks_Alias",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Scenarios_Alias",
                table: "Scenarios");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Alias",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ExpiresAfter",
                table: "TrainingCodes");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "TrainingCodeAppUsers");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "TrainingCodeAppUsers");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "TrainingCodeAppUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
