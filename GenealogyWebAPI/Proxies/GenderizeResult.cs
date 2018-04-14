using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenealogyWebAPI.Model
{
    public class GenderizeResult
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "probability")]
        public double Probability { get; set; }
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}
