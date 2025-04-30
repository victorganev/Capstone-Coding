using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Models;
using System.Text.RegularExpressions;

namespace Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockDataController : ControllerBase
    {
        // Endpoint for fetching data from Zacks
        [HttpGet("GetZacksData")]
        public async Task<IActionResult> GetZacksData(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker))
            {
                return BadRequest("Ticker is required.");
            }

            try
            {
                // Fetch HTML content from Zacks
                var zacksHtmlContent = await HttpService.FetchHtmlContentAsync($"https://www.zacks.com/stock/quote/{ticker}?q={ticker}");

                // Prepare the response object
                var response = new Dictionary<string, object>();

                // Extract Zacks Rank
                string zacksRankPattern = @"<p class=""rank_view"">.*?</p>";
                Match zacksRankMatch = RegexParsing.ExtractHtmlChunk(zacksHtmlContent, zacksRankPattern);

                if (zacksRankMatch.Success)
                {
                    string rankPattern = @"\b\d-(Strong Buy|Buy|Hold|Sell|Strong Sell)\b";
                    Match rankMatch = RegexParsing.ExtractHtmlChunk(zacksRankMatch.Value, rankPattern);
                    if (rankMatch.Success)
                    {
                        response["ZacksRank"] = rankMatch.Value;
                    }
                }

                // Extract stock price
                string pricePattern = @"<p class=""last_price"">.*?</p>";
                Match priceMatch = RegexParsing.ExtractHtmlChunk(zacksHtmlContent, pricePattern);

                if (priceMatch.Success)
                {
                    string price = HAPParsing.ParseSingleNode(priceMatch.Value, "//p[@class='last_price']");
                    response["StockPrice"] = price;
                }

                // Extract stock activity data
                string openRegexPattern = @"<section id=""stock_activity"">.*?</section>";
                Match openMatch = RegexParsing.ExtractHtmlChunk(zacksHtmlContent, openRegexPattern);

                if (openMatch.Success)
                {
                    string extractedChunk = openMatch.Value;
                    var stockDataList = HAPParsing.ParseStockActivityData(extractedChunk);
                    response["StockActivityData"] = stockDataList;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint for fetching data from Finviz
        [HttpGet("GetFinvizData")]
        public async Task<IActionResult> GetFinvizData(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker))
            {
                return BadRequest("Ticker is required.");
            }

            try
            {
                // Fetch HTML content from Finviz
                var finvizHtmlContent = await HttpService.FetchHtmlContentAsync($"https://finviz.com/quote.ashx?t={ticker}&ty=c&ta=1&p=d");

                // Prepare the response object
                var response = new Dictionary<string, object>();

                // Extract general stock data
                string finvizGenDataPattern = @"<table[^>]*class=""[^""]*js-snapshot-table[^""]*"".*?>.*?</table>";
                Match finvizGenDataMatch = RegexParsing.ExtractHtmlChunk(finvizHtmlContent, finvizGenDataPattern);

                if (finvizGenDataMatch.Success)
                {
                    var generalData = HAPParsing.ParseKeyValuePairs(finvizGenDataMatch.Value, "//td[1]", "//td[2]");
                    response["GeneralData"] = generalData;
                }

                // Extract analyst actions
                string analystActionsPattern = @"<table[^>]*class=""[^""]*js-table-ratings[^""]*styled-table-new[^""]*"".*?>.*?</table>";
                Match analystActionsMatch = RegexParsing.ExtractHtmlChunk(finvizHtmlContent, analystActionsPattern);

                if (analystActionsMatch.Success)
                {
                    var analystActions = HAPParsing.ParseAnalystActions(analystActionsMatch.Value);
                    response["AnalystActions"] = analystActions;
                }

                // Extract insider trading data
                string insiderTradingPattern = @"<table[^>]*class=""[^""]*styled-table-new[^""]*""[^>]*>.*?<th[^>]*>Insider Trading</th>.*?</table>";
                Match insiderTradingMatch = RegexParsing.ExtractHtmlChunk(finvizHtmlContent, insiderTradingPattern);

                if (insiderTradingMatch.Success)
                {
                    var insiderData = HAPParsing.ParseInsiderTradingTable(insiderTradingMatch.Value);
                    response["InsiderTradingData"] = insiderData;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Endpoint for Finviz graph data
        [HttpGet("GetGraphData")]
        public IActionResult GetGraphData(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker))
            {
                return BadRequest("Ticker is required.");
            }

            try
            {
                // Fetch HTML content from Finviz
                var finvizHtmlContent = HttpService.FetchHtmlContentAsync($"https://finviz.com/quote.ashx?t={ticker}&ty=c&ta=1&p=d").Result;

                // Extract graph data
                string graphDataPattern = @"var\s+data\s*=\s*(\{.*?\});";
                Match graphDataMatch = RegexParsing.ExtractHtmlChunk(finvizHtmlContent, graphDataPattern);

                if (graphDataMatch.Success)
                {
                    string graphData = graphDataMatch.Groups[1].Value;
                    return Ok(graphData);
                }
                else
                {
                    return NotFound("Graph data not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Test endpoint
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API is working!");
        }
    }
}
