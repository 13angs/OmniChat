using Microsoft.AspNetCore.Mvc;

namespace OmniChat.Models
{
    public class DefaultRequest
    {
        [FromQuery(Name = "by")]
        public required RequestParam By { get; set; }
        
        [FromQuery(Name = "limit")]
        public int Limit { get; set; }
        
        [FromQuery(Name = "sort_by")]
        public string[]? SortBy { get; set; }

        [FromQuery(Name = "sort_order")]
        public string[]? SortOrder { get; set; }
    }
}