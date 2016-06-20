﻿using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Mock4Net.Core;
using Mock4Net.Core.Models;


namespace Mock4Net.MockServer.Controllers
{
    public class MockserverController : ApiController
    {
        [HttpPost]
        public void AddMock(MockDefinition mock)
        {
            var server = GetMockServer();
            server.AddMock(mock.RequestCondition, mock.Response);
        }

        private IMockServerManager GetMockServer()
        {
            return Request.GetOwinContext().Get<IMockServerManager>("FluentMockServer");
        }

        [HttpPost]
        public void ResetMocks()
        {
            var server = GetMockServer();
            server.Reset();
        }


        [HttpPost]
        public List<Mock4Net.Core.Models.Request> SearchLogsFor(RequestCondition condition)
        {
            var server = GetMockServer();
            return server.SearchLogsFor(condition);
        }


       

    }


}
