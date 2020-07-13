using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "lineDescription")]
	public class LineDescription
	{
		[XmlElement(ElementName = "lineNumber")]
		public string LineNumber { get; set; }

		[XmlElement(ElementName = "lineText")]
		public string LineText { get; set; }

		[XmlElement(ElementName = "pickTicketIndicator")]
		public string PickTicketIndicator { get; set; }

		[XmlElement(ElementName = "billOfLadingIndicator")]
		public string BillOfLadingIndicator { get; set; }
	}
}