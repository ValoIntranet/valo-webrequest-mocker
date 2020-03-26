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
    }
    public class ActionObjectPath : ActionObjectPath<object>
    {

    }

}
