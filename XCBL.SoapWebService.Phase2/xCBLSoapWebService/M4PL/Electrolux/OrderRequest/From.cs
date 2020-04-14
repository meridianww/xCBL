using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
    [XmlRoot(ElementName = "from")]
    public class From
    {
        [XmlElement(ElementName = "orgID")]
        public string OrgID { get; set; }
        [XmlElement(ElementName = "locationID")]
        public string LocationID { get; set; }
        [XmlElement(ElementName = "messageID")]
        public string MessageID { get; set; }
    }
}
