using System.Text.Json.Serialization;

namespace Models
{
    //For the analyst action data
    public class AnalystAction
    {
        public string Date { get; set; }
        public string Action { get; set; }
        public string Analyst { get; set; }
        public string RatingChange { get; set; }
        public string PriceTargetChange { get; set; }

        public override string ToString()
        {
            return $"Date: {Date}, Action: {Action}, Analyst: {Analyst}, Rating Change: {RatingChange}, Price Target Change: {PriceTargetChange}";
        }
    }

    public class InsiderTradingData
    {
        public string InsiderTrading { get; set; }
        public string Relationship { get; set; }
        public string Date { get; set; }
        public string Transaction { get; set; }
        public string Cost { get; set; }
        public string Shares { get; set; }
        public string Value { get; set; }
        public string SharesTotal { get; set; }
        public string SecDate { get; set; }
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
        public double Volume { get; set; } 

        public override string ToString()
        {
            return $"Date: {Date}, Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume}";
        }
    }

}
