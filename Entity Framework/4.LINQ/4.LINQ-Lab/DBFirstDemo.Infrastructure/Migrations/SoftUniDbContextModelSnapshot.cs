﻿// <auto-generated />
using System;
using DBFirstDemoo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DBFirstDemoo.Infrastructure.Migrations
{
    [DbContext(typeof(SoftUniDbContext))]
    partial class SoftUniDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AddressID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"), 1L, 1);

                    b.Property<string>("AddressText")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("TownId")
                        .HasColumnType("int")
                        .HasColumnName("TownID");

                    b.HasKey("AddressId");

                    b.HasIndex("TownId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"), 1L, 1);

                    b.Property<int>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("ManagerID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("DepartmentId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EmployeeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"), 1L, 1);

                    b.Property<int?>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("AddressID");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("ManagerID");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money");

                    b.HasKey("EmployeeId");

                    b.HasIndex("AddressId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProjectID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("smalldatetime");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Town", b =>
                {
                    b.Property<int>("TownId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TownID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TownId"), 1L, 1);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("TownId");

                    b.HasIndex("CountryId");

                    b.ToTable("Towns");
                });

            modelBuilder.Entity("EmployeeProject", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId", "ProjectId");

                    b.ToTable("EmployeeProject");
                });

            modelBuilder.Entity("EmployeesProject", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("EmployeeID");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectID");

                    b.HasKey("EmployeeId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("EmployeesProjects", (string)null);
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Address", b =>
                {
                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Town", "Town")
                        .WithMany("Addresses")
                        .HasForeignKey("TownId")
                        .HasConstraintName("FK_Addresses_Towns");

                    b.Navigation("Town");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Department", b =>
                {
                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Employee", "Manager")
                        .WithMany("Departments")
                        .HasForeignKey("ManagerId")
                        .IsRequired()
                        .HasConstraintName("FK_Departments_Employees");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Employee", b =>
                {
                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Address", "Address")
                        .WithMany("Employees")
                        .HasForeignKey("AddressId")
                        .HasConstraintName("FK_Employees_Addresses");

                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .IsRequired()
                        .HasConstraintName("FK_Employees_Departments");

                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Employee", "Manager")
                        .WithMany("InverseManager")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK_Employees_Employees");

                    b.Navigation("Address");

                    b.Navigation("Department");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Town", b =>
                {
                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("EmployeesProject", b =>
                {
                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_EmployeesProjects_Employees");

                    b.HasOne("DBFirstDemoo.Infrastructure.Data.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .IsRequired()
                        .HasConstraintName("FK_EmployeesProjects_Projects");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Address", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Employee", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("InverseManager");
                });

            modelBuilder.Entity("DBFirstDemoo.Infrastructure.Data.Models.Town", b =>
                {
                    b.Navigation("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}
