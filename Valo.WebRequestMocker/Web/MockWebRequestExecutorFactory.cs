using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valo.WebRequestMocker.Web
{
    public class MockWebRequestExecutorFactory : WebRequestExecutorFactory
    {
        IMockResponseProvider ResponseProvider { get; }
        public MockWebRequestExecutorFactory(IMockResponseProvider responseProvider)
        {
            ResponseProvider = responseProvider;
        }
        public override WebRequestExecutor CreateWebRequestExecutor(ClientRuntimeContext context, string requestUrl)
        {
            return new MockWebRequestExecutor(requestUrl, ResponseProvider);
        }
    }
}
