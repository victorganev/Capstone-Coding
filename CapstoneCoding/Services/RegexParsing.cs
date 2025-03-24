//Use this file to handle all regex parsing. Create a function that can be called to parse the text in question. Put any other regex related functionality in here.

using System.Text.RegularExpressions;

namespace Services
{
    public static class RegexParsing
    {
        public static Match ExtractHtmlChunk(string htmlContent, string regexPattern)
        {
            return Regex.Match(htmlContent, regexPattern, RegexOptions.Singleline);
        }
    }
}