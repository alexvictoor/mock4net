using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Mock4Net.Core;
using Mock4Net.MockServerController.Models;


namespace Mock4Net.MockServerController.Controllers
{
    [Authorize]
    public class MockserverController : ApiController
    {
        [HttpPost]
        public void AddMock(MockDefinition mock)
        {
            var server = GetMockServer();
            server.AddMock(mock.RequestCondition, mock.Response);
        }

        [HttpPost]
        public void ResetMocks()
        {
            var server = GetMockServer();
            server.Reset();
        }

        [HttpPost]
        public List<Models.Request> SearchLogsFor(RequestCondition condition)
        {
            var server = GetMockServer();
            return server.SearchLogsFor(condition);
        }


        private IMockServerManager GetMockServer()
        {
            return Request.GetOwinContext().Get<IMockServerManager>("FluentMockServer");
        }

    }


}
