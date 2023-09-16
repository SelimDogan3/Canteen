using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cantin.Data.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "Debts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Debts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ac8179a-3f45-40d2-ac0e-65d58333e265"),
                column: "ConcurrencyStamp",
                value: "42665a05-f0c8-44ca-a06b-ecbd0465dfbc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd76b238-de73-447e-bef6-424f84b844c8"),
                column: "ConcurrencyStamp",
                value: "769a5aab-b9a2-4a56-8bbf-9dc0b18cc70a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eebcd6ba-d079-4fde-a81a-df80076c8002"),
                column: "ConcurrencyStamp",
                value: "823a6d7d-04f3-4500-89a9-e5f6a457e6cf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4083029d-7624-44d6-acfa-4a54deefbd3f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86c1faa9-4fdb-403e-aa69-c29c8416594a", "AQAAAAEAACcQAAAAEIYDdDK7DH4jPKAIdoRrIggtGaUedJmM4ydyg8OUpXkl/nj6Ge89ANSSy8ofFAj5PQ==", "dbc5ab1f-8fa9-4e4f-91ec-4eb17b495053" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6168a092-56d5-439d-a0b8-940fbda81950"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8582f4d5-f417-44db-bf7f-b3d980c5d923", "AQAAAAEAACcQAAAAEMI2+x5icC3DsZBO5JMRV0syrmJV9bf4urN+YasC/Puo2krNoXvQl2sIFhEdoIFk/w==", "39ddb8a9-65c6-4ed0-9118-41467e124277" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("90411f34-b61a-4a4d-bbb1-6d98a2f9cf34"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 11, 19, 42, 0, 878, DateTimeKind.Local).AddTicks(6922));
        }
    }
}
