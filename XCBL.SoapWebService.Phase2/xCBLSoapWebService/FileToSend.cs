using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace xCBLSoapWebService
{
    public class FileToSend
    {
        public string FtpUserName { get; set; }
        public string FtpPassword { get; internal set; }
        public string FtpServerUrl { get; set; }
        public string WebUserName { get; set; }
        public string ScheduleID { get; set; }
        public string RequisitionID { get; set; }
        public string OrderNumber { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }

    }
}