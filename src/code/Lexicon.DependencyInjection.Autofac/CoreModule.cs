namespace Lexicon.DependencyInjection.Autofac
{
    using global::Autofac;
    using Lexicon.Data;
    using Microsoft.Extensions.Configuration;

    //https://github.com/autofac/Examples/blob/master/src/AspNetCoreExample/AutofacModule.cs
    public class CoreModule : Module
    {
        public CoreModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CsvHelperFormatter>()
                .As<ICsvFormatter>()
                .AsSelf();

            new SQLiteDependency(Configuration).Register(builder);

            //todo https://docs.autofac.org/en/latest/faq/select-by-context.html
            //todo do it better
            builder.Register(context =>
                new SourceProvider(new[] 
                    { ("sqliteDb", context.Resolve<IWordProvider>()) }
                ))
                .SingleInstance();

            builder.RegisterType<WordMultiSourceProvider>()
                .SingleInstance();
        }
    }
}
