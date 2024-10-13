using System.Threading.Tasks;

namespace GeometryApp.API.Services.Filters;

public interface IAutoComplete
{
    Task<string[]> GetCompletionsAsync();
}
