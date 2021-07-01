using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningPlan.DataAccess.Implementation.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotSubscriptions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ChatId = table.Column<string>(nullable: false),
                    PlanId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotSubscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanAreas",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PlanId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanAreas_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaTopics",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    PlanAreaId = table.Column<string>(nullable: true),
                    PlanId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaTopics_PlanAreas_PlanAreaId",
                        column: x => x.PlanAreaId,
                        principalTable: "PlanAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaTopics_PlanAreaId",
                table: "AreaTopics",
                column: "PlanAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BotSubscriptions_PlanId",
                table: "BotSubscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanAreas_PlanId",
                table: "PlanAreas",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaTopics");

            migrationBuilder.DropTable(
                name: "BotSubscriptions");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PlanAreas");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
