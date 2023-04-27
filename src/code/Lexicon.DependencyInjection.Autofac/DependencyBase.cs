namespace Lexicon.DependencyInjection.Autofac
{
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