using Newtonsoft.Json;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class ParsedJson
    {
        [JsonProperty("buyer")]
        public string Buyer { get; set; }
        
        [JsonProperty("id")]
        public string Id {get; set;}
        
        [JsonProperty("name_nft")]
        public string NameNft {get; set;}

    }
}
