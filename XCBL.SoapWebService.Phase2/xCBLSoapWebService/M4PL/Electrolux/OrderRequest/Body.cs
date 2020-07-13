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