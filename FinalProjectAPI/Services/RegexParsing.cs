using System.Text.RegularExpressions;

//Service file for parsing HTML content using regex
namespace Services
{
    public static class RegexParsing
    {
        //Method to extract a chunk of HTML content from the html of a website using regex
        public static Match ExtractHtmlChunk(string htmlContent, string regexPattern)
        {
            return Regex.Match(htmlContent, regexPattern, RegexOptions.Singleline);
        }
    }
}