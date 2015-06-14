using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public interface IProvideResponses
    {
        Task<Response> ProvideResponse(Request request);
    }
}
