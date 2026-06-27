using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SegundoCerebro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Projects_ProjectId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Projects",
                newName: "DueDate");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Projects_ProjectId",
                table: "TodoItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Projects_ProjectId",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Projects",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Projects_ProjectId",
                table: "TodoItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
