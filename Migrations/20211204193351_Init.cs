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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CVC = table.Column<string>(type: "text", nullable: true),
                    Pan = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    CardName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardDateExpired",
                schema: "public",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDateExpired", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_CardDateExpired_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "public",
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Cards",
                columns: new[] { "Id", "CVC", "CardName", "IsDefault", "Pan", "UserId" },
                values: new object[,]
                {
                    { new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"), "123", "First card", true, "4397185796979658", new Guid("3bad8330-d287-4319-bb3f-1f9be9331814") },
                    { new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"), "666", "Second card", false, "2367000000019234", new Guid("3bad8330-d287-4319-bb3f-1f9be9331814") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardDateExpired",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "public");
        }
    }
}
