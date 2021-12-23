﻿// <auto-generated />
using System;
using CardService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CardService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211208152044_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CardService.Domain.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CVC")
                        .HasColumnType("text");

                    b.Property<string>("CardName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("Pan")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Cards");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"),
                            CVC = "123",
                            CardName = "First card",
                            IsDefault = true,
                            Pan = "4397185796979658",
                            UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814")
                        },
                        new
                        {
                            Id = new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"),
                            CVC = "666",
                            CardName = "Second card",
                            IsDefault = false,
                            Pan = "2367000000019234",
                            UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814")
                        });
                });

            modelBuilder.Entity("CardService.Domain.CardDateExpired", b =>
                {
                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("CardId");

                    b.ToTable("CardDateExpired");

                    b.HasData(
                        new
                        {
                            CardId = new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"),
                            Month = 12,
                            Year = 2023
                        },
                        new
                        {
                            CardId = new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"),
                            Month = 5,
                            Year = 2030
                        });
                });

            modelBuilder.Entity("CardService.Domain.TransactionHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<string>("TransactionName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.ToTable("TransactionHistory");
                });

            modelBuilder.Entity("CardService.Domain.CardDateExpired", b =>
                {
                    b.HasOne("CardService.Domain.Card", "Card")
                        .WithOne("CardDateExpired")
                        .HasForeignKey("CardService.Domain.CardDateExpired", "CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("CardService.Domain.TransactionHistory", b =>
                {
                    b.HasOne("CardService.Domain.Card", "Card")
                        .WithMany("TransList")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("CardService.Domain.Card", b =>
                {
                    b.Navigation("CardDateExpired");

                    b.Navigation("TransList");
                });
#pragma warning restore 612, 618
        }
    }
}
