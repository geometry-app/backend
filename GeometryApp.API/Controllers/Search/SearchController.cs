using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GeometryApp.API.Controllers.Search.Filters;
using GeometryApp.API.Extensions;
using GeometryApp.API.Services;
using GeometryApp.Common;
using GeometryApp.Common.Filters;
using GeometryApp.Common.Models.Elastic.Levels;
using GeometryApp.Common.Models.Front;
using GeometryApp.Repositories;
using GeometryDashAPI.Server.Enums;
using Microsoft.AspNetCore.Mvc;
using Nest;
using HighlightString = GeometryApp.Common.HighlightString;

namespace GeometryApp.API.Controllers.Search
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : ControllerBase
    {
        private readonly IIndexRepository indexRepository;
        private readonly FiltersService filters;
        private static readonly string IndexServiceName = "search";

        public SearchController(IIndexRepository indexRepository, FiltersService filters)
        {
            this.indexRepository = indexRepository;
            this.filters = filters;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResultDto>> Search(
            [FromQuery(Name = "query")] string? query,
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "lucky")] int isLucky)
        {
            if (isLucky == 1)
                return await CreateLuckySearch();
            var prepared = SearchHelper.ParseAndPrepare(query, filters);
            if (prepared != null)
            {
                var result = await indexRepository.AdvanceSearch(prepared, page, 50);
                return CreateResponse(result);
            }
            query = SearchHelper.RemoveIllegalCharacter(query?.ToLower());
            if (string.IsNullOrWhiteSpace(query))
                return CreateEmptyResponse();
            var response = await indexRepository.SimpleSearch(query, page, 50);
            return CreateResponse(response);
        }

        [HttpGet]
        [Route("filters")]
        public ActionResult<FindFiltersResponse> FindFilters()
        {
            return new FindFiltersResponse(filters.Definitions);
        }

        private static SearchResultDto CreateResponse(ISearchResponse<LevelIndexFull> response)
        {
            return new SearchResultDto()
            {
                Items = response.Hits.Select(item => new LevelPreviewDto()
                {
                    Id = item.Source.MetaPreview!.Id,
                    Description = item.GetHighlighted("description", document => document.MetaPreview?.Description),
                    Name = item.GetHighlighted("name", document => document.MetaPreview?.Name),
                    DemonDifficulty = item.Source?.MetaPreview?.DemonDifficulty ?? 0,
                    Password = CryptExtensions.GetPasswordIfSetFromBase64(item.Source?.MetaFull?.Password),
                    Difficulty = item.Source?.MetaPreview?.Difficulty ?? 0,
                    DifficultyIcon = item.Source?.MetaPreview?.DifficultyIcon ?? 0,
                    IsDemon = item.Source?.MetaPreview?.IsDemon ?? false,
                    Badges = item.Source?.Badges?
                        .Select(x => BadgeFormatter.Format(x.Key, x.Value as Dictionary<string, object>))
                        .ToArray(),
                    Server = item.Source?.Server,
                    Type = PreviewType.Level,
                    Likes = item.Source?.MetaPreview?.Likes,
                    Length = (LengthType)(item.Source?.MetaPreview?.Length ?? 0),
                    Downloads = item.Source?.MetaPreview?.Download
                }),
                Total = response.Total,
                TimeSpend = response.Took
            };
        }

        private async Task<ActionResult<SearchResultDto>> CreateLuckySearch()
        {
            var response = await indexRepository.LuckySearch();
            return CreateResponse(response);
        }

        private ActionResult<SearchResultDto> CreateEmptyResponse()
        {
            return new SearchResultDto()
            {
                Total = 1,
                TimeSpend = 0,
                Items = new[]
                {
                    new LevelPreviewDto()
                    {
                        Id = 101010101,
                        Name = new HighlightString()
                        {
                            Items =
                            [
                                new() { Value = "Hello!" }
                            ]
                        },
                        Description = new HighlightString()
                        {
                            Items =
                            [
                                new() { Value = "I want to let you know that... " },
                                new() { Value = "You entered an empty query!", Highlighted = true },
                                new() { Value = " Please use only the Latin alphabet and feel free to include quotes around your search query if you'd like. For example, try searching for "},
                                new() { Value = "\"Nice level\"", Highlighted = true },
                                new() { Value = " Try again with a valid query to receive your results! Good luck!" }
                            ]
                        }
                    }
                }
            };
        }
    }
}
