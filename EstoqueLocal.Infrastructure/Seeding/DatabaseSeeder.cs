using EstoqueLocal.Domain.Entities;

namespace EstoqueLocal.Infrastructure.Seeding;

public static class DatabaseSeeder
{
    public static void Seed(AppDbContext ctx)
    {
        if (ctx.Itens.Any()) return;

        ctx.Itens.AddRange(
            new Item { Nome = "Cimento CP-II", Categoria = "Construção", Unidade = "Saco", QuantidadeInicial = 100, ConsumoMedio = ConsumoMedio.Medio, Ativo = true },
            new Item { Nome = "Areia", Categoria = "Construção", Unidade = "m3", QuantidadeInicial = 50, ConsumoMedio = ConsumoMedio.Alto, Ativo = true },
            new Item { Nome = "Tinta Branca", Categoria = "Acabamento", Unidade = "Galão", QuantidadeInicial = 20, ConsumoMedio = ConsumoMedio.Baixo, Ativo = true },
            new Item { Nome = "Cabo 2,5mm", Categoria = "Elétrica", Unidade = "Rolo", QuantidadeInicial = 30, ConsumoMedio = ConsumoMedio.Medio, Ativo = true },
            new Item { Nome = "Lâmpada LED", Categoria = "Elétrica", Unidade = "Unidade", QuantidadeInicial = 200, ConsumoMedio = ConsumoMedio.Alto, Ativo = true }
        );
        ctx.SaveChanges();
    }
}
