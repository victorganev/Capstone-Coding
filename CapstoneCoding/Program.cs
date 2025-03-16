using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using HtmlAgilityPack;

class Program
{
    static async Task Main(string[] args)
    {
        //Variable allowing user to choose stock ticker, will need to expand upon this functionality with UI
        var stockTicker = "NVDA";

        // Fetch the HTML content
            //Fetching content from Zacks Investment here
        using var httpClient = new HttpClient();
        var zacksHtmlContent = await httpClient.GetStringAsync("https://www.zacks.com/stock/quote/" + stockTicker +"?q=" + stockTicker);

            //Fetching content from Finviz
        var finvizHtmlContent = await httpClient.GetStringAsync("https://finviz.com/quote.ashx?t=" + stockTicker + "&ty=c&ta=1&p=d");

        // Debugging - print raw HtmlContent 
        // Console.WriteLine(zacksHtmlContent);
        // Console.WriteLine(finvizHtmlContent);

        // Use regex to extract the chunk of HTML containing the needed data
            //Zacks Rank variables
        string zr_RegexPattern = @"<p class=""rank_view"">.*?</p>";
        Match zr_match = Regex.Match(zacksHtmlContent, zr_RegexPattern, RegexOptions.Singleline);
        Match zr_matchFinal = null;

            //Stock price variables
        string price_RegexPattern = @"<p class=""last_price"">.*?</p>";
        Match price_match = Regex.Match(zacksHtmlContent, price_RegexPattern, RegexOptions.Singleline);
        Match price_matchFinal = null;

            //Open price variables
        string open_RegexPattern = @"<section id=""stock_activity"">.*?</section>";
        Match open_match = Regex.Match(zacksHtmlContent, open_RegexPattern, RegexOptions.Singleline);
        Match open_matchFinal = null;

        //Finviz below
            //General stock data table variables
        string finvizGenData_RegexPattern = @"<table[^>]*class=""[^""]*js-snapshot-table[^""]*"".*?>.*?</table>";
        Match finvizGenData_match = Regex.Match(finvizHtmlContent, finvizGenData_RegexPattern, RegexOptions.Singleline);
        Match finvizGenData_matchFinal = null;

        //Checking success of regex match for Zacks Rank, then using additional regex to single out the data needed
        if (zr_match.Success)
        {
            string extractedChunk = zr_match.Value;

            // Further process the extracted chunk to extract the "Zacks Rank" text
            string rankPattern = @"\b\d-(Strong Buy|Buy|Hold|Sell|Strong Sell)\b";
            zr_matchFinal = Regex.Match(extractedChunk, rankPattern);

            //For testing sakes
            if (zr_matchFinal.Success)
            {
                Console.WriteLine("Zacks Rank Extracted Value: " + zr_matchFinal.Value);
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

        //Checking success of regex match for stock price, then using HAP library to single out stock price data
        if (price_match.Success)
        {
            string extractedChunk = price_match.Value;

            // Step 2: Use HtmlAgilityPack to parse the extracted chunk
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(extractedChunk);

            // Use XPath to get the text content of the <p> element
            var pNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@class='last_price']");

            if (pNode != null)
            {
                // Extract the text value (excluding the <span> content)
                string priceText = pNode.FirstChild.InnerText.Trim();
                Console.WriteLine("Extracted Price: " + priceText);
            }
            else
            {
                Console.WriteLine("Could not find the <p> element with class 'last_price'.");
            }
        }
        else
        {
            Console.WriteLine("No matching <p> element found.");
        }

        List<string> stockDataList = new List<string>();

        //Checking success of regex match for the opening price, then using HAP to grab the nodes containing the text data
         if (open_match.Success)
        {
            string extractedChunk = open_match.Value;

            //Setup for HAP parsing
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(extractedChunk);

            //Grabbing both <dt> and <dd> elements from extractedChunk
            var dtNodes = htmlDoc.DocumentNode.SelectNodes("//dt");
            var ddNodes = htmlDoc.DocumentNode.SelectNodes("//dd");

            //Filtering for inner text of html elements and organizing into key-value list
            if (dtNodes != null && ddNodes != null && dtNodes.Count == ddNodes.Count)
            {
                for (int i = 0; i < dtNodes.Count; i++)
                {
                    string key = dtNodes[i].InnerText.Trim();
                    string value = ddNodes[i].InnerText.Trim();
                    stockDataList.Add($"{key}: {value}");
                }
            }
            else
            {
                Console.WriteLine("dtNodes and ddNodes do not have equal lengths or one or both is null.");
            }

            Console.WriteLine("General data about " + stockTicker + " from Stock Activity column on Zacks:");
            foreach (var data in stockDataList)
            {
                Console.WriteLine(data);
            }
        }

        //Checking the success of finvizGenData regex match, then using HAP to grab the table data
        if (finvizGenData_match.Success)
        {
            string extractedChunk = finvizGenData_match.Value;

            // Setup for HAP parsing
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(extractedChunk);

            // Select all rows in the table
            var rows = htmlDoc.DocumentNode.SelectNodes("//tr");

            if (rows != null)
            {
                List<string> finvizDataList = new List<string>();

                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("td");
                    if (cells != null)
                    {
                        // Process cells in pairs
                        for (int i = 0; i < cells.Count - 1; i += 2)
                        {
                            string key = cells[i].InnerText.Trim();
                            string value = cells[i + 1].InnerText.Trim();
                            finvizDataList.Add($"{key}: {value}");
                        }
                    }
                }

                Console.WriteLine("General data from Finviz:");
                foreach (var data in finvizDataList)
                {
                    Console.WriteLine(data);
                }
            }
            else
            {
                Console.WriteLine("No rows found in the Finviz data table.");
            }
        }
        else
        {
            Console.WriteLine("No matching chunk found for Finviz data.");
        }


    }
}