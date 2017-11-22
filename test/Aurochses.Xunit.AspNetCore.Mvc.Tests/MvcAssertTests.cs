using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Aurochses.Xunit.AspNetCore.Mvc.Tests
{
    public class MvcAssertTests
    {
        [Fact]
        public void ViewResult_ThrowsIsTypeException_WhenIActionResultIsNotViewResult()
        {
            // Arrange
            var mockActionResult = new Mock<IActionResult>(MockBehavior.Strict);

            // Act & Assert
            Assert.Throws<IsTypeException>(() => MvcAssert.ViewResult(mockActionResult.Object));
        }

        [Fact]
        public void ViewResult_ThrowsEqualException_WhenViewNameNotEqual()
        {
            // Arrange
            var viewResult = new ViewResult {ViewName = "Index" };

            // Act & Assert
            Assert.Throws<EqualException>(() => MvcAssert.ViewResult(viewResult, "NotIndex"));
        }

        [Fact]
        public void ViewResult_ThrowsTrueException_WhenModelNotEqual()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = new {Value = "TestValue"}
                }
            };

            // Act & Assert
            Assert.Throws<TrueException>(() => MvcAssert.ViewResult(viewResult, null, new {OtherValue = "TestValue"}));
        }

        [Fact]
        public void ViewResult_ReturnViewResult()
        {
            // Arrange
            var viewResult = new ViewResult();

            // Act & Assert
            var result = Assert.IsType<ViewResult>(MvcAssert.ViewResult(viewResult));
            Assert.Equal(viewResult, result);
        }

        [Fact]
        public void ViewData_ThrowsEqualException_WhenViewDataNotEqual()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    new KeyValuePair<string, object>("TestKey", "TestValue")
                }
            };

            // Act & Assert
            Assert.Throws<EqualException>(
                () => MvcAssert.ViewData(
                    viewResult,
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("TestKey", "OtherTestValue")
                    }
                )
            );
        }

        [Fact]
        public void ViewData_ReturnViewData()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    new KeyValuePair<string, object>("TestKey", "TestValue")
                }
            };

            // Act & Assert
            var result = Assert.IsType<ViewDataDictionary>(
                MvcAssert.ViewData(
                    viewResult,
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("TestKey", "TestValue")
                    }
                )
            );

            Assert.Equal(viewResult.ViewData, result);
        }

        [Fact]
        public void ViewData_WithKeyValue_ReturnViewData()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    new KeyValuePair<string, object>("TestKey", "TestValue")
                }
            };

            // Act & Assert
            var result = Assert.IsType<ViewDataDictionary>(
                MvcAssert.ViewData(
                    viewResult,
                    "TestKey",
                    "TestValue"
                )
            );

            Assert.Equal(viewResult.ViewData, result);
        }

        [Fact]
        public void ModelState_IsValid_WhenErrorsNull()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            };

            // Act & Assert
            var result = Assert.IsType<ModelStateDictionary>(
                MvcAssert.ModelState(
                    viewResult,
                    null
                )
            );

            Assert.Equal(viewResult.ViewData.ModelState, result);
        }

        [Fact]
        public void ModelState_IsValid_WhenErrorsEmpty()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            };

            // Act & Assert
            var result = Assert.IsType<ModelStateDictionary>(
                MvcAssert.ModelState(
                    viewResult,
                    new List<KeyValuePair<string, string>>()
                )
            );

            Assert.Equal(viewResult.ViewData.ModelState, result);
        }

        [Fact]
        public void ModelState_IsNotValid_WhenErrors()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            };

            viewResult.ViewData.ModelState.AddModelError("ErrorKey", "ErrorMessage");

            // Act & Assert
            var result = Assert.IsType<ModelStateDictionary>(
                MvcAssert.ModelState(
                    viewResult,
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("ErrorKey", "ErrorMessage")
                    }
                )
            );

            Assert.Equal(viewResult.ViewData.ModelState, result);
        }

        [Fact]
        public void ModelState_WithKeyValue_ReturnModelState()
        {
            // Arrange
            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            };

            viewResult.ViewData.ModelState.AddModelError("ErrorKey", "ErrorMessage");

            // Act & Assert
            var result = Assert.IsType<ModelStateDictionary>(
                MvcAssert.ModelState(
                    viewResult,
                    "ErrorKey",
                    "ErrorMessage"
                )
            );

            Assert.Equal(viewResult.ViewData.ModelState, result);
        }

        [Fact]
        public void JsonResult_ThrowsIsTypeException_WhenIActionResultIsNotJsonResult()
        {
            // Arrange
            var mockActionResult = new Mock<IActionResult>(MockBehavior.Strict);

            // Act & Assert
            Assert.Throws<IsTypeException>(() => MvcAssert.JsonResult(mockActionResult.Object, new { Value = "TestValue" }));
        }

        [Fact]
        public void JsonResult_ThrowsTrueException_WhenValueNotEqual()
        {
            // Arrange
            var jsonResult = new JsonResult(new {Value = "TestValue"});

            // Act & Assert
            Assert.Throws<TrueException>(() => MvcAssert.JsonResult(jsonResult, new {OtherValue = "TestValue"}));
        }

        [Fact]
        public void JsonResult_ReturnJsonResult()
        {
            // Arrange
            var jsonResult = new JsonResult(new { Value = "TestValue" });

            // Act & Assert
            var result = Assert.IsType<JsonResult>(MvcAssert.JsonResult(jsonResult, new { Value = "TestValue" }));
            Assert.Equal(jsonResult, result);
        }

        [Fact]
        public void RedirectToActionResult_ThrowsIsTypeException_WhenIActionResultIsNotRedirectToActionResult()
        {
            // Arrange
            var mockActionResult = new Mock<IActionResult>(MockBehavior.Strict);

            // Act & Assert
            Assert.Throws<IsTypeException>(() => MvcAssert.RedirectToActionResult(mockActionResult.Object, "Index"));
        }

        [Fact]
        public void RedirectToActionResult_ThrowsEqualException_WhenActionNameNotEqual()
        {
            // Arrange
            var redirectToActionResult = new RedirectToActionResult("Index", null, null);

            // Act & Assert
            Assert.Throws<EqualException>(
                () => MvcAssert.RedirectToActionResult(
                    redirectToActionResult,
                    "NotIndex"
                )
            );
        }

        [Fact]
        public void RedirectToActionResult_ThrowsEqualException_WhenControllerNameNotEqual()
        {
            // Arrange
            var redirectToActionResult = new RedirectToActionResult("Index", "Home", null);

            // Act & Assert
            Assert.Throws<EqualException>(
                () => MvcAssert.RedirectToActionResult(
                    redirectToActionResult,
                    "Index",
                    "NotHome"
                )
            );
        }

        [Fact]
        public void RedirectToActionResult_ThrowsEqualException_WhenRouteValuesNotEqual()
        {
            // Arrange
            var redirectToActionResult = new RedirectToActionResult("Index", "Home", new {ReturnUrl = "ReturnUrl"});

            // Act & Assert
            Assert.Throws<EqualException>(
                () => MvcAssert.RedirectToActionResult(
                    redirectToActionResult,
                    "Index",
                    "Home",
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("ReturnUrl", "NotReturnUrl")
                    }
                )
            );
        }

        [Fact]
        public void RedirectToActionResult_ReturnRedirectToActionResult()
        {
            // Arrange
            var redirectToActionResult = new RedirectToActionResult("Index", "Home", new {ReturnUrl = "ReturnUrl"});

            // Act & Assert
            var result = Assert.IsType<RedirectToActionResult>(
                MvcAssert.RedirectToActionResult(
                    redirectToActionResult,
                    "Index",
                    "Home",
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("ReturnUrl", "ReturnUrl")
                    }
                )
            );

            Assert.Equal(redirectToActionResult, result);
        }

        [Fact]
        public void RedirectResult_ThrowsIsTypeException_WhenIActionResultIsNotRedirectResult()
        {
            // Arrange
            var mockActionResult = new Mock<IActionResult>(MockBehavior.Strict);

            // Act & Assert
            Assert.Throws<IsTypeException>(() => MvcAssert.RedirectResult(mockActionResult.Object, "http://example.com"));
        }

        [Fact]
        public void RedirectResult_ThrowsEqualException_WhenUrlNotEqual()
        {
            // Arrange
            var redirectResult = new RedirectResult("http://example.com");

            // Act & Assert
            Assert.Throws<EqualException>(
                () => MvcAssert.RedirectResult(
                    redirectResult,
                    "http://other.example.com"
                )
            );
        }

        [Fact]
        public void RedirectResult_ReturnRedirectResult()
        {
            // Arrange
            var redirectResult = new RedirectResult("http://example.com");

            // Act & Assert
            var result = Assert.IsType<RedirectResult>(
                MvcAssert.RedirectResult(
                    redirectResult,
                    "http://example.com"
                )
            );

            Assert.Equal(redirectResult, result);
        }

        [Fact]
        public void ChallengeResult_ThrowsIsTypeException_WhenIActionResultIsNotChallengeResult()
        {
            // Arrange
            var mockActionResult = new Mock<IActionResult>(MockBehavior.Strict);

            // Act & Assert
            Assert.Throws<IsTypeException>(() => MvcAssert.ChallengeResult(mockActionResult.Object, new AuthenticationProperties(), "TestProvider"));
        }

        [Fact]
        public void ChallengeResult_ThrowsEqualException_WhenAuthenticationPropertiesNotEqual()
        {
            // Arrange
            var challengeResult = new ChallengeResult(new AuthenticationProperties(new AttributeDictionary {{"TestKey", "TestValue"}}));

            // Act & Assert
            Assert.Throws<EqualException>(
                () => MvcAssert.ChallengeResult(
                    challengeResult,
                    new AuthenticationProperties(new AttributeDictionary {{"TestKey", "OtherTestValue"}}),
                    "TestProvider"
                )
            );
        }

        [Fact]
        public void ChallengeResult_ThrowsContainsException_WhenNoSuchProvider()
        {
            // Arrange
            var authenticationProperties = new AuthenticationProperties(new AttributeDictionary { { "TestKey", "TestValue" } });

            var challengeResult = new ChallengeResult
            {
                Properties = authenticationProperties,
                AuthenticationSchemes = new List<string> { "TestProvider" }
            };

            // Act & Assert
            Assert.Throws<ContainsException>(
                () => MvcAssert.ChallengeResult(
                    challengeResult,
                    authenticationProperties,
                    "OtherTestProvider"
                )
            );
        }

        [Fact]
        public void ChallengeResult_ReturnChallengeResult()
        {
            // Arrange
            var authenticationProperties = new AuthenticationProperties(new AttributeDictionary { { "TestKey", "TestValue" } });

            var challengeResult = new ChallengeResult
            {
                Properties = authenticationProperties,
                AuthenticationSchemes = new List<string> { "TestProvider" }
            };

            // Act & Assert
            var result = Assert.IsType<ChallengeResult>(
                MvcAssert.ChallengeResult(
                    challengeResult,
                    authenticationProperties,
                    "TestProvider"
                )
            );

            Assert.Equal(challengeResult, result);
        }
    }
}