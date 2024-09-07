namespace GeometryApp.Services.Roulette.Repositories;

public interface IProgressRepository
{
    Task Set(Guid rouletteId, string sessionId, int sequenceNumber, int levelId, int progress);
    Task<IEnumerable<ProgressEntry>> Get(Guid rouletteId, string sessionId);
}
