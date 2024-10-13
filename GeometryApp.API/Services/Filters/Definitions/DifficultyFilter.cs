using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeometryApp.Common.Filters;
using GeometryApp.Common.Models.GeometryDash;

namespace GeometryApp.API.Services.Filters.Definitions;

public class DifficultyFilter : IFilter, IAutoComplete
{
    public string Name => "difficulty";

    public Dictionary<string, (bool demon, InternalDifficultyIcon[] icon)> values = new(StringComparer.OrdinalIgnoreCase)
    {
        ["demons"] = (true, [
            InternalDifficultyIcon.Easy,
            InternalDifficultyIcon.Normal,
            InternalDifficultyIcon.Hard,
            InternalDifficultyIcon.Harder,
            InternalDifficultyIcon.Insane
        ]),
        ["easy"] = (false, [InternalDifficultyIcon.Easy]),
        ["normal"] = (false, [InternalDifficultyIcon.Normal]),
        ["hard"] = (false, [InternalDifficultyIcon.Hard]),
        ["harder"] = (false, [InternalDifficultyIcon.Harder]),
        ["insane"] = (false, [InternalDifficultyIcon.Insane]),
        ["auto"] = (false, [InternalDifficultyIcon.Auto]),
        ["undefined"] = (false, [InternalDifficultyIcon.Undef]),
        ["easy_demon"] = (true, [InternalDifficultyIcon.Easy]),
        ["medium_demon"] = (true, [InternalDifficultyIcon.Normal]),
        ["hard_demon"] = (true, [InternalDifficultyIcon.Hard]),
        ["insane_demon"] = (true, [InternalDifficultyIcon.Harder]),
        ["extreme_demon"] = (true, [InternalDifficultyIcon.Insane]),
    };

    public FilterDefinition GetDefinition() => new(FilterType.Term, "difficulty");

    public IEnumerable<InternalFilter> Enrich(Filter item)
    {
        if (!values.TryGetValue(item.Value, out var difficulty))
            yield break;
        yield return new InternalFilter("metaPreview.isDemon", [difficulty.demon ? "true" : "false"], (InternalFilterOperator)item.Operator);
        yield return new InternalFilter("metaPreview.difficultyIcon", difficulty.icon.Select(x => ((int)x).ToString()).ToArray(), (InternalFilterOperator)item.Operator);
    }

    public Task<string[]> GetCompletionsAsync() => Task.FromResult(values.Keys.ToArray());
}
