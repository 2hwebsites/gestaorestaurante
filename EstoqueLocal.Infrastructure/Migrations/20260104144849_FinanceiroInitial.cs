using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstoqueLocal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinanceiroInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasFinanceiras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TipoPadrao = table.Column<int>(type: "int", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasFinanceiras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContasAReceber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataRecebimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FormaRecebimento = table.Column<int>(type: "int", nullable: false),
                    OrigemVenda = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasAReceber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Fornecedor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormaPagamento = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlocacaoCusto = table.Column<int>(type: "int", nullable: false),
                    CategoriaFinanceiraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compras_CategoriasFinanceiras_CategoriaFinanceiraId",
                        column: x => x.CategoriaFinanceiraId,
                        principalTable: "CategoriasFinanceiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContasAPagar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CategoriaFinanceiraId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FormaPagamento = table.Column<int>(type: "int", nullable: true),
                    AlocacaoCusto = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasAPagar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasAPagar_CategoriasFinanceiras_CategoriaFinanceiraId",
                        column: x => x.CategoriaFinanceiraId,
                        principalTable: "CategoriasFinanceiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LancamentosCaixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    OrigemVenda = table.Column<int>(type: "int", nullable: true),
                    CategoriaFinanceiraId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormaPagamento = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentosCaixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentosCaixa_CategoriasFinanceiras_CategoriaFinanceiraId",
                        column: x => x.CategoriaFinanceiraId,
                        principalTable: "CategoriasFinanceiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompraItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompraId = table.Column<int>(type: "int", nullable: false),
                    DescricaoItem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoriaItem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompraItens_Compras_CompraId",
                        column: x => x.CompraId,
                        principalTable: "Compras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompraItens_CompraId",
                table: "CompraItens",
                column: "CompraId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CategoriaFinanceiraId",
                table: "Compras",
                column: "CategoriaFinanceiraId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasAPagar_CategoriaFinanceiraId",
                table: "ContasAPagar",
                column: "CategoriaFinanceiraId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosCaixa_CategoriaFinanceiraId",
                table: "LancamentosCaixa",
                column: "CategoriaFinanceiraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraItens");

            migrationBuilder.DropTable(
                name: "ContasAPagar");

            migrationBuilder.DropTable(
                name: "ContasAReceber");

            migrationBuilder.DropTable(
                name: "LancamentosCaixa");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "CategoriasFinanceiras");
        }
    }
}
