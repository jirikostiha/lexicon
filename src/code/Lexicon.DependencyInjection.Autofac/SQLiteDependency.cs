namespace Lexicon.DependencyInjection.Autofac
{
    using System;
    using System.Linq;
    using global::Autofac;
    using Lexicon;
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
            var sections = Configuration.AsEnumerable()
                .Where(x => x.Key.StartsWith(SQLiteOptions.BaseName, StringComparison.InvariantCulture))
                .GroupBy(x => x.Key.Contains(':') ? x.Key.Substring(0, x.Key.IndexOf(':')) : x.Key)
                .ToArray();
            
            foreach (var sectionName in sections.Select(s => s.Key))
            {
                var section = Configuration.GetSection(sectionName);
                if (!section.Exists())
                    continue;

                //options
                builder.RegisterInstance(Options.Create(section.Get<SQLiteOptions>()))
                    .As<IOptions<SQLiteOptions>>()
                    .SingleInstance();

                //repository
                builder.Register(context =>
                context.ComponentRegistry.Registrations
                    new SQLiteWordRepository(context.Resolve<IOptions<SQLiteOptions>>().Value))
                    .As<IWordProvider>()
                    .As<IWordRepository>()
                    .AsSelf();
            }
        }
    }
}
