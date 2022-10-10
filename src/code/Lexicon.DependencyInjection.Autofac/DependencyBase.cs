namespace Lexicon.DependencyInjection.Autofac
{
    using global::Autofac;
    using Microsoft.Extensions.Configuration;

    public abstract class DependencyBase
    {
        protected DependencyBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; set; }

        public abstract void Register(ContainerBuilder builder);
    }
}
