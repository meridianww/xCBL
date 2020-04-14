using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
    [XmlRoot(ElementName = "fxEnvelope")]
    public class ElectroluxOrderDetails
    {
        [XmlElement(ElementName = "header")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "body")]
        public Body Body { get; set; }
    }
}
