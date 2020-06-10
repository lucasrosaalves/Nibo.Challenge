using Microsoft.Extensions.DependencyInjection;
using Nibo.Domain.Repositories;
using Nibo.Infra.Repositories;

namespace Nibo.Infra.Extensions
{
    public static class RepositoryExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
