namespace Lexicon.DependencyInjection.Autofac
{
    using global::Autofac;
    using Lexicon.Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

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
            new SqlLiteDependency(Configuration).Register(builder);

            //shttps://docs.autofac.org/en/latest/faq/select-by-context.html
            builder.RegisterType<SourceProvider>()
                .SingleInstance();

            builder.RegisterType<WordMultiSourceProvider>()
                .SingleInstance();

            builder.RegisterType<CsvHelperFormatter>()
                .AsSelf()
                .As<ICsvFormatter>();
        }
    }
}
