﻿// <auto-generated />
using System;
using BlogDemo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogDemo.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    [Migration("20240627103803_AuthorAdded")]
    partial class AuthorAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlogDemo.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"), 1L, 1);

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR(100)")
                        .HasColumnName("AuthorName");

                    b.Property<int?>("BlogId")
                        .HasColumnType("int");

                    b.HasKey("AuthorId");

                    b.HasIndex("BlogId")
                        .IsUnique()
                        .HasFilter("[BlogId] IS NOT NULL");

                    b.ToTable("Authors", "blg");
                });

            modelBuilder.Entity("BlogDemo.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogId"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("NVARCHAR(500)")
                        .HasColumnName("Blog Description");

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("BlogName");

                    b.HasKey("BlogId");

                    b.HasIndex(new[] { "Name" }, "ix_Blogs_Name_Unique")
                        .IsUnique();

                    b.ToTable("Blogs", "blg");
                });

            modelBuilder.Entity("BlogDemo.Author", b =>
                {
                    b.HasOne("BlogDemo.Blog", "Blog")
                        .WithOne("Author")
                        .HasForeignKey("BlogDemo.Author", "BlogId");

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("BlogDemo.Blog", b =>
                {
                    b.Navigation("Author")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
