using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data;

public partial class TechStoreDbContext : IdentityDbContext<ApplicationUser>
{
    public TechStoreDbContext(DbContextOptions<TechStoreDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Status> Statuses { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Favorited> Favorited { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorited>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ProductId });
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.ProductId });
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(a => new { a.AddressId, a.UserId });
        });

        modelBuilder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("MyUsers");
        });



        base.OnModelCreating(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
