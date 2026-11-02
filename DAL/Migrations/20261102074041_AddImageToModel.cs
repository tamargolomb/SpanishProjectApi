using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsAndRatings_Users_UsersUserId",
                table: "ReviewsAndRatings");

            migrationBuilder.DropIndex(
                name: "IX_ReviewsAndRatings_UsersUserId",
                table: "ReviewsAndRatings");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "ReviewsAndRatings");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LevelId",
                table: "Users",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Levels_LevelId",
                table: "Users",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Levels_LevelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LevelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "image",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "UsersUserId",
                table: "ReviewsAndRatings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsAndRatings_UsersUserId",
                table: "ReviewsAndRatings",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsAndRatings_Users_UsersUserId",
                table: "ReviewsAndRatings",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
