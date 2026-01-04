using EstoqueLocal.Domain.Entities;
using Finance = EstoqueLocal.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLocal.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<Item> Itens => Set<Item>();
    public DbSet<EntradaEstoque> Entradas => Set<EntradaEstoque>();
    public DbSet<SaidaConsumo> Saidas => Set<SaidaConsumo>();
    public DbSet<Conferencia> Conferencias => Set<Conferencia>();
    public DbSet<ConferenciaLinha> ConferenciaLinhas => Set<ConferenciaLinha>();
    public DbSet<Finance.CategoriaFinanceira> CategoriasFinanceiras => Set<Finance.CategoriaFinanceira>();
    public DbSet<Finance.LancamentoCaixa> LancamentosCaixa => Set<Finance.LancamentoCaixa>();
    public DbSet<Finance.ContaAPagar> ContasAPagar => Set<Finance.ContaAPagar>();
    public DbSet<Finance.ContaAReceber> ContasAReceber => Set<Finance.ContaAReceber>();
    public DbSet<Finance.Compra> Compras => Set<Finance.Compra>();
    public DbSet<Finance.CompraItem> CompraItens => Set<Finance.CompraItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(e =>
        {
            e.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            e.Property(p => p.Categoria).HasMaxLength(100);
            e.Property(p => p.Unidade).HasMaxLength(50);
            e.Property(p => p.QuantidadeInicial).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<EntradaEstoque>(e =>
        {
            e.Property(p => p.Quantidade).HasColumnType("decimal(18,2)");
            e.Property(p => p.ValorTotal).HasColumnType("decimal(18,2)");
            e.Property(p => p.Fornecedor).HasMaxLength(200);
            e.HasOne(i => i.Item).WithMany(i => i.Entradas).HasForeignKey(i => i.ItemId);
        });

        modelBuilder.Entity<SaidaConsumo>(e =>
        {
            e.Property(p => p.Quantidade).HasColumnType("decimal(18,2)");
            e.Property(p => p.Motivo).HasMaxLength(200);
            e.HasOne(i => i.Item).WithMany(i => i.Saidas).HasForeignKey(i => i.ItemId);
        });

        modelBuilder.Entity<Conferencia>(e =>
        {
            e.HasMany(c => c.Linhas).WithOne(l => l.Conferencia).HasForeignKey(l => l.ConferenciaId);
        });

        modelBuilder.Entity<ConferenciaLinha>(e =>
        {
            e.Property(p => p.QuantidadeContada).HasColumnType("decimal(18,2)");
            e.HasOne(l => l.Item).WithMany(i => i.ConferenciaLinhas).HasForeignKey(l => l.ItemId);
        });

        modelBuilder.Entity<Finance.CategoriaFinanceira>(e =>
        {
            e.Property(p => p.Nome).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Finance.LancamentoCaixa>(e =>
        {
            e.Property(p => p.Descricao).IsRequired().HasMaxLength(300);
            e.Property(p => p.Valor).HasColumnType("decimal(18,2)");
            e.Property(p => p.DataHora).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Finance.ContaAPagar>(e =>
        {
            e.Property(p => p.Descricao).IsRequired().HasMaxLength(300);
            e.Property(p => p.Valor).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Finance.ContaAReceber>(e =>
        {
            e.Property(p => p.Descricao).IsRequired().HasMaxLength(300);
            e.Property(p => p.Valor).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Finance.Compra>(e =>
        {
            e.Property(p => p.Fornecedor).HasMaxLength(200);
            e.Property(p => p.ValorTotal).HasColumnType("decimal(18,2)");
            e.Property(p => p.DataHora).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Finance.CompraItem>(e =>
        {
            e.Property(p => p.DescricaoItem).HasMaxLength(200);
            e.Property(p => p.CategoriaItem).HasMaxLength(100);
            e.Property(p => p.Unidade).HasMaxLength(50);
            e.Property(p => p.Quantidade).HasColumnType("decimal(18,2)");
            e.Property(p => p.Valor).HasColumnType("decimal(18,2)");
        });
    }
}
