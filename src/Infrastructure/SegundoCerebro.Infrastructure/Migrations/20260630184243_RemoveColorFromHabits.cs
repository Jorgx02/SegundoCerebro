using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SegundoCerebro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColorFromHabits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Habits");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Habits",
                type: "text",
                nullable: true);
        }
    }
}
