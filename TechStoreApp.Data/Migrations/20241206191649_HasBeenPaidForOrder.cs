using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class HasBeenPaidForOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBeenPaidFor",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenPaidFor",
                table: "Orders");
        }
    }
}
