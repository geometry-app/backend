using GeometryApp.Services.Roulette.Repositories.Entities;

namespace GeometryApp.Services.Roulette.Repositories;

public interface IRouletteOwnerRepository
{
    Task Put(string sessionId, bool owner, Guid rouletteId);
    Task<IEnumerable<RouletteOwnerEntity>> GetAll(string sessionId);
}
