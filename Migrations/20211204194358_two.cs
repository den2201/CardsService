using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardService.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "CardDateExpired",
                columns: new[] { "CardId", "Month", "Year" },
                values: new object[,]
                {
                    { new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"), 12, 2023 },
                    { new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"), 5, 2030 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "CardDateExpired",
                keyColumn: "CardId",
                keyValue: new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "CardDateExpired",
                keyColumn: "CardId",
                keyValue: new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"));
        }
    }
}
