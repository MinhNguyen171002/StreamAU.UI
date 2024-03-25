using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UserUI.Data.Model
{
    public class YoutubeVideo
    {
        [JsonProperty("videoid")] public string? VideoId { get; internal set; }
        [JsonProperty("title")] public string? Title { get; internal set; }
        [JsonProperty("thumbnail")] public string? Thumbnail { get; internal set; }
        [JsonProperty("PlayListId")] public Guid PlayListId { get; set; }
        public string UserName { get; set; }
        public class RootVideo
        {
            [JsonProperty("youtubeVideos")]
            public List<YoutubeVideo> details { get; set; }
        }
    }
}
