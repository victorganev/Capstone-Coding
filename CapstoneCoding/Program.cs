using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Fetch the HTML content
        using var httpClient = new HttpClient();
        var htmlContent = await httpClient.GetStringAsync("https://www.zacks.com/stock/quote/NVDA?q=NVDA"); // Need to add variable in stock tickers place

        // Debugging - print raw htmlContent 
        // Console.WriteLine(htmlContent);

        // Use regex to extract the chunk of HTML containing the "Zacks Rank" text
        string pattern = @"<p class=""rank_view"">.*?</p>";
        Match match = Regex.Match(htmlContent, pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            string extractedChunk = match.Value;

            // Console.WriteLine("Extracted Chunk:" + extractedChunk);

            // Further process the extracted chunk to extract the "Zacks Rank" text
            string rankPattern = @"\b\d-(Strong Buy|Buy|Hold|Sell|Strong Sell)\b";
            Match rankMatch = Regex.Match(extractedChunk, rankPattern);

            //For testing sakes
            if (rankMatch.Success)
            {
                Console.WriteLine("Extracted Value: " + rankMatch.Value);
            }
            else
            {
                Console.WriteLine("Rank value not found in the extracted chunk.");
            }
        }
        else
        {
            Console.WriteLine("No matching chunk found.");
        }
    }
}