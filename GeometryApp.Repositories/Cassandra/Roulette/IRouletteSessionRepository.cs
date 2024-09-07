using System;
using System.Threading.Tasks;

namespace GeometryApp.Repositories.Cassandra.Roulette;

public interface IRouletteSessionRepository
{
    Task<string> CreateSession(string session, Guid roulette);
}
