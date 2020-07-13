using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "OrderDescriptionList")]
	public class OrderDescriptionList
	{
		[XmlElement(ElementName = "OrderDescription")]
		public OrderDescription OrderDescription { get; set; }
	}
}