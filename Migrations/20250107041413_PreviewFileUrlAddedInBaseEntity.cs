using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imarat_Shariah.Migrations
{
    /// <inheritdoc />
    public partial class PreviewFileUrlAddedInBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewFileUrl",
                table: "siyajats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewFileUrl",
                table: "khulas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewFileUrl",
                table: "siyajats");

            migrationBuilder.DropColumn(
                name: "PreviewFileUrl",
                table: "khulas");
        }
    }
}
