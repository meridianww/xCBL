using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "Order")]
	public class Order
	{
		[XmlElement(ElementName = "OrderHeader")]
		public OrderHeader OrderHeader { get; set; }

		[XmlElement(ElementName = "OrderLineDetailList")]
		public OrderLineDetailList OrderLineDetailList { get; set; }

		[XmlElement(ElementName = "OrderDescriptionList")]
		public OrderDescriptionList OrderDescriptionList { get; set; }
	}
}