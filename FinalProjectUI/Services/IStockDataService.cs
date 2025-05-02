using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public interface IStockDataService
{
    Task<Dictionary<string, object>> GetZacksData(string ticker);
    Task<Dictionary<string, object>> GetFinvizData(string ticker);
    Task<List<CandlestickData>> GetGraphData(string ticker);
}
