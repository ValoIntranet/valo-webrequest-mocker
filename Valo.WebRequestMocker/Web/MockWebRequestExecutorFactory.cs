using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valo.WebRequestMocker.Helpers;
using Valo.WebRequestMocker.Model;

namespace Valo.WebRequestMocker.Web
{
    public class MockWebRequestExecutorFactory : WebRequestExecutorFactory
    {
        public bool RunAsIntegrationTest { get; set; }
        IMockResponseProvider ResponseProvider { get; }
        public event EventHandler<RequestExecutedArgs> OnRequestExecuted;
        public IMockDataRepo MockDataRepository { get; set; }
        protected MockDataCreator MockCreator { get; set; }
        public MockWebRequestExecutorFactory(IMockResponseProvider responseProvider, 
            bool runAsIntegrationTests = false,
            IMockDataRepo repo = null)
        {
            ResponseProvider = responseProvider;
            RunAsIntegrationTest = runAsIntegrationTests;
            MockDataRepository = repo;
            if (RunAsIntegrationTest)
            {
                MockCreator = new MockDataCreator(MockDataRepository);
            }
            else if (MockDataRepository != null)
            {
                ResponseProvider = new MockEntryResponseProvider()
                {
                    ResponseEntries = MockDataRepository.LoadMockData<object>()
                };
            }
        }
        public override WebRequestExecutor CreateWebRequestExecutor(ClientRuntimeContext context, string requestUrl)
        {
            if(RunAsIntegrationTest)
            {
                ComposedWebRequestExecutor executor = new ComposedWebRequestExecutor(new SPWebRequestExecutor(context, requestUrl));
                executor.OnRequestExecuted += OnRequestExecuted;
                if(MockDataRepository != null)
                {
                    executor.OnRequestExecuted += delegate (object sender, RequestExecutedArgs e)
                    {
                        MockCreator.AddToMockResponse(e);
                    };
                }
                return executor;
            }
            return new MockWebRequestExecutor(requestUrl, ResponseProvider);
        }

        public void SaveMockData()
        {
            if (RunAsIntegrationTest && MockDataRepository != null)
            {
                MockDataRepository.SaveMockData(MockCreator.Responses);
            }
        }
    }
}
