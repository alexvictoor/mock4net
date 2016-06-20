using System.Collections.Generic;

namespace Mock4Net.Core
{
    public interface IMockServerManager   
    {
        void AddMock(Mock4Net.Core.Models.RequestCondition requestCondition, Mock4Net.Core.Models.Response response);
        void Reset();
        List<Mock4Net.Core.Models.Request> SearchLogsFor(Mock4Net.Core.Models.RequestCondition condition);
    }
}