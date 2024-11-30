using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedAndMaxMilestonesProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxMilestones",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Milestones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxMilestones",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Milestones");
        }
    }
}
