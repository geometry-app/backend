using GeometryApp.Common.Filters;

namespace GeometryApp.Services.Roulette.Generators;

public interface IRouletteGenerator
{
    Task<Roulette> CreateRoulette(Guid id, DemonWeights weights, PreparedRequest? request, params (string key, string value)[] properties);
}
