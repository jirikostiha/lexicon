namespace Lexicon.DependencyInjection.Autofac
{
    using global::Autofac;
    using Lexicon.Data;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class CoreModule : Module
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

            builder.Register(context =>
            {
                return new SourceProvider(
                    context.Resolve<IEnumerable<Tuple<string, IWordProvider>>>()
                    .Select(x => (x.Item1, x.Item2))
                    .ToArray());
            })
                .SingleInstance();

            builder.RegisterType<WordMultiSourceProvider>()
                .SingleInstance();
        }
    }
}
