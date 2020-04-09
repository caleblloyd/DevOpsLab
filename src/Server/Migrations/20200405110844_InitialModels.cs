using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevOpsLab.Server.Migrations
{
    public partial class InitialModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Rank = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Code = table.Column<string>(nullable: false),
                    MaxUsers = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Rank = table.Column<double>(nullable: true),
                    CourseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scenarios_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Rank = table.Column<double>(nullable: true),
                    TrackId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackCourses_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingCodeAppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TrainingCodeId = table.Column<Guid>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCodeAppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingCodeAppUsers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingCodeAppUsers_TrainingCodes_TrainingCodeId",
                        column: x => x.TrainingCodeId,
                        principalTable: "TrainingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingCodeTracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Rank = table.Column<double>(nullable: true),
                    TrainingCodeId = table.Column<Guid>(nullable: false),
                    TrackId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCodeTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingCodeTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingCodeTracks_TrainingCodes_TrainingCodeId",
                        column: x => x.TrainingCodeId,
                        principalTable: "TrainingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Rank = table.Column<double>(nullable: true),
                    ScenarioId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Step_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_CourseId",
                table: "Scenarios",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Step_ScenarioId",
                table: "Step",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackCourses_CourseId",
                table: "TrackCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackCourses_Rank",
                table: "TrackCourses",
                column: "Rank");

            migrationBuilder.CreateIndex(
                name: "IX_TrackCourses_TrackId",
                table: "TrackCourses",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_Rank",
                table: "Tracks",
                column: "Rank");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCodeAppUsers_AppUserId",
                table: "TrainingCodeAppUsers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCodeAppUsers_TrainingCodeId",
                table: "TrainingCodeAppUsers",
                column: "TrainingCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCodes_Code",
                table: "TrainingCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCodeTracks_Rank",
                table: "TrainingCodeTracks",
                column: "Rank");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCodeTracks_TrackId",
                table: "TrainingCodeTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCodeTracks_TrainingCodeId",
                table: "TrainingCodeTracks",
                column: "TrainingCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Step");

            migrationBuilder.DropTable(
                name: "TrackCourses");

            migrationBuilder.DropTable(
                name: "TrainingCodeAppUsers");

            migrationBuilder.DropTable(
                name: "TrainingCodeTracks");

            migrationBuilder.DropTable(
                name: "Scenarios");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "TrainingCodes");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
