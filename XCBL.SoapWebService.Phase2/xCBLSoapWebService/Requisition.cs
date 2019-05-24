using System.Collections.Generic;

namespace xCBLSoapWebService
{
    public class Requisition
    {
        public string ReqNumber { get; set; }
        public string RequisitionIssueDate { get; set; }
        public string RequisitionTypeCoded { get; set; }
        public string RequisitionTypeCodedOther { get; set; }

        public string Other_WorkOrder_RefNum { get; set; }
        public string Other_WorkOrder_RefDate { get; set; }
        public string Other_OriginalOrder_RefNum { get; set; }
        public string Other_OriginalOrder_RefDate { get; set; }
        public string Other_Cabinets_RefNum { get; set; }
        public string Other_Parts_RefNum { get; set; }
        public string Other_BOL_RefNum { get; set; }
        public string Other_Manifest_RefNum { get; set; }
        public string Other_NewOrderNumber_RefNum { get; set; }
        public string Other_RequisitionType_RefNum { get; set; }
        public string Other_Domicile_RefNum { get; set; }

        public string PurposeCoded { get; set; }
        public string PurposeCodedOther { get; set; }
        public string RequestedShipByDate { get; set; }

        public string ShipToParty_Name1 { get; set; }
        public string ShipToParty_Street { get; set; }
        public string ShipToParty_StreetSupplement1 { get; set; }
        public string ShipToParty_PostalCode { get; set; }
        public string ShipToParty_City { get; set; }
        public string ShipToParty_RegionCoded { get; set; }

        public string ShipFromParty_Name1 { get; set; }
        public string ShipFromParty_Street { get; set; }
        public string ShipFromParty_StreetSupplement1 { get; set; }
        public string ShipFromParty_PostalCode { get; set; }
        public string ShipFromParty_City { get; set; }
        public string ShipFromParty_RegionCoded { get; set; }

        public string QuantityValue_Cabinets { get; set; }
        public string UOMCoded_Cabinets { get; set; }
        public string UOMCodedOther_Cabinets { get; set; }
        public string QuantityQualifierCoded_Cabinets { get; set; }
        public string QuantityQualifierCodedOther_Cabinets { get; set; }

        public string QuantityValue_Parts { get; set; }
        public string UOMCoded_Parts { get; set; }
        public string UOMCodedOther_Parts { get; set; }
        public string QuantityQualifierCoded_Parts { get; set; }
        public string QuantityQualifierCodedOther_Parts { get; set; }

        public string ShippingInstructions { get; set; }
        public string TransitDirectionCoded { get; set; }
        public string TransitDirectionCodedOther { get; set; }

        public string StartTransportLocation_GPSSystem { get; set; }
        public string StartTransportLocation_Latitude { get; set; }
        public string StartTransportLocation_Longitude { get; set; }
        public string StartTransportLocation_LocationID { get; set; }

        public string EndTransportLocation_GPSSystem { get; set; }
        public string EndTransportLocation_Latitude { get; set; }
        public string EndTransportLocation_Longitude { get; set; }
        public string EndTransportLocation_LocationID { get; set; }
    }

}