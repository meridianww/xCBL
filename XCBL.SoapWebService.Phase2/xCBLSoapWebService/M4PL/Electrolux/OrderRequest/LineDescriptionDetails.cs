using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "lineDescriptionDetails")]
	public class LineDescriptionDetails
	{
		[XmlElement(ElementName = "lineDescription")]
		public LineDescription LineDescription { get; set; }
	}
}