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
            // Конфигурация для Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                      .ValueGeneratedOnAdd(); // Автоинкремент

                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Password)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(c => c.IsSuperuser)
                      .HasDefaultValue(false);
            });

            // Конфигурация для Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id)
                      .ValueGeneratedOnAdd(); // Явно указываем автоинкремент

                // Связь с Client
                entity.HasOne(o => o.Client)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.ClientId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Конвертация enum
                entity.Property(o => o.Status)
                      .HasConversion<string>();

                entity.Property(o => o.ItemType)
                      .HasConversion<string>();

                entity.Property(o => o.SteelType)
                      .HasConversion<string>();

                entity.Property(o => o.Style)
                      .HasConversion<string>();

                // Настройка других свойств
                entity.Property(o => o.Price)
                      .HasColumnType("decimal(18,2)"); // Для точного хранения денежных значений
            });
        }
    }
}