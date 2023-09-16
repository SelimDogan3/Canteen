using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cantin.Data.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Exchange",
                table: "Debts",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PaidAmount",
                table: "Debts",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Debts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ac8179a-3f45-40d2-ac0e-65d58333e265"),
                column: "ConcurrencyStamp",
                value: "70931e0c-34cf-4c48-8d4f-f051fb1860cd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd76b238-de73-447e-bef6-424f84b844c8"),
                column: "ConcurrencyStamp",
                value: "c8b0e476-3bbb-4da4-af5a-598ae3d4d432");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eebcd6ba-d079-4fde-a81a-df80076c8002"),
                column: "ConcurrencyStamp",
                value: "851375b8-04e9-4f08-8317-5f6351360cc3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4083029d-7624-44d6-acfa-4a54deefbd3f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05b12c88-f7c3-4f08-b6d8-4d4c6c3bb652", "AQAAAAEAACcQAAAAEABIRLQF2nuF8pqVCmgkSwm7v0imCo6B18swcuvIpjrD1AjK7W6AP76uELya7JCz/g==", "6abc6e78-9e35-4f11-8ecb-27453f83c2b5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6168a092-56d5-439d-a0b8-940fbda81950"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d084e1b1-12a5-41ad-989b-6f3d7f768f2a", "AQAAAAEAACcQAAAAEC8edOXBcg4XslYUI1wmZNhuX62xtMtGGf2GycCooaPipQKjz6HFuJjzOwMwUSipxA==", "9af13ee9-00b5-430b-9a63-1cadd83946b3" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("90411f34-b61a-4a4d-bbb1-6d98a2f9cf34"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 14, 1, 0, 40, 917, DateTimeKind.Local).AddTicks(667));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exchange",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Debts");

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
    }
}
