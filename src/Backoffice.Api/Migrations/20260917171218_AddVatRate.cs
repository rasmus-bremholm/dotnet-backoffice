using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backoffice.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVatRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VatRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_VatRateId",
                table: "Products",
                column: "VatRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VatRates_VatRateId",
                table: "Products",
                column: "VatRateId",
                principalTable: "VatRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_VatRates_VatRateId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "VatRates");

            migrationBuilder.DropIndex(
                name: "IX_Products_VatRateId",
                table: "Products");
        }
    }
}
