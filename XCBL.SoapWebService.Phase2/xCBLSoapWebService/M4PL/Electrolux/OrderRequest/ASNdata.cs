using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "ASNdata")]
	public class ASNdata
	{
		[XmlElement(ElementName = "Scac")]
		public string Scac { get; set; }
		[XmlElement(ElementName = "VehicleId")]
		public string VehicleId { get; set; }
		[XmlElement(ElementName = "BolNumber")]
		public string BolNumber { get; set; }
		[XmlElement(ElementName = "Shipdate")]
		public string Shipdate { get; set; }
		[XmlElement(ElementName = "ETAtime")]
		public string ETAtime { get; set; }
		[XmlElement(ElementName = "ETAdate")]
		public string ETAdate { get; set; }
	}
}
