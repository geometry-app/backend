using System.Threading.Tasks;
using GeometryApp.Common.Filters;
using GeometryApp.Common.Models.Elastic.Levels;
using Nest;

namespace GeometryApp.Repositories
{
    public interface IIndexRepository
    {
        Task<ISearchResponse<LevelIndexFull>> SimpleSearch(string query, int skip = 0, int take = 10);
        Task<ISearchResponse<LevelIndexFull>> AdvanceSearch(string text, InternalFilter[] filters, int skip = 0, int take = 10);
        Task<ISearchResponse<LevelIndexFull>> LuckySearch();
    }
}
