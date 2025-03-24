using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Models;
using Services;


class Program
{
    static async Task Main(string[] args)
    {
        var stockTicker = "NVDA";

        // Fetch HTML content
        var zacksHtmlContent = await HttpService.FetchHtmlContentAsync($"https://www.zacks.com/stock/quote/{stockTicker}?q={stockTicker}");
        var finvizHtmlContent = await HttpService.FetchHtmlContentAsync($"https://finviz.com/quote.ashx?t={stockTicker}&ty=c&ta=1&p=d");

        // Extract Zacks Rank
        string zacksRankPattern = @"<p class=""rank_view"">.*?</p>";
        Match zacksRankMatch = RegexParsing.ExtractHtmlChunk(zacksHtmlContent, zacksRankPattern);

        if (zacksRankMatch.Success)
        {
            string rankPattern = @"\b\d-(Strong Buy|Buy|Hold|Sell|Strong Sell)\b";
            Match rankMatch = RegexParsing.ExtractHtmlChunk(zacksRankMatch.Value, rankPattern);
            if (rankMatch.Success)
            {
                Console.WriteLine($"Zacks Rank: {rankMatch.Value}");
            }
        }

        // Extract stock price
        string pricePattern = @"<p class=""last_price"">.*?</p>";
        Match priceMatch = RegexParsing.ExtractHtmlChunk(zacksHtmlContent, pricePattern);

        if (priceMatch.Success)
        {
            string price = HAPParsing.ParseSingleNode(priceMatch.Value, "//p[@class='last_price']");
            Console.WriteLine($"Stock Price: {price}");
        }
        
        // Extract opening price data
        string openRegexPattern = @"<section id=""stock_activity"">.*?</section>";
        Match openMatch = RegexParsing.ExtractHtmlChunk(zacksHtmlContent, openRegexPattern);

        if (openMatch.Success)
        {
            string extractedChunk = openMatch.Value;

            // Use HAPParsing to parse the stock activity data
            var stockDataList = HAPParsing.ParseStockActivityData(extractedChunk);

            Console.WriteLine("General data about " + stockTicker + " from Stock Activity column on Zacks:");
            foreach (var data in stockDataList)
            {
                Console.WriteLine(data);
            }
        }
        else
        {
            Console.WriteLine("No matching chunk found for stock activity data.");
        }

        // Extract general stock data from Finviz
        string finvizGenDataPattern = @"<table[^>]*class=""[^""]*js-snapshot-table[^""]*"".*?>.*?</table>";
        Match finvizGenDataMatch = RegexParsing.ExtractHtmlChunk(finvizHtmlContent, finvizGenDataPattern);

        if (finvizGenDataMatch.Success)
        {
            Console.WriteLine("\n\nGeneral data about " + stockTicker + " from Finviz:");

            var generalData = HAPParsing.ParseKeyValuePairs(finvizGenDataMatch.Value, "//td[1]", "//td[2]");
            foreach (var data in generalData)
            {
                Console.WriteLine(data);
            }
        }

        // Extract analyst actions
        string analystActionsPattern = @"<table[^>]*class=""[^""]*js-table-ratings[^""]*styled-table-new[^""]*"".*?>.*?</table>";
        Match analystActionsMatch = RegexParsing.ExtractHtmlChunk(finvizHtmlContent, analystActionsPattern);

        if (analystActionsMatch.Success)
        {
            var analystActions = HAPParsing.ParseAnalystActions(analystActionsMatch.Value);
            foreach (var action in analystActions)
            {
                Console.WriteLine(action);
            }
        }

        // Extract insider trading data
        string insiderTradingPattern = @"<table[^>]*class=""[^""]*styled-table-new[^""]*""[^>]*>.*?<th[^>]*>Insider Trading</th>.*?</table>";
        Match insiderTradingMatch = RegexParsing.ExtractHtmlChunk(finvizHtmlContent, insiderTradingPattern);

        if (insiderTradingMatch.Success)
        {
            var insiderData = HAPParsing.ParseInsiderTradingTable(insiderTradingMatch.Value);
            foreach (var data in insiderData)
            {
                Console.WriteLine(data);
            }
        }
    }
}