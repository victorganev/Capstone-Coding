//As certain data patterns emerge, create and define custom models in here.

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
}
