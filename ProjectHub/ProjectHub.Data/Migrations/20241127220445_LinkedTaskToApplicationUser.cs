using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkedTaskToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedToUserId",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToUserId",
                table: "Tasks",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_AssignedToUserId",
                table: "Tasks",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_AssignedToUserId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedToUserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Tasks");
        }
    }
}
