using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nibo.Domain.Commands;
using Nibo.Domain.Handlers;
using Nibo.Domain.Repositories;
using Nibo.Infra.Repositories;

namespace Nibo.API.Configurations
{
    public static class ServicesConfiguration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<ImportExtractFilesCommand, ValidationResult>, ImportExtractFilesCommandHandler>();

            services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
