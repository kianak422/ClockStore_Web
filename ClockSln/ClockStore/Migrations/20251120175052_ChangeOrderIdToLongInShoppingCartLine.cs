using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClockStore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderIdToLongInShoppingCartLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartLine_Orders_OrderID",
                table: "ShoppingCartLine");

            // Drop primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartLine",
                table: "ShoppingCartLine");

            // Alter column type
            migrationBuilder.AlterColumn<long>(
                name: "OrderID",
                table: "ShoppingCartLine",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Recreate primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartLine",
                table: "ShoppingCartLine",
                columns: new[] { "OrderID", "Id" });

            // Recreate foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartLine_Orders_OrderID",
                table: "ShoppingCartLine",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartLine_Orders_OrderID",
                table: "ShoppingCartLine");

            // Drop primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartLine",
                table: "ShoppingCartLine");

            // Alter column type back to int
            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "ShoppingCartLine",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            // Recreate primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartLine",
                table: "ShoppingCartLine",
                columns: new[] { "OrderID", "Id" });

            // Recreate foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartLine_Orders_OrderID",
                table: "ShoppingCartLine",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
