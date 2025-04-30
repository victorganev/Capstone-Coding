//Use this file to handle all HtmlAgilityPack (HAP) parsing. Any functionality related to HAP parsing goes in here.

using System.Collections.Generic;
using HtmlAgilityPack;
using Models;

namespace Services
{
    public static class HAPParsing
    {
        public static string ParseSingleNode(string htmlChunk, string xpath)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlChunk);

            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);
            return node?.InnerText.Trim();
        }

        public static List<string> ParseKeyValuePairs(string htmlChunk, string keyXPath, string valueXPath)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlChunk);

            var rows = htmlDoc.DocumentNode.SelectNodes("//tr");
            var keyValuePairs = new List<string>();

            if (rows != null)
            {
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
                            keyValuePairs.Add($"{key}: {value}");
                        }

                        // Handle odd number of cells (e.g., last cell without a pair)
                        if (cells.Count % 2 != 0)
                        {
                            string key = cells[cells.Count - 1].InnerText.Trim();
                            keyValuePairs.Add($"{key}: N/A");
                        }
                    }
                }
            }

            return keyValuePairs;
        }

        public static List<AnalystAction> ParseAnalystActions(string htmlChunk)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlChunk);

            var rows = htmlDoc.DocumentNode.SelectNodes("//tr");
            var analystActions = new List<AnalystAction>();

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("td");
                    if (cells != null && cells.Count >= 5)
                    {
                        var action = new AnalystAction
                        {
                            Date = cells[0].InnerText.Trim(),
                            Action = cells[1].InnerText.Trim(),
                            Analyst = cells[2].InnerText.Trim(),
                            RatingChange = cells[3].InnerText.Trim().Replace("&rarr;", "to").Trim(),
                            PriceTargetChange = cells[4].InnerText.Trim().Replace("&rarr;", "to").Trim()
                        };

                        analystActions.Add(action);
                    }
                }
            }

            return analystActions;
        }

        public static List<InsiderTradingData> ParseInsiderTradingTable(string htmlChunk)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlChunk);

            var rows = htmlDoc.DocumentNode.SelectNodes("//tr");
            var insiderDataList = new List<InsiderTradingData>();

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("td");
                    if (cells != null && cells.Count >= 9)
                    {
                        var secLinkNode = cells[8].SelectSingleNode(".//a");
                        string secLink = secLinkNode?.GetAttributeValue("href", string.Empty);
                        string secDate = cells[8].InnerText.Replace(secLinkNode?.OuterHtml ?? "", "").Trim();

                        var insiderData = new InsiderTradingData
                        {
                            InsiderTrading = cells[0].InnerText.Trim(),
                            Relationship = cells[1].InnerText.Trim(),
                            Date = cells[2].InnerText.Trim(),
                            Transaction = cells[3].InnerText.Trim(),
                            Cost = cells[4].InnerText.Trim(),
                            Shares = cells[5].InnerText.Trim(),
                            Value = cells[6].InnerText.Trim(),
                            SharesTotal = cells[7].InnerText.Trim(),
                            SecDate = secDate,
                            SecLink = secLink
                        };

                        insiderDataList.Add(insiderData);
                    }
                }
            }

            return insiderDataList;
        }

        public static List<string> ParseStockActivityData(string htmlChunk)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlChunk);

            // Grabbing both <dt> and <dd> elements from the extracted chunk
            var dtNodes = htmlDoc.DocumentNode.SelectNodes("//dt");
            var ddNodes = htmlDoc.DocumentNode.SelectNodes("//dd");

            var stockDataList = new List<string>();

            // Filtering for inner text of HTML elements and organizing into key-value pairs
            if (dtNodes != null && ddNodes != null && dtNodes.Count == ddNodes.Count)
            {
                for (int i = 0; i < dtNodes.Count; i++)
                {
                    string key = dtNodes[i].InnerText.Trim();
                    string value = ddNodes[i].InnerText.Trim();
                    stockDataList.Add($"{key}: {value}");
                }
            }

            return stockDataList;
        }
    }
}