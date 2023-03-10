using Autofac;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using YogaBot.Storage;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;
using YogaBot.Storage.Presences;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Modules;

public class StorageModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        Register<UsersRepository, IUsersRepository>(builder, (c) => new UsersRepository(c));
        Register<ArrangementRepository, IArrangementRepository>(builder, (c) => new ArrangementRepository(c));
        Register<EventsRepository, IEventsRepository>(builder, (c) => new EventsRepository(c));
        Register<UserArrangementRelationsRepository, IUserEventRelationsRepository>(builder, (c) => new UserArrangementRelationsRepository(c));
        Register<PresenceRepository, IPresenceRepository>(builder, (c) => new PresenceRepository(c));
    }

    private void Register<TImpl, TAbs>(ContainerBuilder builder, Func<StorageDbContext, TImpl> createImpl) 
        where TAbs : notnull 
        where TImpl : notnull
    {
        builder.Register(c =>
            {
                var dbContext = GetDbContext(c);
                return createImpl(dbContext);
            })
            .As<TAbs>()
            .InstancePerLifetimeScope();
    }

    private StorageDbContext GetDbContext(IComponentContext context)
    {
        var configuration = context.Resolve<IConfiguration>();
        //var connectionString = configuration.GetConnectionString("DatabaseConnectionTemplateWithoutDbName");
        var configurationString = new NpgsqlConnectionStringBuilder()
        {
            Host = "localhost",
            Port = 5432,
            Database = "yoga",
            Username = "postgres",
            Password = "masterkey"
        }.ToString();
        

        var optionsBuilder = new DbContextOptionsBuilder<StorageDbContext>();
        optionsBuilder.UseNpgsql(configurationString);

        return new StorageDbContext(optionsBuilder.Options);
    }
}