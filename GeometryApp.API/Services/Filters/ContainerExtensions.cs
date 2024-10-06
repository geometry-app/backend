using GeometryApp.API.Services.Filters.Definitions;
using Microsoft.Extensions.DependencyInjection;

namespace GeometryApp.API.Services.Filters;

public static class ContainerExtensions
{
    public static IServiceCollection RegisterFilters(this IServiceCollection services)
    {
        services.AddSingleton<IFilter, LengthFilter>();
        services.AddSingleton<IFilter, LikesFilter>();
        services.AddSingleton<IFilter, DownloadsFilter>();
        services.AddSingleton<IFilter, ListFilter>();

        services.AddSingleton<FiltersService>();
        return services;
    }
}
