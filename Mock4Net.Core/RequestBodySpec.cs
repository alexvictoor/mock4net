[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core
{
    /// <summary>
    /// The request body spec.
    /// </summary>
    public class RequestBodySpec : ISpecifyRequests
    {
        /// <summary>
        /// The _body.
        /// </summary>
        private string _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBodySpec"/> class.
        /// </summary>
        /// <param name="body">
        /// The body.
        /// </param>
        public RequestBodySpec(string body)
        {
            _body = body.Trim();
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
            return WildcardPatternMatcher.MatchWildcardString(_body, request.Body.Trim());
        }
    }
}
