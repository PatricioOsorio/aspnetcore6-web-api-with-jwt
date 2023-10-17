using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiNet6.Models;

public partial class DbApiContext : DbContext
{
  public DbApiContext()
  {
  }

  public DbApiContext(DbContextOptions<DbApiContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Categoria> Categoria { get; set; }

  public virtual DbSet<Producto> Producto { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  { }
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Categoria>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07FC9E921C");

      entity.Property(e => e.Descripcion)
              .HasMaxLength(50)
              .IsUnicode(false);
    });

    modelBuilder.Entity<Producto>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07B93DA9A9");

      entity.Property(e => e.CodigoBarra)
              .HasMaxLength(20)
              .IsUnicode(false);
      entity.Property(e => e.Descripcion)
              .HasMaxLength(50)
              .IsUnicode(false);
      entity.Property(e => e.Marca)
              .HasMaxLength(50)
              .IsUnicode(false);
      entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

      entity.HasOne(d => d.Categoria).WithMany(p => p.Producto)
              .HasForeignKey(d => d.IdCategoria)
              .HasConstraintName("FK_IDCATEGORIA");
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
