using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace forum.Migrations
{
    /// <inheritdoc />
    public partial class MessageUpdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "dislikes",
                table: "Post",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "likes",
                table: "Post",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dislikes",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "likes",
                table: "Post");
        }
    }
}
