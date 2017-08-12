using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Aurochses.Testing.Mvc.Tests
{
    public class HttpRequestMessageHelpersTests
    {
        private readonly Mock<HttpRequestMessage> _mockHttpRequestMessage;
        private readonly Mock<HttpResponseMessage> _mockHttpResponseMessage;

        public HttpRequestMessageHelpersTests()
        {
            _mockHttpRequestMessage = new Mock<HttpRequestMessage>(MockBehavior.Strict);
            _mockHttpResponseMessage = new Mock<HttpResponseMessage>(MockBehavior.Strict);
        }

        [Fact]
        public async Task SetupAsync_Success()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                {"firstField", "firstValue"},
                {"secondField", "secondValue"}
            };

            const string antiForgeryTokenValue = "AntiForgeryTokenValue";
            const string responseHtml = "<form><input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"" + antiForgeryTokenValue + "\" /></form>";
            _mockHttpResponseMessage.Object.Content = new StringContent(responseHtml);

            var responseCookies = new Dictionary<string, string>
            {
                {"firstCookie", "firstValue"},
                {"secondCookie", "secondValue"}
            };

            _mockHttpResponseMessage.Object.Headers.Add("Set-Cookie", responseCookies.Select(x => $"{x.Key}={x.Value}").ToList());

            var request = _mockHttpRequestMessage.Object;

            // Act
            await request.SetupAsync(formData, _mockHttpResponseMessage.Object);

            // Assert
            Assert.IsType<FormUrlEncodedContent>(request.Content);

            var content = await request.Content.ReadAsStringAsync();
            Assert.Contains($"__RequestVerificationToken={antiForgeryTokenValue}", content);
            foreach (var item in formData)
            {
                Assert.Contains($"{item.Key}={item.Value}", content);
            }

            var cookies = request.Headers.GetValues("Cookie").ToList();
            Assert.Equal(responseCookies.Count, cookies.Count);
            foreach (var item in responseCookies)
            {
                Assert.True(cookies.Contains($"{item.Key}={item.Value}"));
            }
        }
    }
}