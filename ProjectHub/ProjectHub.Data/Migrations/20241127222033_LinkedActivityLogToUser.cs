using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkedActivityLogToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ActivityLogs");

            migrationBuilder.RenameColumn(
                name: "ActionDate",
                table: "ActivityLogs",
                newName: "Timestamp");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "ActivityLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ActivityLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ApplicationUserId",
                table: "ActivityLogs",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_AspNetUsers_ApplicationUserId",
                table: "ActivityLogs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_AspNetUsers_UserId",
                table: "ActivityLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_AspNetUsers_ApplicationUserId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_AspNetUsers_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_ApplicationUserId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ActivityLogs");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "ActivityLogs",
                newName: "ActionDate");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ActivityLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
