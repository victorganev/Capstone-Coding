using System.Text.Json.Serialization;

namespace Models
{
    //For the analyst action data
    public class AnalystAction
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("analyst")]
        public string Analyst { get; set; }

        [JsonPropertyName("ratingChange")]
        public string RatingChange { get; set; }

        [JsonPropertyName("priceTargetChange")]
        public string PriceTargetChange { get; set; }

        public override string ToString()
        {
            return $"Date: {Date}, Action: {Action}, Analyst: {Analyst}, Rating Change: {RatingChange}, Price Target Change: {PriceTargetChange}";
        }
    }

    public class InsiderTradingData
    {
        [JsonPropertyName("insiderTrading")]
        public string InsiderTrading { get; set; }
        [JsonPropertyName("relationship")]
        public string Relationship { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("transaction")]
        public string Transaction { get; set; }
        [JsonPropertyName("cost")]
        public string Cost { get; set; }
        [JsonPropertyName("shares")]
        public string Shares { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("sharesTotal")]
        public string SharesTotal { get; set; }
        [JsonPropertyName("secDate")]
        public string SecDate { get; set; }
        [JsonPropertyName("secLink")]
        public string SecLink { get; set; }

        public override string ToString()
        {
            return $"Insider Trading: {InsiderTrading}\n" +
                   $"Relationship: {Relationship}\n" +
                   $"Date: {Date}\n" +
                   $"Transaction: {Transaction}\n" +
                   $"Cost: {Cost}\n" +
                   $"Shares: {Shares}\n" +
                   $"Value: {Value}\n" +
                   $"Shares Total: {SharesTotal}\n" +
                   $"SEC Date: {SecDate}\n" +
                   $"SEC Link: {SecLink}\n" +
                   new string('-', 50);
        }
    }

    public class CandlestickData
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("open")]
        public double Open { get; set; }

        [JsonPropertyName("high")]
        public double High { get; set; }

        [JsonPropertyName("low")]
        public double Low { get; set; }

        [JsonPropertyName("close")]
        public double Close { get; set; }

        [JsonPropertyName("volume")]
        public double Volume { get; set; } // Optional if volume is included in the JSON

    }
}
