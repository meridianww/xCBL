using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "ShipFrom")]
	public class ShipFrom
	{
		[XmlElement(ElementName = "LocationID")]
		public string LocationID { get; set; }

		[XmlElement(ElementName = "LocationName")]
		public string LocationName { get; set; }

		[XmlElement(ElementName = "contactFirstName")]
		public string ContactFirstName { get; set; }

		[XmlElement(ElementName = "contactLastName")]
		public string ContactLastName { get; set; }

		[XmlElement(ElementName = "contactEmailID")]
		public string ContactEmailID { get; set; }

		[XmlElement(ElementName = "addressLine1")]
		public string AddressLine1 { get; set; }

		[XmlElement(ElementName = "addressLine2")]
		public string AddressLine2 { get; set; }

		[XmlElement(ElementName = "addressLine3")]
		public string AddressLine3 { get; set; }

		[XmlElement(ElementName = "city")]
		public string City { get; set; }

		[XmlElement(ElementName = "state")]
		public string State { get; set; }

		[XmlElement(ElementName = "zipCode")]
		public string ZipCode { get; set; }

		[XmlElement(ElementName = "country")]
		public string Country { get; set; }

		[XmlElement(ElementName = "contactNumber")]
		public string ContactNumber { get; set; }
	}
}