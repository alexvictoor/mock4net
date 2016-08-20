using System.Threading.Tasks;

[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core
{
    /// <summary>
    /// The route.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// The _request spec.
        /// </summary>
        private readonly ISpecifyRequests _requestSpec;

        /// <summary>
        /// The _provider.
        /// </summary>
        private readonly IProvideResponses _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="requestSpec">
        /// The request spec.
        /// </param>
        /// <param name="provider">
        /// The provider.
        /// </param>
        public Route(ISpecifyRequests requestSpec, IProvideResponses provider)
        {
            _requestSpec = requestSpec;
            _provider = provider;
        }

        /// <summary>
        /// The response to.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<Response> ResponseTo(Request request)
        {
            return _provider.ProvideResponse(request);
        }

        /// <summary>
        /// The is request handled.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsRequestHandled(Request request)
        {
            return _requestSpec.IsSatisfiedBy(request);
        }
    }
}
