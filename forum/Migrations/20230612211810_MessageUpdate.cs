using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace forum.Migrations
{
    /// <inheritdoc />
    public partial class MessageUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receiverId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "senderId",
                table: "Message");

            migrationBuilder.AddColumn<string>(
                name: "receiverEmail",
                table: "Message",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "senderEmail",
                table: "Message",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receiverEmail",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "senderEmail",
                table: "Message");

            migrationBuilder.AddColumn<int>(
                name: "receiverId",
                table: "Message",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "senderId",
                table: "Message",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
