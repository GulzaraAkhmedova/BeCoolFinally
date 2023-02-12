using Microsoft.EntityFrameworkCore.Migrations;

namespace BeCool.Domain.Migrations
{
    public partial class AddDeletedByUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Subscribes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "ProductColors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "ProductTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "ProductSizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "ProductImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "ProductCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "ProductMaterials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Faqs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "ContactPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "BlogPostTagCloud",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "BlogPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "BlogPostComments",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Subscribes");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ProductCatalog");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ProductMaterials");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Faqs");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ContactPosts");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "BlogPostTagCloud");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "BlogPostComments");
        }
    }
}
