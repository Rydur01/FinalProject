using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DependencyInjectionDemo.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ToDos",
                columns: new[] { "Id", "DueDate", "Name" },
                values: new object[] { 1, new DateTime(2022, 11, 3, 0, 0, 0, 0, DateTimeKind.Local), "Homework" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ToDos",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
