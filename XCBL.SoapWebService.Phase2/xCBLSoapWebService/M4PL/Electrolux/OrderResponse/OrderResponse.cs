using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace M4PL.Entities.XCBL.Electrolux.OrderResponse
{
	[XmlRoot(ElementName = "OrderResponse")]
	public class OrderResponse
	{
		[XmlElement(ElementName = "subject")]
		public string Subject { get; set; }
		[XmlElement(ElementName = "clientMessageID")]
		public string ClientMessageID { get; set; }
		[XmlElement(ElementName = "senderMessageID")]
		public string SenderMessageID { get; set; }
		[XmlElement(ElementName = "statusCode")]
		public string StatusCode { get; set; }
	}
}
