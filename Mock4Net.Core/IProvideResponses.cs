using System.Diagnostics.CodeAnalysis;

[module:
    SuppressMessage("StyleCop.CSharp.DocumentationRules", 
        "SA1633:FileMustHaveHeader", 
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core
{
    using System.Threading.Tasks;

    /// <summary>
    /// The ProvideResponses interface.
    /// </summary>
    public interface IProvideResponses
    {
        /// <summary>
        /// The provide response.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Response> ProvideResponse(Request request);
    }
}
