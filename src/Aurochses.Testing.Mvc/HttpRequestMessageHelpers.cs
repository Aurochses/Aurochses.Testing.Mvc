using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Aurochses.Testing.Mvc
{
    /// <summary>
    /// HttpRequestMessageHelpers.
    /// </summary>
    public static class HttpRequestMessageHelpers
    {
        /// <summary>
        /// Setup Request as an asynchronous operation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="formData">The form data.</param>
        /// <param name="response">The response.</param>
        /// <returns>Task.</returns>
        public static async Task SetupAsync(this HttpRequestMessage request, Dictionary<string, string> formData, HttpResponseMessage response)
        {
            // Add AntiForgeryToken
            var antiForgeryToken = await response.GetAntiForgeryTokenAsync();
            if (!string.IsNullOrWhiteSpace(antiForgeryToken))
            {
                formData.Add("__RequestVerificationToken", antiForgeryToken);
            }

            // Set FormUrlEncodedContent
            request.Content = new FormUrlEncodedContent(formData);

            // Add Cookies
            var cookies = response.GetCookies();
            cookies
                .Keys
                .ToList()
                .ForEach(
                    key =>
                    {
                        request.Headers.Add("Cookie", new CookieHeaderValue(key, cookies[key]).ToString());
                    }
                );
        }
    }
}