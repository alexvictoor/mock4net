using System.Collections.Concurrent;
using System.Collections.Generic;

[module:
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
    /// The response.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// The _headers.
        /// </summary>
        private readonly IDictionary<string, string> _headers = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// The status code.
        /// </summary>
        public volatile int StatusCode = 200;

        /// <summary>
        /// The body.
        /// </summary>
        public volatile string Body;

        /// <summary>
        /// Gets the headers.
        /// </summary>
        public IDictionary<string, string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// The add header.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddHeader(string name, string value)
        {
            _headers.Add(name, value);
        }
    }
}