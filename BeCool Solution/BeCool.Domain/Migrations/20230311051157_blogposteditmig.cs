using Microsoft.EntityFrameworkCore.Migrations;

namespace BeCool.Domain.Migrations
{
    public partial class blogposteditmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "BlogPostComments",
                type: "int",
                nullable: true);

            

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_CreatedByUserId",
                table: "BlogPostComments",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostComments_Users_CreatedByUserId",
                table: "BlogPostComments",
                column: "CreatedByUserId",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostComments_Users_CreatedByUserId",
                table: "BlogPostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Membership",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                schema: "Membership",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_BlogPostComments_CreatedByUserId",
                table: "BlogPostComments");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "BlogPostComments");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                schema: "Membership",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "Membership",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Roles_RoleId",
                schema: "Membership",
                table: "Roles",
                column: "RoleId",
                principalSchema: "Membership",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserId",
                schema: "Membership",
                table: "Roles",
                column: "UserId",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
