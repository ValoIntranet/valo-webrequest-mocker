using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Valo.WebRequestMocker.Helpers;

namespace Valo.WebRequestMocker.Web
{
    internal class SPWebRequestExecutor : WebRequestExecutor
    {
        private HttpWebRequest webRequest;
        private HttpWebResponse webResponse;
        private ClientRuntimeContext baseContext;
        private bool setupCredentials;

        public SPWebRequestExecutor(ClientRuntimeContext context, string requestUrl)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (string.IsNullOrEmpty(requestUrl))
                throw new ArgumentNullException(nameof(requestUrl));
            this.baseContext = context;
            this.webRequest = (HttpWebRequest)System.Net.WebRequest.Create(requestUrl);
            this.webRequest.Timeout = context.RequestTimeout;
            this.webRequest.Method = "POST";
            this.webRequest.Pipelined = false;
        }

        public override HttpWebRequest WebRequest
        {
            get
            {
                return this.webRequest;
            }
        }

        public override string RequestContentType
        {
            get
            {
                return this.webRequest.ContentType;
            }
            set
            {
                this.webRequest.ContentType = value;
            }
        }

        public override string RequestMethod
        {
            get
            {
                return this.webRequest.Method;
            }
            set
            {
                this.webRequest.Method = value;
            }
        }

        public override bool RequestKeepAlive
        {
            get
            {
                return this.webRequest.KeepAlive;
            }
            set
            {
                this.webRequest.KeepAlive = value;
            }
        }

        public override WebHeaderCollection RequestHeaders
        {
            get
            {
                return this.webRequest.Headers;
            }
        }
        ComposedStream requestStream;
        public override Stream GetRequestStream()
        {
            if (!this.setupCredentials)
            {
                ClientRuntimeContext.SetupRequestCredential(this.baseContext, this.webRequest);
                this.setupCredentials = true;
            }

            if (requestStream == null)
            {
                requestStream = new ComposedStream(new MemoryStream());
            }
            else if (!requestStream.BaseStream.CanWrite)
            {
                requestStream = new ComposedStream(new MemoryStream());
            }
            return requestStream;
        }

        public override void Execute()
        {
            requestStream.BaseStream.Position = 0;
            requestStream.BaseStream.CopyTo(webRequest.GetRequestStream());
            this.webRequest.GetRequestStream().Close();
            this.webResponse = (HttpWebResponse)webRequest.GetResponse();
            if (responseStream != null)
            {
                responseStream.Dispose();
            }
            responseStream = new MemoryStream();
            this.webResponse.GetResponseStream().CopyTo(responseStream);
        }

        public override async Task ExecuteAsync()
        {
            this.webRequest.GetRequestStream().Close();
            WebResponse resp = (WebResponse)await this.webRequest.GetResponseAsync();
            this.webResponse = (HttpWebResponse)resp;
        }

        public override HttpStatusCode StatusCode
        {
            get
            {
                if (this.webResponse == null)
                    throw new InvalidOperationException();
                return this.webResponse.StatusCode;
            }
        }

        public override string ResponseContentType
        {
            get
            {
                if (this.webResponse == null)
                    throw new InvalidOperationException();
                return this.webResponse.ContentType;
            }
        }

        public override WebHeaderCollection ResponseHeaders
        {
            get
            {
                if (this.webResponse == null)
                    throw new InvalidOperationException();
                return this.webResponse.Headers;
            }
        }
        private MemoryStream responseStream;
        public override Stream GetResponseStream()
        {
            if (this.webResponse == null)
                throw new InvalidOperationException();
            return responseStream;
        }

        public override void Dispose()
        {
            if (this.webResponse == null)
                return;
            this.webResponse.Close();
        }
    }
}
