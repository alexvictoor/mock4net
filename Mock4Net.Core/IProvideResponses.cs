using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public interface IProvideResponses
    {
        Response ProvideResponse(Request request);
    }
}
