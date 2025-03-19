﻿
using Microsoft.EntityFrameworkCore;

namespace TodoApi
{
    public partial class ToDoDbContext : DbContext
    {
        public ToDoDbContext()
        {
        }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<item>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("items");
                entity.Property(e => e.Name).HasMaxLength(100);
                
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public DbSet<item> items { get; set; } // הוספת DbSet עבור item

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    
}

// using System;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;

// namespace TodoApi;

// public partial class ToDoDbContext : DbContext
// {
//     public ToDoDbContext()
//     {
//     }

//     public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
//         : base(options)
//     {
//     }

//     public virtual DbSet<item> item { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder
//             .UseCollation("utf8mb4_0900_ai_ci")
//             .HasCharSet("utf8mb4");

//         modelBuilder.Entity<item>(entity =>
//         {
//             entity.HasKey(e => e.Id).HasName("PRIMARY");

//             entity.ToTable("item");

//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Name)
//                 .HasMaxLength(100)
//                 .HasColumnName("name");
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }