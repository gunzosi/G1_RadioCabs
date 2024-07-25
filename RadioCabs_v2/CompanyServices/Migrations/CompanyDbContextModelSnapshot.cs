﻿// <auto-generated />
using System;
using CompanyServices.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompanyServices.Migrations
{
    [DbContext(typeof(CompanyDbContext))]
    partial class CompanyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CompanyServices.Models.AdvertisementImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("AdvertisementImages");
                });

            modelBuilder.Entity("CompanyServices.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyDistrict")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyTaxCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyTelephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyWard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPerson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPersonMobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Designation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MembershipId")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CompanyServices.Models.CompanyLocationService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CityService")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyLocationServices");
                });

            modelBuilder.Entity("CompanyServices.Models.CompanyService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("ServiceType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyServices");
                });

            modelBuilder.Entity("CompanyServices.Models.AdvertisementImage", b =>
                {
                    b.HasOne("CompanyServices.Models.Company", "Company")
                        .WithMany("Advertisements")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyServices.Models.CompanyLocationService", b =>
                {
                    b.HasOne("CompanyServices.Models.Company", "Company")
                        .WithMany("CompanyLocationServices")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyServices.Models.CompanyService", b =>
                {
                    b.HasOne("CompanyServices.Models.Company", "Company")
                        .WithMany("CompanyServices")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyServices.Models.Company", b =>
                {
                    b.Navigation("Advertisements");

                    b.Navigation("CompanyLocationServices");

                    b.Navigation("CompanyServices");
                });
#pragma warning restore 612, 618
        }
    }
}
