using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cantin.Data.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "DebtProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ac8179a-3f45-40d2-ac0e-65d58333e265"),
                column: "ConcurrencyStamp",
                value: "67504e58-b45a-4fe8-ae32-c71eca1602a1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd76b238-de73-447e-bef6-424f84b844c8"),
                column: "ConcurrencyStamp",
                value: "c231ca9e-40e9-4f0f-8122-2abe2bf17cc3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eebcd6ba-d079-4fde-a81a-df80076c8002"),
                column: "ConcurrencyStamp",
                value: "4d09c2a6-3ade-4f96-91e9-0de5a85c112b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4083029d-7624-44d6-acfa-4a54deefbd3f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0c78796-4686-41be-80f9-09e3624f2077", "AQAAAAEAACcQAAAAEBcpvyJ99fHc9CpLdswG/dgrl/K/Rb6RoxP1bbEyefIbnbJT/IPmYp4eyM92kh9UlQ==", "1b4e8bd3-9aab-4410-af7d-206304cdea09" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6168a092-56d5-439d-a0b8-940fbda81950"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aa155b38-f754-44a8-955d-9937c903df0e", "AQAAAAEAACcQAAAAEHQjUvyCGHcMPQgASbcO/I/qDG4nfURuPGQAaFdvSGURcB/SpJ8l1jECN8TCSXSBpw==", "4ba1bdc9-9919-494b-a4bb-230f27d44f66" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("90411f34-b61a-4a4d-bbb1-6d98a2f9cf34"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 14, 0, 9, 25, 820, DateTimeKind.Local).AddTicks(1219));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "DebtProducts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ac8179a-3f45-40d2-ac0e-65d58333e265"),
                column: "ConcurrencyStamp",
                value: "5fb49f0d-f373-4d6c-a270-4cd58c3a8db3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd76b238-de73-447e-bef6-424f84b844c8"),
                column: "ConcurrencyStamp",
                value: "58970024-c277-452a-a535-6450624a33e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eebcd6ba-d079-4fde-a81a-df80076c8002"),
                column: "ConcurrencyStamp",
                value: "afdc4ea7-f049-4e1b-8715-a9be81e56963");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4083029d-7624-44d6-acfa-4a54deefbd3f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c24f334a-961e-409c-835e-478b68c9cdc4", "AQAAAAEAACcQAAAAEOCpXHwx+6w9vGe9XvYqXzGUABDCWRduBbk+syFhVXZAZGMG0GVl9VunPj9Yj4lcLg==", "bba32a34-d192-45d1-8d66-bf9fbacf50a8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6168a092-56d5-439d-a0b8-940fbda81950"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64e6e34c-884b-498a-8e42-2f757393e2cb", "AQAAAAEAACcQAAAAEAZcnSb9fm46lL/+/DiLjxvTwIrGal4ryLe8Tk0S3734mHJ53XI9vRCnkjwl06+APg==", "a7c4e5e9-9abb-4ed0-b10e-57304cda3c14" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("90411f34-b61a-4a4d-bbb1-6d98a2f9cf34"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 13, 20, 59, 54, 885, DateTimeKind.Local).AddTicks(7504));
        }
    }
}
