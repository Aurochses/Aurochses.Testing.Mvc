using System.Collections.Generic;
using System.Linq;
using Aurochses.Runtime;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc
{
    /// <summary>
    /// Mvc Assert.
    /// </summary>
    public static class MvcAssert
    {
        /// <summary>
        /// Check view result.
        /// </summary>
        /// <param name="actionResult">The action result.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns>ViewResult.</returns>
        public static ViewResult ViewResult(IActionResult actionResult, string viewName = null, object model = null)
        {
            var viewResult = Assert.IsType<ViewResult>(actionResult);

            Assert.Equal(viewName, viewResult.ViewName);

            Assert.True(model.ValueEquals(viewResult.Model));

            return viewResult;
        }

        /// <summary>
        /// Check view data.
        /// </summary>
        /// <param name="viewResult">The view result.</param>
        /// <param name="items">The items.</param>
        /// <returns>ViewDataDictionary.</returns>
        public static ViewDataDictionary ViewData(ViewResult viewResult, IList<KeyValuePair<string, object>> items)
        {
            var viewData = viewResult.ViewData;

            Assert.Equal(items, viewData);

            return viewData;
        }

        /// <summary>
        /// Check view data.
        /// </summary>
        /// <param name="viewResult">The view result.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ViewDataDictionary.</returns>
        public static ViewDataDictionary ViewData(ViewResult viewResult, string key, object value)
        {
            return ViewData(viewResult, new List<KeyValuePair<string, object>> {new KeyValuePair<string, object>(key, value)});
        }

        /// <summary>
        /// Check model state.
        /// </summary>
        /// <param name="viewResult">The view result.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>ModelStateDictionary.</returns>
        public static ModelStateDictionary ModelState(ViewResult viewResult, IList<KeyValuePair<string, string>> errors)
        {
            var modelState = viewResult.ViewData.ModelState;

            if (errors == null || errors.Count == 0)
            {
                Assert.True(modelState.IsValid);

                Assert.Empty(modelState);

                return modelState;
            }

            Assert.False(modelState.IsValid);

            Assert.Equal(errors.Count, modelState.Count);

            foreach (var error in errors)
            {
                var modelStateEntry = modelState[error.Key];

                Assert.NotNull(modelStateEntry);

                Assert.Contains(modelStateEntry.Errors, x => x.ErrorMessage == error.Value);
            }

            return modelState;
        }

        /// <summary>
        /// Check model state.
        /// </summary>
        /// <param name="viewResult">The view result.</param>
        /// <param name="key">The key.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>ModelStateDictionary.</returns>
        public static ModelStateDictionary ModelState(ViewResult viewResult, string key, string errorMessage)
        {
            return ModelState(viewResult, new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(key, errorMessage) });
        }

        /// <summary>
        /// Check json result.
        /// </summary>
        /// <param name="actionResult">The action result.</param>
        /// <param name="value">The value.</param>
        /// <returns>JsonResult.</returns>
        public static JsonResult JsonResult(IActionResult actionResult, object value)
        {
            var jsonResult = Assert.IsType<JsonResult>(actionResult);

            Assert.True(value.ValueEquals(jsonResult.Value));

            return jsonResult;
        }

        /// <summary>
        /// Check redirect to action result.
        /// </summary>
        /// <param name="actionResult">The action result.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>RedirectToActionResult.</returns>
        public static RedirectToActionResult RedirectToActionResult(IActionResult actionResult, string actionName, string controllerName = null, IList<KeyValuePair<string, object>> routeValues = null)
        {
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);

            Assert.Equal(actionName, redirectToActionResult.ActionName);
            Assert.Equal(controllerName, redirectToActionResult.ControllerName);
            Assert.Equal(routeValues, redirectToActionResult.RouteValues);

            return redirectToActionResult;
        }

        /// <summary>
        /// Check redirect result.
        /// </summary>
        /// <param name="actionResult">The action result.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>RedirectResult.</returns>
        public static RedirectResult RedirectResult(IActionResult actionResult, string returnUrl)
        {
            var redirectResult = Assert.IsType<RedirectResult>(actionResult);

            Assert.Equal(returnUrl, redirectResult.Url);

            return redirectResult;
        }

        /// <summary>
        /// Check challenge result.
        /// </summary>
        /// <param name="actionResult">The action result.</param>
        /// <param name="authenticationProperties">The authentication properties.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>ChallengeResult.</returns>
        public static ChallengeResult ChallengeResult(IActionResult actionResult, AuthenticationProperties authenticationProperties, string provider)
        {
            var challengeResult = Assert.IsType<ChallengeResult>(actionResult);

            Assert.Equal(authenticationProperties, challengeResult.Properties);

            Assert.Contains(provider, challengeResult.AuthenticationSchemes);

            return challengeResult;
        }
    }
}