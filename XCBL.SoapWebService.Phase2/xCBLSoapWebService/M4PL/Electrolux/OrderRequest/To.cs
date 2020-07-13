using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "to")]
	public class To
	{
		[XmlElement(ElementName = "orgID")]
		public string OrgID { get; set; }

		[XmlElement(ElementName = "locationID")]
		public string LocationID { get; set; }
	}
}