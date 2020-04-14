using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
    [XmlRoot(ElementName = "OrderHeader")]
    public class OrderHeader
    {
        [XmlElement(ElementName = "senderID")]
        public string SenderID { get; set; }
        [XmlElement(ElementName = "recieverID")]
        public string RecieverID { get; set; }
        [XmlElement(ElementName = "originalOrderNumber")]
        public string OriginalOrderNumber { get; set; }
        [XmlElement(ElementName = "orderNumber")]
        public string OrderNumber { get; set; }
        [XmlElement(ElementName = "ReleaseNum")]
        public string ReleaseNum { get; set; }
        [XmlElement(ElementName = "orderType")]
        public string OrderType { get; set; }
        [XmlElement(ElementName = "orderDate")]
        public string OrderDate { get; set; }
        [XmlElement(ElementName = "customerPO")]
        public string CustomerPO { get; set; }
        [XmlElement(ElementName = "purchaseOrderType")]
        public string PurchaseOrderType { get; set; }
        [XmlElement(ElementName = "cosigneePO")]
        public string CosigneePO { get; set; }
        [XmlElement(ElementName = "deliveryDate")]
        public string DeliveryDate { get; set; }
        [XmlElement(ElementName = "deliveryTime")]
        public string DeliveryTime { get; set; }
        [XmlElement(ElementName = "RMAIndicator")]
        public string RMAIndicator { get; set; }
        [XmlElement(ElementName = "departmentNumber")]
        public string DepartmentNumber { get; set; }
        [XmlElement(ElementName = "freightCarrierCode")]
        public string FreightCarrierCode { get; set; }
        [XmlElement(ElementName = "HotOrder")]
        public string HotOrder { get; set; }
        [XmlElement(ElementName = "ShipFrom")]
        public ShipFrom ShipFrom { get; set; }
        [XmlElement(ElementName = "ShipTo")]
        public ShipTo ShipTo { get; set; }
        [XmlElement(ElementName = "DeliverTo")]
        public DeliverTo DeliverTo { get; set; }
    }
}
