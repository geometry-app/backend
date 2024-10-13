using GeometryApp.Services.Roulette;

namespace GeometryApp.API.Controllers.Roulette.Balance;

public class BalanceResponse
{
    public RouletteLevelWeights Weights { get; set; }
    public long Total { get; set; }
}
