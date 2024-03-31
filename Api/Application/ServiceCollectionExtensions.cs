 namespace Api.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IProductServiceApp, ProductServiceApp>();
        }
    }
}
