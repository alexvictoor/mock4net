﻿[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules",
        "SA1309:FieldNamesMustNotBeginWithUnderscore",
        Justification = "Reviewed. Suppression is OK here, as it conflicts with internal naming rules.")]
[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]
// ReSharper disable InconsistentNaming
namespace Mock4Net.Core
{
    /// <summary>
    /// The request url spec.
    /// </summary>
    public class RequestUrlSpec : ISpecifyRequests
    {
        /// <summary>
        /// The _url.
        /// </summary>
        private readonly string _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestUrlSpec"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        public RequestUrlSpec(string url)
        {
            _url = url;
        }

        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsSatisfiedBy(Request request)
        {
            return WildcardPatternMatcher.MatchWildcardString(_url, request.Url);
        }
    }
}
