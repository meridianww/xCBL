using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace xCBLSoapWebService.M4PL.Authentication
{
    public class TokenHelper
    {
        public static string Token { get; set; }
        public static DateTime ExpirationTime { get; set; }

        public static string GetAuthToken(bool isElectrolux = false)
        {
            return DateTime.UtcNow > ExpirationTime ? GenerateToken(isElectrolux) : Token;
        }

        public static string GenerateToken(bool isElectrolux = false)
        {
            LoginResponse result = null;
            string serviceCall = string.Format("{0}/Account/Login", ConfigurationManager.AppSettings["APIUrl"]);
            Login loginModel = new Login()
            {
                ClientId = ConfigurationManager.AppSettings["ClientId"],
                Password = isElectrolux ? ConfigurationManager.AppSettings["Electrolux_xCBL_Password"] : ConfigurationManager.AppSettings["Password"],
                Username = isElectrolux ? ConfigurationManager.AppSettings["Electrolux_xCBL_Username"] : ConfigurationManager.AppSettings["Username"]
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceCall);
            request.KeepAlive = false;
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string loginModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
                streamWriter.Write(loginModelJson);
            }

            WebResponse response = request.GetResponse();

            using (Stream tokenResponseStream = response.GetResponseStream())
            {
                using (TextReader tokenReader = new StreamReader(tokenResponseStream))
                {
                    string tokenResponseString = tokenReader.ReadToEnd();

                    using (var stringReader = new StringReader(tokenResponseString))
                    {
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(tokenResponseString);
                    }
                }
            }

            if (result.Status && result.Results != null && result.Results.Count > 0)
            {
                ExpirationTime = result.Results[0].ExpirationTime;
                Token = string.Format("{0} {1}", result.Results[0].token_type, result.Results[0].access_token);
            }

            return Token;
        }
    }
}