using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingMoreModels1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInShoppingCart_Products_ProductId",
                table: "ProductInShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInShoppingCart_ShoppingCart_ShoppingCartId",
                table: "ProductInShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_OwnerId",
                table: "ShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCart",
                table: "ShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInShoppingCart",
                table: "ProductInShoppingCart");

            migrationBuilder.RenameTable(
                name: "ShoppingCart",
                newName: "ShoppingCarts");

            migrationBuilder.RenameTable(
                name: "ProductInShoppingCart",
                newName: "ProductInShoppingCarts");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCart_OwnerId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInShoppingCart_ShoppingCartId",
                table: "ProductInShoppingCarts",
                newName: "IX_ProductInShoppingCarts_ShoppingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInShoppingCart_ProductId",
                table: "ProductInShoppingCarts",
                newName: "IX_ProductInShoppingCarts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInShoppingCarts",
                table: "ProductInShoppingCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInShoppingCarts_Products_ProductId",
                table: "ProductInShoppingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInShoppingCarts_ShoppingCarts_ShoppingCartId",
                table: "ProductInShoppingCarts",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_OwnerId",
                table: "ShoppingCarts",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInShoppingCarts_Products_ProductId",
                table: "ProductInShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInShoppingCarts_ShoppingCarts_ShoppingCartId",
                table: "ProductInShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_OwnerId",
                table: "ShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInShoppingCarts",
                table: "ProductInShoppingCarts");

            migrationBuilder.RenameTable(
                name: "ShoppingCarts",
                newName: "ShoppingCart");

            migrationBuilder.RenameTable(
                name: "ProductInShoppingCarts",
                newName: "ProductInShoppingCart");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_OwnerId",
                table: "ShoppingCart",
                newName: "IX_ShoppingCart_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInShoppingCarts_ShoppingCartId",
                table: "ProductInShoppingCart",
                newName: "IX_ProductInShoppingCart_ShoppingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInShoppingCarts_ProductId",
                table: "ProductInShoppingCart",
                newName: "IX_ProductInShoppingCart_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCart",
                table: "ShoppingCart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInShoppingCart",
                table: "ProductInShoppingCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInShoppingCart_Products_ProductId",
                table: "ProductInShoppingCart",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInShoppingCart_ShoppingCart_ShoppingCartId",
                table: "ProductInShoppingCart",
                column: "ShoppingCartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_OwnerId",
                table: "ShoppingCart",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
