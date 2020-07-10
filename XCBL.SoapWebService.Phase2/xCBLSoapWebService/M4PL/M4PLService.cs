using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using xCBLSoapWebService.Common;
using xCBLSoapWebService.M4PL.Authentication;

namespace xCBLSoapWebService.M4PL
{
    public class M4PLService
    {
        public static T CallM4PLAPI<T>(object input,string  url, string methodType = "POST",bool isElectrolux = false, string baseUrl = "", string clientId = "",string userName = "", string Password ="")
        {

            string dataContentType = "application/json";
            var response = default(T);
            string authToken = TokenHelper.GetAuthToken(isElectrolux, baseUrl, clientId, userName, Password);
            string commentJson = string.Empty;
            string serviceCall = string.Format("{0}/{1}", string.IsNullOrEmpty(baseUrl) ? ConfigurationManager.AppSettings["M4PLProdAPI"] : baseUrl,url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceCall);
            request.KeepAlive = false;
            request.ContentType = dataContentType;
            request.Method = methodType;
            request.Headers.Add(HttpRequestHeader.Authorization, authToken);
            
            string inputrequest = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            byte[] buffer = Encoding.UTF8.GetBytes(inputrequest);
            using (Stream requestContentStream = (Stream)new MemoryStream(buffer))
            {
                request.Accept = dataContentType;
                request.ContentType = dataContentType;
                if (requestContentStream != null)
                {
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestContentStream.CopyTo(requestStream);
                    }
                }
            }

            var webResponse = (HttpWebResponse)request.GetResponse();
            using (Stream responseStream = webResponse.GetResponseStream())
            {
                var httpResponse = SerializationHelper.Deserialize<M4PLHttpResponse>(responseStream,webResponse.ContentLength, dataContentType);
                response = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(httpResponse.Results));
               // response = httpResponse.Status;
            }

            webResponse.Close();

            return response;
        }
    }
}