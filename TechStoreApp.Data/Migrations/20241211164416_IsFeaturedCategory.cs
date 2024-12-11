using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsFeaturedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Categories");
        }
    }
}
