using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderResponse
{
	[XmlRoot(ElementName = "OrderResponse")]
	public class OrderResponseResult
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