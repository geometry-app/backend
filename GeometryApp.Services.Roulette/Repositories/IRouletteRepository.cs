namespace GeometryApp.Services.Roulette.Repositories;

public interface IRouletteRepository
{
    Task<Guid> Save(Roulette roulette);
    Task<Roulette> Get(Guid rouletteId);
    Task SealSequence(Guid rouletteId, int sequenceId);
}
