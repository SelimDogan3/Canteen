using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cantin.Data.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ManualStockReductionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DebtId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHistories_Debts_DebtId",
                        column: x => x.DebtId,
                        principalTable: "Debts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockHistories_ManuelStockReductions_ManualStockReductionId",
                        column: x => x.ManualStockReductionId,
                        principalTable: "ManuelStockReductions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockHistories_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockHistories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockHistories_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ac8179a-3f45-40d2-ac0e-65d58333e265"),
                column: "ConcurrencyStamp",
                value: "926ac771-1f85-424a-b0a1-85c45a961728");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd76b238-de73-447e-bef6-424f84b844c8"),
                column: "ConcurrencyStamp",
                value: "51479648-6be7-405b-9ccd-6f5f728169be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eebcd6ba-d079-4fde-a81a-df80076c8002"),
                column: "ConcurrencyStamp",
                value: "498cb781-243c-42c8-bb07-48ae7860b733");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6168a092-56d5-439d-a0b8-940fbda81950"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e0ea0d0-e66d-48a9-8a2e-36d2b8657b1e", "AQAAAAEAACcQAAAAEERexcdh4e2yXKtRiV9TgTjIx9srnGT0ysZwp8h0yVG47lhQ+ss4QbhraBtYA9q8YQ==", "cd9d1426-4773-4708-8c20-e2ff759ad639" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("90411f34-b61a-4a4d-bbb1-6d98a2f9cf34"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 25, 22, 19, 2, 566, DateTimeKind.Local).AddTicks(1454));

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_DebtId",
                table: "StockHistories",
                column: "DebtId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_ManualStockReductionId",
                table: "StockHistories",
                column: "ManualStockReductionId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_SaleId",
                table: "StockHistories",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_StoreId",
                table: "StockHistories",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_SupplyId",
                table: "StockHistories",
                column: "SupplyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockHistories");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ac8179a-3f45-40d2-ac0e-65d58333e265"),
                column: "ConcurrencyStamp",
                value: "59e3cfef-04ea-49aa-8d47-c0acf5b2d5a6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd76b238-de73-447e-bef6-424f84b844c8"),
                column: "ConcurrencyStamp",
                value: "6aa7b3a6-0380-4670-a55e-f82ba3ce7654");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eebcd6ba-d079-4fde-a81a-df80076c8002"),
                column: "ConcurrencyStamp",
                value: "7dc4bdab-9fba-4081-9a30-300342ec276a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6168a092-56d5-439d-a0b8-940fbda81950"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f8851967-2799-4fbe-a616-3f8d5e62bf03", "AQAAAAEAACcQAAAAEOWM/aDw5GYJCw7CZIlQ0Q2tF0WkV2cyPCUB7H46Kt6cBJlwpZzmIBNP5LdJT2ClRg==", "12984c11-0761-40d2-938c-fefda55c57b9" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("90411f34-b61a-4a4d-bbb1-6d98a2f9cf34"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 25, 17, 18, 26, 164, DateTimeKind.Local).AddTicks(8231));
        }
    }
}
