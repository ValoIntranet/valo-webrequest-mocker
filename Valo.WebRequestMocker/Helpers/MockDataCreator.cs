using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valo.WebRequestMocker.Model;
using Valo.WebRequestMocker.Web;

namespace Valo.WebRequestMocker.Helpers
{
    public class MockDataCreator
    {
        protected IMockDataRepo Repo { get; }
        public List<MockResponseEntry> Responses { get; } = new List<MockResponseEntry>();
        public MockDataCreator(IMockDataRepo repo)
        {
            Repo = repo;
        }

        public void AddToMockResponse(RequestExecutedArgs requestExecutedArgs)
        {
            if(requestExecutedArgs.RequestBody.Contains("GetUpdatedFormDigestInformation "))
            {
                return;
            }
            CSOMRequest request = MockEntryResponseProvider.GetRequest(requestExecutedArgs.RequestBody);
            List<ActionObjectPath<object>> requestedOperations = MockEntryResponseProvider.GetActionObjectPathsFromRequest<object>(request);
            foreach(ActionObjectPath<object> requestedOperation in requestedOperations)
            {
                MockResponseEntry mockResponseEntry = requestedOperation.GenerateMock(requestExecutedArgs.ResponseBody, requestExecutedArgs.CalledUrl);
                if(mockResponseEntry != null && mockResponseEntry.SerializedReturnValue != "{\"IsNull\":false}")
                {
                    Responses.Add(mockResponseEntry);
                }
            }
        }

        public void Save()
        {
            Repo.SaveMockData(Responses);
        }
    }
}
