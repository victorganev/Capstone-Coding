//Use this file to handle all the HTTP requests. Create functions to grab the data from the website in question, etc.

using System.Net.Http;
using System.Threading.Tasks;

namespace Services
{
    public static class HttpService
    {
        public static async Task<string> FetchHtmlContentAsync(string url)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetStringAsync(url);
        }
    }
}