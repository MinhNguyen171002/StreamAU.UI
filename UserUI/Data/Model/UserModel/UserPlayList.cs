using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace UserUI.Data.Model
{
    public class UserPlayList
    {
        [JsonProperty("PlayListId")] public Guid PlayListId { get; set; }
        [JsonProperty("PlayListName")] public string PlayListName { get; set; }
        [JsonProperty("UserId")] public string UserId { get; set; }
        [JsonProperty("videos")] public virtual ICollection<YoutubeVideo> videos { get; set; }
        public class RootUser
        {
            [JsonProperty("userPlayLists")]
            public List<UserPlayList> pl { get; set; }
        }
    }
}
