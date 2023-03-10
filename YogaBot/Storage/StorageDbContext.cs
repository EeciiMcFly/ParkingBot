﻿using Microsoft.EntityFrameworkCore;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;
using YogaBot.Storage.Presences;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Storage
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext()
        {
        }

        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<UserData> Users { get; set; }
        public DbSet<ArrangementData> Arrangements { get; set; }
        public DbSet<UserArrangementRelationData> UserEventRelations { get; set; }
        public DbSet<EventData> Events { get; set; }
        public DbSet<PresenceData> Presences { get; set; }

        // Метод переопределяется для создания миграций
        // При работе сервиса контекст определяется в autofac и IsConfigured будет true
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql("Host=localhost; Port=5432;Database=postgres;Username=yoga;Password=masterkey");
            }
        }
    }
}