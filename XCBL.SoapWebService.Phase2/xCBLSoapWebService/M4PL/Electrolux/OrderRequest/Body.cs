using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
    [XmlRoot(ElementName = "body")]
    public class Body
    {
        [XmlElement(ElementName = "Order")]
        public Order Order { get; set; }
    }
}
