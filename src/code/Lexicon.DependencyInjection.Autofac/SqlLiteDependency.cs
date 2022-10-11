namespace Lexicon.DependencyInjection.Autofac
{
    using global::Autofac;
    using Lexicon;
    using Lexicon.SqlLite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class SqlLiteDependency : DependencyBase
    {
        public SqlLiteDependency(IConfiguration configuration) 
            : base(configuration)
        { }

        public override void Register(ContainerBuilder builder)
        {
            var sectionName = SQLiteOptions.Name;
            var section = Configuration.GetSection(sectionName);
            if (!section.Exists())
                return;
            
            //options
            builder.RegisterInstance(Options.Create(section.Get<SQLiteOptions>()))
                .As<IOptions<SQLiteOptions>>()
                .SingleInstance();

            //repository
            builder.Register(context => 
                new SQLiteWordRepository(context.Resolve<IOptions<SQLiteOptions>>().Value))
                .As<IWordProvider>()
                .As<IWordRepository>()
                .AsSelf();
        }
    }
}
