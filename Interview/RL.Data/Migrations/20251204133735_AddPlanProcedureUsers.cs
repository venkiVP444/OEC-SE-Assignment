using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RL.Data.Migrations
{
    public partial class AddPlanProcedureUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanProcedures",
                table: "PlanProcedures");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlanProcedures",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanProcedures",
                table: "PlanProcedures",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PlanProcedureUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlanProcedureId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanProcedureUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanProcedureUsers_PlanProcedures_PlanProcedureId",
                        column: x => x.PlanProcedureId,
                        principalTable: "PlanProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProcedureUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcedures_PlanId",
                table: "PlanProcedures",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcedureUsers_PlanProcedureId",
                table: "PlanProcedureUsers",
                column: "PlanProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcedureUsers_UserId",
                table: "PlanProcedureUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanProcedureUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanProcedures",
                table: "PlanProcedures");

            migrationBuilder.DropIndex(
                name: "IX_PlanProcedures_PlanId",
                table: "PlanProcedures");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlanProcedures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanProcedures",
                table: "PlanProcedures",
                columns: new[] { "PlanId", "ProcedureId" });
        }
    }
}
