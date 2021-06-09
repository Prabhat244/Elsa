using Elsa.Samples.UserRegistration.Web.Activities;
using Elsa.Samples.UserRegistration.Web.Activities.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Samples.UserRegistration.Web.Extensions
{
    public static class UserServiceCollectionExtensions
    {
        public static ElsaOptionsBuilder AddUserActivities(this ElsaOptionsBuilder options)
        {
            return options
                .AddActivity<CreateUser>()
                .AddActivity<ActivateUser>()
                .AddActivity<DeleteUser>();
        }
        public static IServiceCollection AddOptionsProviders(this IServiceCollection services)
        {
            return services
                  .AddActivityPropertyOptionsProvider<CountryOptionsProvider>()
                  .AddActivityPropertyOptionsProvider<CreateUser>()
                  .AddRuntimeSelectItemsProvider<CountryOptionsProvider>();

        }
    }
}