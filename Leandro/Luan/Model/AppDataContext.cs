using Microsoft.EntityFrameworkCore;
using Luan.Models;

namespace Luan.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }
    public DbSet<Consumo> Consumos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Caminho do banco de dados SQLite
        optionsBuilder.UseSqlite("Data Source=Leandro_Luan.db");
    }
}
