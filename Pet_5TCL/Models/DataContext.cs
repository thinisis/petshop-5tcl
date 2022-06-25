using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Pet_5TCL.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<orders_item> orders_item { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<products_type> products_type { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<account>()
                .HasMany(e => e.orders)
                .WithRequired(e => e.account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<order>()
                .Property(e => e.tongtien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<order>()
                .HasMany(e => e.orders_item)
                .WithRequired(e => e.order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<orders_item>()
                .Property(e => e.dongia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<product>()
                .Property(e => e.dongia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<product>()
                .HasMany(e => e.orders_item)
                .WithRequired(e => e.product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<products_type>()
                .HasMany(e => e.products)
                .WithRequired(e => e.products_type)
                .WillCascadeOnDelete(false);
        }
    }
}
