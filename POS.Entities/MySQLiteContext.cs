using Microsoft.EntityFrameworkCore;
using POS.Entities;
using System;

namespace POS.Entities
{
    public class MySQLiteContext : DbContext
    {
        public MySQLiteContext(DbContextOptions<MySQLiteContext> options)
            : base(options)
        {
        }



        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryLog> CategoryLog { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductLog> ProductLog { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesDetail> SalesDetail { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<WarehouseLocation> WarehouseLocation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User Account

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email);
                entity.Property(e => e.PhoneNumber);
            });

            #endregion


            modelBuilder.Entity<Category>(entity =>
                    {
                        entity.HasKey(e => e.IdCategory);
                        entity.Property(e => e.Name).IsRequired();
                        entity.Property(e => e.Status).IsRequired();
                        entity.Property(e => e.CreateUser).IsRequired();
                        entity.Property(e => e.CreateDate).IsRequired();
                        entity.Property(e => e.LastUpdateUser);
                        entity.Property(e => e.LastUpdateDate);
                    });

            modelBuilder.Entity<CategoryLog>(entity =>
            {
                entity.HasKey(e => new { e.IdMovement, e.IdCategory, e.Name });
                entity.Property(e => e.IdMovement).IsRequired();
                entity.Property(e => e.IdCategory).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.LastUpdateUser);
                entity.Property(e => e.LastUpdateDate);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => new { e.IdMovement, e.IdStock });
                entity.Property(e => e.IdMovement).IsRequired();
                entity.Property(e => e.IdStock).IsRequired();
                entity.Property(e => e.MovementType).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.MovementUser).IsRequired();
                entity.Property(e => e.MovementDate).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IdCategory).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.MeasureUnit).IsRequired();
                entity.Property(e => e.UrlImage).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreateUser).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.LastUpdateUser);
                entity.Property(e => e.LastUpdateDate);
            });

            modelBuilder.Entity<ProductLog>(entity =>
            {
                entity.HasKey(e => new { e.IdMovement, e.IdProduct, e.Name, e.IdCategory });
                entity.Property(e => e.IdMovement).IsRequired();
                entity.Property(e => e.IdProduct).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IdCategory).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.MeasureUnit).IsRequired();
                entity.Property(e => e.UrlImage).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.LastUpdateUser);
                entity.Property(e => e.LastUpdateDate);
            });

            modelBuilder.Entity<Sales>(entity =>
            {
                entity.HasKey(e => e.IdSales);
                entity.Property(e => e.SalesDate).IsRequired();
                entity.Property(e => e.Total).IsRequired();
                entity.Property(e => e.CreateUser).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.LastUpdateUser);
                entity.Property(e => e.LastUpdateDate);
            });

            modelBuilder.Entity<SalesDetail>(entity =>
            {
                entity.HasKey(e => e.IdSalesDetail);
                entity.Property(e => e.IdSales).IsRequired();
                entity.Property(e => e.IdProduct).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Subtotal).IsRequired();
                entity.Property(e => e.CreateUser).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.LastUpdateUser);
                entity.Property(e => e.LastUpdateDate);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.IdStock);
                entity.Property(e => e.IdWarehouse).IsRequired();
                entity.Property(e => e.IdProduct).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.CreateUser).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.LastUpdateUser);
                entity.Property(e => e.LastUpdateDate);
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.IdWarehouse);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IdWL).IsRequired();
                entity.Property(e => e.CreateUser).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.LastUpdateUser);
                entity.Property(e => e.LastUpdateDate);
            });

            modelBuilder.Entity<WarehouseLocation>(entity =>
            {
                entity.HasKey(e => e.IdWL);
                entity.Property(e => e.Address).IsRequired();
            });
        }
    }
}
