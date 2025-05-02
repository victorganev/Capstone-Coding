using System.Net.Http;
using System.Threading.Tasks;

namespace Services
{
    public static class HttpService
    {
        //Method to fetch HTML content from a given URL
        public static async Task<string> FetchHtmlContentAsync(string url)
        {
            using var httpClient = new HttpClient();
            try
            {
                return await httpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

            return await httpClient.GetStringAsync(url);
        }
    }
}