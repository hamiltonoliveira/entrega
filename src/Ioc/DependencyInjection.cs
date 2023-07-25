using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ioc
{
        public static class DependencyInjection
        {
            public static IServiceCollection AddInfraStructure(this IServiceCollection service, IConfiguration Configuration)
            {
                //service.AddScoped<IAutenticarRepositorio, AutenticarRepositorio>();
                //service.AddScoped<IUsersRepository, UsersRepository>();
               return service;
            }

            public static IServiceCollection AddServices(this IServiceCollection service, IConfiguration Configuration)
            {
                //service.AddScoped<IAutenticarService, AutenticarService>();
                //service.AddScoped<IUsersService, UsersService>();
               return service;
            }
        }
 }
