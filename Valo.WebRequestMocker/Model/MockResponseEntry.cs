using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valo.WebRequestMocker.Model
{
    public class MockResponseEntry<T>
    {
        /// <summary>
        /// Absolute url of web service called
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Method name called by CSOM (GetEffectivePermissionMask, EnsureUser ect)
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Dictionary of parameters which should be compared with called api function parameters
        /// </summary>
        public Dictionary<string, string> NameValueParameters { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// Name of requested property (Web, Site)
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Optional. If there is parent method call this is the list of values to compare.
        /// For example when You call Web.Lists.GetByTitle("List Title").GetItems() the ParentParameterValues for GetItems will be {"List Title"}
        /// </summary>
        public List<string> ParentParameterValues { get; set; } = new List<string>();
        /// <summary>
        /// Serialized return value
        /// </summary>
        public string SerializedReturnValue
        {
            get
            {
                return JsonConvert.SerializeObject(ReturnValue);
            }
        }
        public T ReturnValue { get; set; }
    }
    public class MockResponseEntry : MockResponseEntry<object>
    {

    }
}
