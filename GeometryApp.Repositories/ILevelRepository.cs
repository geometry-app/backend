using System.Collections.Generic;
using System.Threading.Tasks;
using GeometryApp.Repositories.Entities;

namespace GeometryApp.Repositories
{
    public interface ILevelRepository
    {
        Task Add(LevelEntity level);
        Task<LevelEntity> GetLast(int id);
        Task<IEnumerable<LevelEntity>> Get(int id);
        Task<bool> Contains(int id);
        IAsyncEnumerable<int> SelectAllIds();
    }
}
