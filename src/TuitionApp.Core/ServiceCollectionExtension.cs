using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTuitionAppCore(this IServiceCollection services)
        {
            //var assembly = typeof(ServiceCollectionExtension).Assembly;

            services.AddMediatR(Assembly.GetExecutingAssembly());


            return services;
        }
    }
}
