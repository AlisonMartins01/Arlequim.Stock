using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arlequim.Stock.Infrastructure.Persistence
{
    public class AppDbContext :  DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<StockEntry> StockEntries => Set<StockEntry>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        internal sealed class UserMap : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> b)
            {
                b.ToTable("Users");

                b.HasKey(x => x.Id);

                b.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                b.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                b.HasIndex(x => x.Email)
                    .IsUnique();

                b.Property(x => x.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(300);

                // Mapeia o enum como int (padrão). Se quiser string: .HasConversion<string>()
                b.Property(x => x.Role)
                    .IsRequired();

                b.HasMany(u => u.Orders)
                     .WithOne(o => o.User)
                     .HasForeignKey(o => o.UserId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Restrict);

                b.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")   // SQL Server
                    .ValueGeneratedOnAdd();
            }
        }

        internal sealed class ProductMap : IEntityTypeConfiguration<Product>
        {
            public void Configure(EntityTypeBuilder<Product> b)
            {
                b.ToTable("Products");

                b.HasKey(x => x.Id);

                b.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property(x => x.Description)
                    .HasMaxLength(2000);

                b.Property(x => x.Price)
                    .HasPrecision(18, 2)
                    .IsRequired();

                b.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                b.Property(x => x.CurrentStock)
                .IsRequired();

                b.Property(x => x.RowVersion)
                 .IsRowVersion();

                b.HasMany(x => x.StockEntries)
                    .WithOne(x => x.Product!)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(x => x.OrderItems)
                    .WithOne(x => x.Product!)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

        internal sealed class StockEntryMap : IEntityTypeConfiguration<StockEntry>
        {
            public void Configure(EntityTypeBuilder<StockEntry> b)
            {
                b.ToTable("StockEntries");

                b.HasKey(x => x.Id);

                b.Property(x => x.ProductId)
                    .IsRequired();

                b.Property(x => x.Quantity)
                    .IsRequired();

                b.Property(x => x.InvoiceNumber)
                    .IsRequired()
                    .HasMaxLength(60);

                b.HasIndex(x => x.InvoiceNumber); 

                b.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();
            }
        }

        internal sealed class OrderMap : IEntityTypeConfiguration<Order>
        {
            public void Configure(EntityTypeBuilder<Order> b)
            {
                b.ToTable("Orders");

                b.HasKey(x => x.Id);

                b.Property(x => x.CustomerDocument)
                    .IsRequired()
                    .HasMaxLength(40);

                b.Property(x => x.SellerName)
                    .IsRequired()
                    .HasMaxLength(150);

                b.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                b.HasMany(x => x.Items)
                    .WithOne(x => x.Order)
                    .HasForeignKey(x => x.OrderId)
                    .OnDelete(DeleteBehavior.Cascade); 

                b.HasIndex(o => o.UserId);
            }
        }

        internal sealed class OrderItemMap : IEntityTypeConfiguration<OrderItem>
        {
            public void Configure(EntityTypeBuilder<OrderItem> b)
            {
                b.ToTable("OrderItems");

                b.HasKey(x => x.Id);

                b.Property(x => x.OrderId)
                    .IsRequired();

                b.Property(x => x.ProductId)
                    .IsRequired();

                b.Property(x => x.Quantity)
                    .IsRequired();

                b.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2)
                    .IsRequired();

                b.HasIndex(x => new { x.OrderId, x.ProductId }).IsUnique();
            }
        }
    }
}
