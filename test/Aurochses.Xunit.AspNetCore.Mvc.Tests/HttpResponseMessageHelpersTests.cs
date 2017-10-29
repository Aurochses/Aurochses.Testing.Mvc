using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Tests
{
    public class HttpResponseMessageHelpersTests
    {
        private readonly Mock<HttpResponseMessage> _mockHttpResponseMessage;

        public HttpResponseMessageHelpersTests()
        {
            _mockHttpResponseMessage = new Mock<HttpResponseMessage>(MockBehavior.Strict);
        }

        [Fact]
        public async Task GetAntiForgeryTokenAsync_EmptyContent_ReturnNull()
        {
            // Arrange
            _mockHttpResponseMessage.Object.Content = new StringContent(string.Empty);

            // Act & Assert
            Assert.Null(await _mockHttpResponseMessage.Object.GetAntiForgeryTokenAsync());
        }

        [Fact]
        public async Task GetAntiForgeryTokenAsync_Content_ReturnAntiForgeryToken()
        {
            // Arrange
            const string value = "AntiForgeryTokenValue";
            const string html = "<form><input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"" + value + "\" /></form>";
            _mockHttpResponseMessage.Object.Content = new StringContent(html);

            // Act & Assert
            Assert.Equal(value, await _mockHttpResponseMessage.Object.GetAntiForgeryTokenAsync());
        }

        [Fact]
        public void GetCookies_Empty()
        {
            // Arrange & Act & Assert
            Assert.Empty(_mockHttpResponseMessage.Object.GetCookies());
        }

        [Fact]
        public void GetCookies_Success()
        {
            // Arrange
            var list = new Dictionary<string, string>
            {
                {"firstCookie", "firstValue"},
                {"secondCookie", "secondValue"}
            };

            _mockHttpResponseMessage.Object.Headers.Add("Set-Cookie", list.Select(x => $"{x.Key}={x.Value}").ToList());

            // Act
            var cookies = _mockHttpResponseMessage.Object.GetCookies();

            // Assert
            Assert.NotEmpty(cookies);
            Assert.Equal(list.Count, cookies.Count);
            foreach (var item in list)
            {
                Assert.Contains(item, cookies);
            }
        }
    }
}