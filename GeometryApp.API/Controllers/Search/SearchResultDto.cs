using System.Collections.Generic;
using System.Runtime.Serialization;
using GeometryApp.Common.Models.Front;

namespace GeometryApp.API.Controllers.Search
{
    [DataContract]
    public class SearchResultDto
    {
        [DataMember(Name = "timeSpend")] public long TimeSpend { get; set; }
        [DataMember(Name = "total")] public long Total { get; set; }
        [DataMember(Name = "items")] public IEnumerable<LevelPreviewDto> Items { get; set; }
    }
}