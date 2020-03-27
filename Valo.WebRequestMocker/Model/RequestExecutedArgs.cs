using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valo.WebRequestMocker.Model
{
    public class RequestExecutedArgs : EventArgs
    {
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public string CalledUrl { get; set; }
    }
}
