using Microsoft.EntityFrameworkCore;
using ForgingDwarf.Common.Models;

namespace ForgingDwarf.Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)          // У заказа один клиент
                .WithMany(c => c.Orders)        // У клиента много заказов
                .HasForeignKey(o => o.ClientId) // Внешний ключ
                .OnDelete(DeleteBehavior.Cascade);

            // Конвертируем enum в строки для БД
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.ItemType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.SteelType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.Style)
                .HasConversion<string>();
        }
    }
}