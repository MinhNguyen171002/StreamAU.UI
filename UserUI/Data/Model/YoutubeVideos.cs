using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserUI.Data.Model
{
    public class YoutubeVideos
    {
        [JsonProperty("thumbnail")] public string Thumbnail { get;set; }
        [JsonProperty("title")] public string Title { get;set; }
        [JsonProperty("videoId")]  public string VideoId { get;set; }

    }
    public class Root
    {
        [JsonProperty("videos")]
        public List<YoutubeVideos> videos { get; set; }
    }
}
