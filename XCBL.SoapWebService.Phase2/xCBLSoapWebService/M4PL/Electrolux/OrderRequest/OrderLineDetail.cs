using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
    [XmlRoot(ElementName = "OrderLineDetail")]
    public class OrderLineDetail
    {
        [XmlElement(ElementName = "lineNumber")]
        public string LineNumber { get; set; }
        [XmlElement(ElementName = "ItemID")]
        public string ItemID { get; set; }
        [XmlElement(ElementName = "ItemDescription")]
        public string ItemDescription { get; set; }
        [XmlElement(ElementName = "shipQuantity")]
        public int ShipQuantity { get; set; }
        [XmlElement(ElementName = "weight")]
        public decimal Weight { get; set; }
        [XmlElement(ElementName = "weightUnitOfMeasure")]
        public string WeightUnitOfMeasure { get; set; }
        [XmlElement(ElementName = "volume")]
        public string Volume { get; set; }
        [XmlElement(ElementName = "volumeUnitOfMeasure")]
        public string VolumeUnitOfMeasure { get; set; }
        [XmlElement(ElementName = "secondaryLocation")]
        public string SecondaryLocation { get; set; }
        [XmlElement(ElementName = "materialType")]
        public string MaterialType { get; set; }
        [XmlElement(ElementName = "shipUnitOfMeasure")]
        public string ShipUnitOfMeasure { get; set; }
        [XmlElement(ElementName = "customerStockNumber")]
        public string CustomerStockNumber { get; set; }
        [XmlElement(ElementName = "statusCode")]
        public string StatusCode { get; set; }
        [XmlElement(ElementName = "EDILINEID")]
        public string EDILINEID { get; set; }
        [XmlElement(ElementName = "materialTypeDescription")]
        public string MaterialTypeDescription { get; set; }
        [XmlElement(ElementName = "LineNumberReference")]
        public string LineNumberReference { get; set; }
        [XmlElement(ElementName = "lineDescriptionDetails")]
        public LineDescriptionDetails LineDescriptionDetails { get; set; }
    }
}
