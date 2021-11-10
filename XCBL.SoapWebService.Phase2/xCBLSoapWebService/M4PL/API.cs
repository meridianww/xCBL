using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using xCBLSoapWebService.M4PL.Authentication;
using xCBLSoapWebService.M4PL.Entities;

namespace xCBLSoapWebService.M4PL
{
    public static class API
    {
        private static string baseUrl = ConfigurationManager.AppSettings["M4PLProdAPI"];

        public static Job GetJobInformation(long jobId)
        {
            Job job = new Job();
            string authToken = TokenHelper.GetAuthToken();
            string serviceCall = $"{baseUrl}/Jobs/GetJob?jobId={jobId}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceCall);
            request.KeepAlive = false;
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, authToken);
            try
            {
                WebResponse response = request.GetResponse();

                using (Stream jobInformation = response.GetResponseStream())
                {
                    using (TextReader jobInformationReader = new StreamReader(jobInformation))
                    {
                        string jobInformationResponse = jobInformationReader.ReadToEnd();

                        using (var stringReader = new StringReader(jobInformationResponse))
                        {
                            API_Job_Response jobResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<API_Job_Response>(jobInformationResponse);
                            job = jobResponse.Results[0];
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return job;
        }

        public static SearchOrder SearchJob(string contractNumber)
        {
            SearchOrder job = new SearchOrder();
            string authToken = TokenHelper.GetAuthToken();
            string serviceCall = $"{baseUrl}/JobServices/GetSearchOrder?search={contractNumber}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceCall);
            request.KeepAlive = false;
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, authToken);
            try
            {
                WebResponse response = request.GetResponse();

                using (Stream jobInformation = response.GetResponseStream())
                {
                    using (TextReader jobInformationReader = new StreamReader(jobInformation))
                    {
                        string jobInformationResponse = jobInformationReader.ReadToEnd();

                        using (var stringReader = new StringReader(jobInformationResponse))
                        {
                            API_Search_Response jobResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<API_Search_Response>(jobInformationResponse);
                            job = jobResponse.Results[0][0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return job;
        }
    }
}