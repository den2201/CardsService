using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardService.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "public",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CVC = table.Column<string>(type: "text", nullable: true),
                    Pan = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    CardName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "ExpiredDates",
                schema: "public",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiredDates", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_ExpiredDates_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "public",
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Cards",
                columns: new[] { "CardId", "CVC", "CardName", "IsDefault", "Pan", "UserId" },
                values: new object[,]
                {
                    { new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"), "123", "First card", true, "4397185796979658", new Guid("3bad8330-d287-4319-bb3f-1f9be9331814") },
                    { new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"), "666", "Second card", false, "2367000000019234", new Guid("3bad8330-d287-4319-bb3f-1f9be9331814") }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "ExpiredDates",
                columns: new[] { "CardId", "Month", "Year" },
                values: new object[,]
                {
                    { new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"), 12, 2023 },
                    { new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"), 5, 2019 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpiredDates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "public");
        }
    }
}
