using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Aurochses.Xunit.AspNetCore.Mvc
{
    /// <summary>
    /// HttpResponseMessageHelpers.
    /// </summary>
    public static class HttpResponseMessageHelpers
    {
        /// <summary>
        /// Get anti forgery token as an asynchronous operation.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static async Task<string> GetAntiForgeryTokenAsync(this HttpResponseMessage response)
        {
            var httpResponseMessageAsString = await response.Content.ReadAsStringAsync();

            var match = Regex.Match(httpResponseMessageAsString, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
        public static IDictionary<string, string> GetCookies(this HttpResponseMessage response)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                SetCookieHeaderValue
                    .ParseList(values.ToList())
                    .ToList()
                    .ForEach(
                        cookie =>
                        {
                            result.Add(cookie.Name, cookie.Value);
                        }
                    );
            }

            return result;
        }
    }
}