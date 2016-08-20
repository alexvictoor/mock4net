using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public interface IProvideResponses
    {
        Task<Response> ProvideResponse(Request request);
    }
}
