using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstoqueLocal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinanceiroAdjustCaixaLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LancamentoCaixaId",
                table: "ContasAReceber",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LancamentoCaixaId",
                table: "ContasAPagar",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LancamentoCaixaId",
                table: "ContasAReceber");

            migrationBuilder.DropColumn(
                name: "LancamentoCaixaId",
                table: "ContasAPagar");
        }
    }
}
