using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Infrastructure;
using EstoqueLocal.Infrastructure.Services;
using EstoqueLocal.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IEntradaService, EntradaService>();
builder.Services.AddScoped<ISaidaService, SaidaService>();
builder.Services.AddScoped<IConferenciaService, ConferenciaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DatabaseSeeder.Seed(db);
}

app.Run();
