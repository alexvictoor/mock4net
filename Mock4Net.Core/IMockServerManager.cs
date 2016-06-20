using System.Collections.Generic;

namespace Mock4Net.Core
{
    public interface IMockServerManager   
    {
        void AddMock(MockServerController.Models.RequestCondition requestCondition, MockServerController.Models.Response response);
        void Reset();
        List<MockServerController.Models.Request> SearchLogsFor(MockServerController.Models.RequestCondition condition);
    }
}