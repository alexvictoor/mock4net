using System.Collections.Generic;
using System.Linq;

[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core
{
    /// <summary>
    /// The request param spec.
    /// </summary>
    public class RequestParamSpec : ISpecifyRequests
    {
        /// <summary>
        /// The _key.
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// The _values.
        /// </summary>
        private readonly List<string> _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestParamSpec"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        public RequestParamSpec(string key, List<string> values)
        {
            _key = key;
            _values = values;
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
            return request.GetParameter(_key).Intersect(_values).Count() == _values.Count();
        }
    }
}
