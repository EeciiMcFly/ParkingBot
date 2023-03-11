﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YogaBot.Storage;

#nullable disable

namespace YogaBot.Migrations
{
    [DbContext(typeof(StorageDbContext))]
    partial class StorageDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("YogaBot.Storage.Arrangements.Arrangement", b =>
                {
                    b.Property<long>("ArrangementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ArrangementId"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ArrangementId");

                    b.ToTable("Arrangements");
                });

            modelBuilder.Entity("YogaBot.Storage.Events.Event", b =>
                {
                    b.Property<long>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("EventId"));

                    b.Property<long>("ArrangementId")
                        .HasColumnType("bigint");

                    b.Property<int>("Cost")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EventId");

                    b.HasIndex("ArrangementId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("YogaBot.Storage.Presences.Presence", b =>
                {
                    b.Property<long>("PresenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("PresenceId"));

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("PresenceId");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Presences");
                });

            modelBuilder.Entity("YogaBot.Storage.UserArrangementRelations.UserArrangementRelation", b =>
                {
                    b.Property<long>("UserArrangementRelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserArrangementRelationId"));

                    b.Property<long>("ArrangementId")
                        .HasColumnType("bigint");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("UserArrangementRelationId");

                    b.HasIndex("ArrangementId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEventRelations");
                });

            modelBuilder.Entity("YogaBot.Storage.Users.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserId"));

                    b.Property<long>("TelegramUserId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.HasIndex("TelegramUserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("YogaBot.Storage.Events.Event", b =>
                {
                    b.HasOne("YogaBot.Storage.Arrangements.Arrangement", "Arrangement")
                        .WithMany()
                        .HasForeignKey("ArrangementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Arrangement");
                });

            modelBuilder.Entity("YogaBot.Storage.Presences.Presence", b =>
                {
                    b.HasOne("YogaBot.Storage.Events.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YogaBot.Storage.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("YogaBot.Storage.UserArrangementRelations.UserArrangementRelation", b =>
                {
                    b.HasOne("YogaBot.Storage.Arrangements.Arrangement", "Arrangement")
                        .WithMany()
                        .HasForeignKey("ArrangementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YogaBot.Storage.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Arrangement");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
