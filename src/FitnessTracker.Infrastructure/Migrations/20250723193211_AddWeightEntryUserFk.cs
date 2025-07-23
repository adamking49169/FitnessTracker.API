using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightEntryUserFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WeightEntries_UserId",
                table: "WeightEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_Users_UserId",
                table: "WeightEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_Users_UserId",
                table: "WeightEntries");

            migrationBuilder.DropIndex(
                name: "IX_WeightEntries_UserId",
                table: "WeightEntries");
        }
    }
}
