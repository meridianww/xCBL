using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xCBLSoapWebService.M4PL.Entities
{
    public class API_Job_Response
    {
        public bool Status { get; set; }
        public List<Job> Results { get; set; }
        public int TotalResults { get; set; }
        public int ReturnedResults { get; set; }
    }
}