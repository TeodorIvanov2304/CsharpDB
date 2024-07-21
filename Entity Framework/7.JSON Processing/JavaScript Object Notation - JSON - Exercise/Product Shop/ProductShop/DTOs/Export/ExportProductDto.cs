using Newtonsoft.Json;

namespace ProductShop.DTOs.Export
{
    public class ExportProductDto
    {
        //[JsonProperty("name")]
        public string Name { get; set; }
       // [JsonProperty("price")]
        public decimal Price { get; set; }
        //[JsonProperty("seller")]
        public string Seller { get; set; }
    }
}
