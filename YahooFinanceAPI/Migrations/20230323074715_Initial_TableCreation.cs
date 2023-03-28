using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YahooFinanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial_TableCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "StockHistoricalDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(20,8)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(20,8)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(20,8)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(20,8)", nullable: false),
                    AdjustedClose = table.Column<decimal>(type: "decimal(20,8)", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistoricalDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHistoricalDatas_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockHistoricalDatas_StockId",
                table: "StockHistoricalDatas",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockHistoricalDatas");

            migrationBuilder.DropTable(
                name: "Stocks");
        }
    }
}
