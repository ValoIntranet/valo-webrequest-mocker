using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valo.WebRequestMocker.Web
{
    /// <summary>
    /// Checks if request body contains dictionary key and returns value
    /// </summary>
    public class DictionaryResponseProvider : IMockResponseProvider
    {
        public Dictionary<string, string> PropertyResponseMapping { get; set; } = new Dictionary<string, string>();
        public virtual string GetResponse(string url, string verb, string body)
        {
            foreach (var mapping in PropertyResponseMapping)
            {
                if (body.Contains(mapping.Key))
                {
                    return mapping.Value;
                }
            }
            return String.Empty;
        }
    }
    /// <summary>
    /// Checks if request body contains dictionary key and returns file content from path provided in dictionary value
    /// </summary>
    public class DictionaryFileResponseProvider : DictionaryResponseProvider
    {
        public override string GetResponse(string url, string verb, string body)
        {
            return System.IO.File.ReadAllText(base.GetResponse(url, verb, body));
        }
    }
}
