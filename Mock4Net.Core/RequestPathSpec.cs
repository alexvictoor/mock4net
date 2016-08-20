[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core
{
    /// <summary>
    /// The request path spec.
    /// </summary>
    public class RequestPathSpec : ISpecifyRequests
    {
        /// <summary>
        /// The _path.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPathSpec"/> class.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        public RequestPathSpec(string path)
        {
            _path = path;
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
            return WildcardPatternMatcher.MatchWildcardString(_path, request.Path);
        }
    }
}
