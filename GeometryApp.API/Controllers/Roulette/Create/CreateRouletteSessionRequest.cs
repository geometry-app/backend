using System;
using GeometryApp.Services.Roulette;

namespace GeometryApp.API.Controllers.Roulette.Create;

public class CreateRouletteSessionRequest
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string Server { get; set; }
    public DemonWeights Weights { get; set; }

    public Guid? RouletteId { get; set; }
}
