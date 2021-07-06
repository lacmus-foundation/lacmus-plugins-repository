using System.Collections.Generic;
using LacmusPlugin;
using Newtonsoft.Json;

namespace LacmusPluginsRepository.Models
{
    [JsonObject]
    public class PluginsResponse
    {
        [JsonProperty("maxPage")]
        public int MaxPage { get; set; }
        [JsonProperty("plugins")]
        public IEnumerable<IObjectDetectionPlugin> Plugins { get; set; }
    }
}