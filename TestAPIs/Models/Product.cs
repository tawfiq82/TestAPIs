using System.Text.Json.Serialization;

namespace TestAPIs.Models
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        //runtime property

        [JsonIgnore]
        public int PopularityScore { get; set; }
    }
}
