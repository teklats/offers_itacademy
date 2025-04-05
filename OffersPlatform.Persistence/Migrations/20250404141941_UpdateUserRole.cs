using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersPlatform.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Categories_CategoryId",
                table: "UserPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Categories_CategoryId1",
                table: "UserPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Users_UserId",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences");

            migrationBuilder.RenameTable(
                name: "UserPreferences",
                newName: "UserCategories");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_CategoryId1",
                table: "UserCategories",
                newName: "IX_UserCategories_CategoryId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_CategoryId",
                table: "UserCategories",
                newName: "IX_UserCategories_CategoryId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCategories",
                table: "UserCategories",
                columns: new[] { "UserId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCategories_Categories_CategoryId",
                table: "UserCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCategories_Categories_CategoryId1",
                table: "UserCategories",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCategories_Users_UserId",
                table: "UserCategories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCategories_Categories_CategoryId",
                table: "UserCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCategories_Categories_CategoryId1",
                table: "UserCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCategories_Users_UserId",
                table: "UserCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCategories",
                table: "UserCategories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserCategories");

            migrationBuilder.RenameTable(
                name: "UserCategories",
                newName: "UserPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_UserCategories_CategoryId1",
                table: "UserPreferences",
                newName: "IX_UserPreferences_CategoryId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserCategories_CategoryId",
                table: "UserPreferences",
                newName: "IX_UserPreferences_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences",
                columns: new[] { "UserId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Categories_CategoryId",
                table: "UserPreferences",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Categories_CategoryId1",
                table: "UserPreferences",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Users_UserId",
                table: "UserPreferences",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
