using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersPlatform.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateCompanyBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Companies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Companies");
        }
    }
}
