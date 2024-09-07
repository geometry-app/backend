namespace GeometryApp.Services.Roulette.Generators;

public interface IRouletteGenerator
{
    Task<Roulette> CreateRoulette(Guid id, DemonWeights weights, params (string key, string value)[] properties);
}
