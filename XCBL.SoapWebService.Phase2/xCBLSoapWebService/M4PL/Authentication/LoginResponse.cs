using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Web;

namespace xCBLSoapWebService.M4PL.Authentication
{
    public class LoginResponse
    {
        public bool Status { get; set; }
        public List<Result> Results { get; set; }
        public int TotalResults { get; set; }
        public int ReturnedResults { get; set; }
    }

    public class Result
    {
        [JsonProperty(".expires")]
        public DateTime ExpirationTime { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string userName { get; set; }
        public string systemMessage { get; set; }
    }
}