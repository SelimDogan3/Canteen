using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cantin.Data.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Debts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Debts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DebtProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "DebtProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DebtProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "DebtProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DebtProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "DebtProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "DebtProducts",
                type: "datetime2",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DebtProducts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "DebtProducts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DebtProducts");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DebtProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DebtProducts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "DebtProducts");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "DebtProducts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ac8179a-3f45-40d2-ac0e-65d58333e265"),
                column: "ConcurrencyStamp",
                value: "f603634a-baea-4692-bec9-d82ef132a214");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd76b238-de73-447e-bef6-424f84b844c8"),
                column: "ConcurrencyStamp",
                value: "ec7d43cd-aa86-477d-8f55-e14806f3ee5e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eebcd6ba-d079-4fde-a81a-df80076c8002"),
                column: "ConcurrencyStamp",
                value: "2685194b-067b-426c-935c-a411e4f684b6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4083029d-7624-44d6-acfa-4a54deefbd3f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8c9235fd-0d89-4c1a-9408-24accf33e1fa", "AQAAAAEAACcQAAAAEG2IyVPUZVo909pyGtHWUKa3Rn819Sp2CERFuC2eIZ99bP3nhG3acpKIxUctFh//sA==", "707eb9c3-d171-434c-95cc-7c859e9898da" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6168a092-56d5-439d-a0b8-940fbda81950"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd8711ea-bf3a-43a7-b319-f6ff500b43f0", "AQAAAAEAACcQAAAAEB8dtUD9mBLK/3D2nw3v1xHMdG/jAVBotlCqLeF9ygCpNshaS91CtYkbY1J22zp9pQ==", "8c4adcc3-b989-4d81-a1d8-2104cc847973" });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("90411f34-b61a-4a4d-bbb1-6d98a2f9cf34"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 10, 11, 35, 4, 802, DateTimeKind.Local).AddTicks(9250));
        }
    }
}
