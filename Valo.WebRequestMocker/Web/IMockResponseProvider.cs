using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valo.WebRequestMocker.Web
{
    /// <summary>
    /// Based on provided request details (url, verb and body) returns ProcessQuery or  another WebService response 
    /// </summary>
    public interface IMockResponseProvider
    {
        string GetResponse(string url, string verb, string body);
    }
}
