using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTuitionAppCore(this IServiceCollection services)
        {
            var applicationAssemblies = Assembly.GetExecutingAssembly();
            services.AddMediatR(applicationAssemblies);

            // not using FluentValidation.AspNetCore package due to issue - https://github.com/JasonGT/NorthwindTraders/issues/76
            // manually register fluent validation
            services.AddValidatorsFromAssembly(applicationAssemblies);

            return services;
        }
    }
}
