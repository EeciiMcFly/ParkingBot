﻿// <auto-generated />


#nullable disable

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MorionParkingBot.Users;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
namespace MorionParkingBot.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20220516043047_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MorionParkingBot.Database.LicenseInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("UserId");

                    b.ToTable("LicenseInfos");
                });

            modelBuilder.Entity("MorionParkingBot.Database.UserData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("TelegramUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TelegramUserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MorionParkingBot.Database.LicenseInfo", b =>
                {
                    b.HasOne("MorionParkingBot.Database.UserData", "User")
                        .WithMany("LicenseInfos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MorionParkingBot.Database.UserData", b =>
                {
                    b.Navigation("LicenseInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
