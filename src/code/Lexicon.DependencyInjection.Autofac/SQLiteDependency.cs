namespace Lexicon.DependencyInjection.Autofac
{
    using System;
    using System.Linq;
    using global::Autofac;
    using Lexicon;
    using Lexicon.Data;
    using Lexicon.SQLite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class SQLiteDependency : DependencyBase
    {
        public SQLiteDependency(IConfiguration configuration) 
            : base(configuration)
        { }

        public override void Register(ContainerBuilder builder)
        {
            var sectionKeys = Configuration.GetSectionKeys()
                .Where(s => s.StartsWith(SQLiteOptions.BaseName, StringComparison.InvariantCulture))
                .ToArray();

            foreach (var sectionKey in sectionKeys)
            {
                var section = Configuration.GetSection(sectionKey);
                if (!section.Exists())
                    continue;

                //options
                builder.RegisterInstance(Options.Create(section.Get<SQLiteOptions>()))
                    .As<IOptions<SQLiteOptions>>()
                    .Keyed<IOptions<SQLiteOptions>>(sectionKey)
                    .SingleInstance();

                //repository
                builder.Register(context =>
                    new SQLiteWordRepository(context.ResolveKeyed<IOptions<SQLiteOptions>>(sectionKey).Value))
                    .As<IWordProvider>()
                    .As<IWordRepository>()
                    .AsSelf();

                //repository as tuple
                builder.Register(context =>
                    new Tuple<string, IWordProvider>(
                        sectionKey,
                        new SQLiteWordRepository(context.ResolveKeyed<IOptions<SQLiteOptions>>(sectionKey).Value)))
                    .AsSelf();
            }
        }
    }
}
