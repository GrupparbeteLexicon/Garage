using Garage.Data;

namespace Garage.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> SeedDefaultData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GarageContext>();
        await SeedData.Initialize(context, scope.ServiceProvider);
        return app;
    }
}
