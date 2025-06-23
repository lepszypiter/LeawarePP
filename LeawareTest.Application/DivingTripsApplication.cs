
using Microsoft.Extensions.DependencyInjection;

namespace LeawareTest.Application;

public static class LeawareTestApplication
{
    public static void RegisterLeawareTestApplication(this IServiceCollection services)
    {
        var tt = typeof(LeawareTestApplication);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(tt.Assembly));
    }
}
