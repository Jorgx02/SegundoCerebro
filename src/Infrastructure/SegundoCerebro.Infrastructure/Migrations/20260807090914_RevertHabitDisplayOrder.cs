using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SegundoCerebro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RevertHabitDisplayOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Habits");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Habits",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
