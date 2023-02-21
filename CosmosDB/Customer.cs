using Newtonsoft.Json;

namespace CosmosDB
{
    public class Customer
    {
        [JsonProperty("id")]
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string customerCity { get; set; }

        public List<Order> orders { get; set; }
    }
}
