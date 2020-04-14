using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
    [XmlRoot(ElementName = "OrderDescription")]
    public class OrderDescription
    {
        [XmlElement(ElementName = "DescriptionText")]
        public string DescriptionText { get; set; }
        [XmlElement(ElementName = "pickTicketIndicator")]
        public string PickTicketIndicator { get; set; }
        [XmlElement(ElementName = "billOfLadingIndicator")]
        public string BillOfLadingIndicator { get; set; }
    }
}
