using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace Library.API.Helpers
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchesMediaType : Attribute, IActionConstraint
    {
        private readonly string _requestHeaderToMatch;
        private readonly string[] _mediaTypes;

        public RequestHeaderMatchesMediaType(string requestHeaderToMatch, string[] mediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch;
            _mediaTypes = mediaTypes;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;
            if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
                return false;

            // if one of the media types matches, return true
            foreach (var mediaType in _mediaTypes)
            {
                var mediaTypeMatches = string.Equals(requestHeaders[_requestHeaderToMatch].ToString(),
                    mediaType, StringComparison.OrdinalIgnoreCase);

                if (mediaTypeMatches)
                    return true;
            }

            return false;
        }
    }
}
