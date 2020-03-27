using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valo.WebRequestMocker.Model
{
    public class ActionObjectPath<T>
    {
        public string GetResponse(List<MockResponseEntry<T>> mockResponseEntries, CSOMRequest request)
        {
            return  Action.GetResponse(ObjectPath, mockResponseEntries, request);
        }
        public BaseAction Action { get; set; }
        public Identity ObjectPath { get; set; }

        public MockResponseEntry GenerateMock(string responseBody, string calledUrl)
        {
            List<object> possibleResponses = JsonConvert.DeserializeObject<List<object>>(responseBody);
            object responseId = possibleResponses.First(FindResponse);
            object associatedResponse = possibleResponses[possibleResponses.IndexOf(responseId) + 1];

            return ObjectPath.CreateMockResponse(associatedResponse, Action, calledUrl);
        }

        private bool FindResponse(object response)
        {
            if(response is Int64)
            {
                return (Int64)response == Action.Id;
            }
            return false;
        }
    }
    public class ActionObjectPath : ActionObjectPath<object>
    {

    }

}
