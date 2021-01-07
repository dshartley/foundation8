using System;
using System.Net;
using System.Net.Http;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Smart.Platform.Net.Http
{
    /// <summary>
    /// A helper class for Http.
    /// </summary>
    public class HttpHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class.
        /// </summary>
        private HttpHelper() { }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the absolute URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static string GetAbsoluteUrl(HttpRequest request, string relativePath)
        {
            #region Check Parameters

            if (request == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "request is nothing"));
            if (string.IsNullOrEmpty(relativePath)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativePath is nothing"));

            #endregion


            string r = request.GetEncodedPathAndQuery();

            // TODO: Deprecated in .NET Core
            //string r = request.Url.GetLeftPart(UriPartial.Authority) + page.ResolveUrl(relativePath);

            return r;
        }

        /// <summary>
        /// Create an HttpActionResult.
        /// </summary>
        /// <param name="statusCode">The statusCode.</param>
        /// <param name="jsonObject">The jsonObject.</param>
        /// <returns></returns>
        public static IActionResult CreateHttpActionResult(HttpStatusCode statusCode, DataJSONWrapper jsonObject)
        {
            #region Check Parameters

            if (jsonObject == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "jsonObject is nothing"));

            #endregion

            // Serialize to the JSON string
            string              s = JSONHelper.SerializeFromJSON(jsonObject);

            ContentResult       result = new ContentResult();
            result.Content      = s;
            result.ContentType  = "application/json";
            result.StatusCode   = (int)statusCode;

            return result;
        }

        #endregion
    }
}
