using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public class StockDataService : IStockDataService
{
    private readonly HttpClient _httpClient;

    public StockDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, object>> GetZacksData(string ticker)
    {
        return await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"StockData/GetZacksData?ticker={ticker}");
    }

    public async Task<Dictionary<string, object>> GetFinvizData(string ticker)
    {
        return await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"StockData/GetFinvizData?ticker={ticker}");
    }

    public async Task<List<CandlestickData>> GetGraphData(string ticker)
    {
        return await _httpClient.GetFromJsonAsync<List<CandlestickData>>($"StockData/GetGraphData?ticker={ticker}");
    }
}
