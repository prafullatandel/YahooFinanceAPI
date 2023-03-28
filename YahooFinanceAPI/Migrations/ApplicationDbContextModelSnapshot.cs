﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YahooFinanceAPI.Data;

#nullable disable

namespace YahooFinanceAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("YahooFinanceAPI.Models.Stock", b =>
                {
                    b.Property<int>("StockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StockId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StockId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("YahooFinanceAPI.Models.StockHistoricalData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AdjustedClose")
                        .HasColumnType("decimal(20,8)");

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(20,8)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(20,8)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(20,8)");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(20,8)");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("StockHistoricalDatas");
                });

            modelBuilder.Entity("YahooFinanceAPI.Models.StockHistoricalData", b =>
                {
                    b.HasOne("YahooFinanceAPI.Models.Stock", "Stock")
                        .WithMany("HistoricalData")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("YahooFinanceAPI.Models.Stock", b =>
                {
                    b.Navigation("HistoricalData");
                });
#pragma warning restore 612, 618
        }
    }
}
