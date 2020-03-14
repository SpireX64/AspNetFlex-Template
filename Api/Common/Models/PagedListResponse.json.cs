using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNetFlex.Api.Common.Models
{
    [JsonObject]
    public class PagedListResponse<T>
    {
        [JsonProperty("page", Required = Required.Always)] 
        public int Page { get; set; }
        
        [JsonProperty("total_count", Required = Required.Always)]
        public int TotalCount { get; set; }
        
        [JsonProperty("items_per_page", Required = Required.Always)]
        public int ItemsPerPage { get; set; }
        
        [JsonProperty("items")] 
        public IList<T> Items { get; set; } = new List<T>();
    }
}