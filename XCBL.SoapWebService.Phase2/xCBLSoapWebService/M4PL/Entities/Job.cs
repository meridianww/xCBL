using System;

namespace xCBLSoapWebService.M4PL.Entities
{
    public class Job
    {
        /// <summary>
        /// Gets or sets the job master identifier.
        /// </summary>
        /// <value>
        /// The JobMITJob identifier.
        /// </value>
        public long? JobMITJobID { get; set; }

        /// <summary>
        /// Gets or sets the program identifier.
        /// </summary>
        /// <value>
        /// The Program identifier.
        /// </value>
        public long? ProgramID { get; set; }

        public string ProgramIDName { get; set; }

        /// <summary>
        /// Gets or sets the type of job.
        /// </summary>
        /// <value>
        /// The JobSiteCode.
        /// </value>
        public string JobSiteCode { get; set; }

        /// <summary>
        /// Gets or sets the consignee code.
        /// </summary>
        /// <value>
        /// The JobConsigneeCode.
        /// </value>
        public string JobConsigneeCode { get; set; }

        /// <summary>
        /// Gets or sets the sales order.
        /// </summary>
        /// <value>
        /// The JobCustomerSalesOrder.
        /// </value>
        public string JobCustomerSalesOrder { get; set; }

        /// <summary>
        /// Gets or sets the .
        /// </summary>
        /// <value>
        /// The JobBOLMaster.
        /// </value>
        public string JobBOLMaster { get; set; }

        /// <summary>
        /// Gets or sets the .
        /// </summary>
        /// <value>
        /// The JobBOLChild.
        /// </value>
        public string JobBOLChild { get; set; }

        /// <summary>
        /// Gets or sets the purchase order.
        /// </summary>
        /// <value>
        /// The JobCustomerPurchaseOrder.
        /// </value>
        public string JobCustomerPurchaseOrder { get; set; }

        /// <summary>
        /// Gets or sets the carrier contract.
        /// </summary>
        /// <value>
        /// The JobCarrierContract.
        /// </value>
        public string JobCarrierContract { get; set; }

        /// <summary>
        /// Gets or sets the  status.
        /// </summary>
        /// <value>
        /// The GatewayStatusId.
        /// </value>
        public string JobGatewayStatus { get; set; }

        /// <summary>
        /// Gets or sets the status date.
        /// </summary>
        /// <value>
        /// The JobStatusedDate.
        /// </value>
        public DateTime? JobStatusedDate { get; set; }

        /// <summary>
        /// Gets or sets the job completion.
        /// </summary>
        /// <value>
        /// The JobCompleted.
        /// </value>
        public bool JobCompleted { get; set; }

        /// <summary>
        /// Gets or sets the job Type.
        /// </summary>
        /// <value>
        /// The JobType.
        /// </value>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the shipment Type.
        /// </summary>
        /// <value>
        /// The ShipmentType.
        /// </value>
        public string ShipmentType { get; set; }

        /// <summary>
        /// Gets or sets the job delivery Analyst contact for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryAnalystContactID identifier.
        /// </value>
        public long? JobDeliveryAnalystContactID { get; set; }


        /// <summary>
        /// Gets or sets the job delivery Analyst contact fullname for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryAnalystContactIDName identifier.
        /// </value>

        public string JobDeliveryAnalystContactIDName { get; set; }


        /// <summary>
        /// Gets or sets the job delivery responsible contact for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryResponsibleContact identifier.
        /// </value>
        public long? JobDeliveryResponsibleContactID { get; set; }

        public string JobDeliveryResponsibleContactIDName { get; set; }

        /// <summary>
        /// Gets or sets the job delivery site poc for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliverySitePOC.
        /// </value>
        public string JobDeliverySitePOC { get; set; }

        /// <summary>
        /// Gets or sets the .
        /// </summary>
        /// <value>
        /// The JobDeliverySitePOCPhone for job delivery.
        /// </value>
        public string JobDeliverySitePOCPhone { get; set; }

        /// <summary>
        /// Gets or sets the job delivery poc email for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliverySitePOCEmail.
        /// </value>
        public string JobDeliverySitePOCEmail { get; set; }

        /// <summary>
        /// Gets or sets the .
        /// </summary>
        /// <value>
        /// The JobDeliverySiteName.
        /// </value>
        public string JobDeliverySiteName { get; set; }

        /// <summary>
        /// Gets or sets  job delivery the street address for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryStreetAddress.
        /// </value>
        public string JobDeliveryStreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier street address2 for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryStreetAddress2.
        /// </value>
        public string JobDeliveryStreetAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the delivery city for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryCity.
        /// </value>
        public string JobDeliveryCity { get; set; }

        /// <summary>
        /// Gets or sets the state province fo job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryStateProvince.
        /// </value>
        public string JobDeliveryState { get; set; }


        /// <summary>
        /// Gets or sets the postal code for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryPostalCode.
        /// </value>
        public string JobDeliveryPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the delivery country.
        /// </summary>
        /// <value>
        /// The JobDeliveryCountry.
        /// </value>
        public string JobDeliveryCountry { get; set; }

        /// <summary>
        /// Gets or sets the timezone foe delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryTimeZone.
        /// </value>
        public string JobDeliveryTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the date and time planned for delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryDatePlanned.
        /// </value>
        public DateTime? JobDeliveryDateTimePlanned { get; set; }

        /// <summary>
        /// Gets or sets the date and time actual for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryDateActual.
        /// </value>
        public DateTime? JobDeliveryDateTimeActual { get; set; }

        /// <summary>
        /// Gets or sets the date and time  baseline time for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryDateBaseline.
        /// </value>
        public DateTime? JobDeliveryDateTimeBaseline { get; set; }

        /// <summary>
        /// Gets or sets the bsaeline time for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryComment.
        /// </value>
        public byte[] JobDeliveryComment { get; set; }

        /// <summary>
        /// Gets or sets the comment for job delivery
        /// </summary>
        /// <value>
        /// The JobDeliveryRecipientPhone.
        /// </value>
        public string JobDeliveryRecipientPhone { get; set; }

        /// <summary>
        /// Gets or sets the recipient email for job delivery.
        /// </summary>
        /// <value>
        /// The JobDeliveryRecipientEmail.
        /// </value>
        public string JobDeliveryRecipientEmail { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The JobLatitude.
        /// </value>
        public string JobLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The JobLongitude.
        /// </value>
        public string JobLongitude { get; set; }

        /// <summary>
        /// Gets or sets the responsible Contact identifier.
        /// </summary>
        /// <value>
        /// The JobOriginResponsibleContactID.
        /// </value>
        public long? JobOriginResponsibleContactID { get; set; }

        public string JobOriginResponsibleContactIDName { get; set; }

        /// <summary>
        /// Gets or sets the origin site Poc.
        /// </summary>
        /// <value>
        /// The JobOriginSitePOC.
        /// </value>
        public string JobOriginSitePOC { get; set; }

        /// <summary>
        /// Gets or sets the origin site poc phone.
        /// </summary>
        /// <value>
        /// The JobOriginSitePOCPhone.
        /// </value>
        public string JobOriginSitePOCPhone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The JobOriginSitePOCEmail.
        /// </value>
        public string JobOriginSitePOCEmail { get; set; }

        /// <summary>
        /// Gets or sets the site name.
        /// </summary>
        /// <value>
        /// The JobOriginSiteName.
        /// </value>
        public string JobOriginSiteName { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        /// <value>
        /// The JobOriginStreetAddress.
        /// </value>
        public string JobOriginStreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the street address2.
        /// </summary>
        /// <value>
        /// The JobOriginStreetAddress2.
        /// </value>
        public string JobOriginStreetAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The JobOriginCity.
        /// </value>
        public string JobOriginCity { get; set; }

        /// <summary>
        /// Gets or sets the state province.
        /// </summary>
        /// <value>
        /// The JobOriginStateProvince.
        /// </value>
        public string JobOriginState { get; set; }


        /// <summary>
        /// Gets or sets the postalcode.
        /// </summary>
        /// <value>
        /// The JobOriginPostalCode.
        /// </value>
        public string JobOriginPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The JobOriginCountry.
        /// </value>
        public string JobOriginCountry { get; set; }

        /// <summary>
        /// Gets or sets the timezone.
        /// </summary>
        /// <value>
        /// The JobOriginTimeZone.
        /// </value>
        public string JobOriginTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the date and time planned.
        /// </summary>
        /// <value>
        /// The JobOriginDatePlanned.
        /// </value>
        public DateTime? JobOriginDateTimePlanned { get; set; }

        /// <summary>
        /// Gets or sets the date and time actual.
        /// </summary>
        /// <value>
        /// The JobOriginDateActual.
        /// </value>
        public DateTime? JobOriginDateTimeActual { get; set; }

        /// <summary>
        /// Gets or sets the Date and time Baseline.
        /// </summary>
        /// <value>
        /// The JobOriginDateBaseline.
        /// </value>
        public DateTime? JobOriginDateTimeBaseline { get; set; }

        /// <summary>
        /// Gets or sets the ProcessingFlags.
        /// </summary>
        /// <value>
        /// The JobProcessingFlags.
        /// </value>
        public string JobProcessingFlags { get; set; }

        public string JobBOL { get; set; }

        public string JobDeliverySitePOC2 { get; set; }
        public string JobDeliverySitePOCPhone2 { get; set; }
        public string JobDeliverySitePOCEmail2 { get; set; }
        public string JobOriginSitePOC2 { get; set; }
        public string JobOriginSitePOCPhone2 { get; set; }
        public string JobOriginSitePOCEmail2 { get; set; }
        public string JobSellerCode { get; set; }
        public string JobSellerSitePOC { get; set; }
        public string JobSellerSitePOCPhone { get; set; }
        public string JobSellerSitePOCEmail { get; set; }
        public string JobSellerSitePOC2 { get; set; }
        public string JobSellerSitePOCPhone2 { get; set; }
        public string JobSellerSitePOCEmail2 { get; set; }
        public string JobSellerSiteName { get; set; }
        public string JobSellerStreetAddress { get; set; }
        public string JobSellerStreetAddress2 { get; set; }
        public string JobSellerCity { get; set; }
        public string JobSellerState { get; set; }
        public string JobSellerPostalCode { get; set; }
        public string JobSellerCountry { get; set; }
        public string JobUser01 { get; set; }
        public string JobUser02 { get; set; }
        public string JobUser03 { get; set; }
        public string JobUser04 { get; set; }
        public string JobUser05 { get; set; }
        public string JobStatusFlags { get; set; }
        public string JobScannerFlags { get; set; }
        public string JobManifestNo { get; set; }



        public string PlantIDCode { get; set; }

        public string CarrierID { get; set; }

        public long? JobDriverId { get; set; }
        public string JobDriverIdName { get; set; }
        public DateTime? WindowDelStartTime { get; set; }
        public DateTime? WindowDelEndTime { get; set; }
        public DateTime? WindowPckStartTime { get; set; }
        public DateTime? WindowPckEndTime { get; set; }
        public int? JobRouteId { get; set; }
        public string JobStop { get; set; }
        public string JobSignText { get; set; }
        public string JobSignLatitude { get; set; }
        public string JobSignLongitude { get; set; }
        public decimal? JobQtyOrdered { get; set; }
        public int? JobQtyActual { get; set; }
        public int? JobQtyUnitTypeId { get; set; }
        public string JobQtyUnitTypeIdName { get; set; }
        public int? JobPartsOrdered { get; set; }
        public int? JobPartsActual { get; set; }
        public decimal? JobTotalCubes { get; set; }
        public string JobServiceMode { get; set; }
        public string JobChannel { get; set; }
        public string JobProductType { get; set; }
        public string JobSONumber { get; set; }
        public string JobPONumber { get; set; }

        public decimal? PckEarliest { get; set; }
        public decimal? PckLatest { get; set; }
        public bool PckDay { get; set; }
        public decimal? DelEarliest { get; set; }
        public decimal? DelLatest { get; set; }
        public bool DelDay { get; set; }
        public DateTime? ProgramPickupDefault { get; set; }
        public DateTime? ProgramDeliveryDefault { get; set; }
        public DateTime? JobOrderedDate { get; set; }
        public DateTime? JobShipmentDate { get; set; }
        public DateTime? JobInvoicedDate { get; set; }
        public string JobShipFromSiteName { get; set; }
        public string JobShipFromStreetAddress { get; set; }
        public string JobShipFromStreetAddress2 { get; set; }
        public string JobShipFromCity { get; set; }
        public string JobShipFromState { get; set; }
        public string JobShipFromPostalCode { get; set; }
        public string JobShipFromCountry { get; set; }
        public string JobShipFromSitePOC { get; set; }
        public string JobShipFromSitePOCPhone { get; set; }
        public string JobShipFromSitePOCEmail { get; set; }
        public string JobShipFromSitePOC2 { get; set; }
        public string JobShipFromSitePOCPhone2 { get; set; }
        public string JobShipFromSitePOCEmail2 { get; set; }

        public string CustomerERPId { get; set; }
        public string VendorERPId { get; set; }

        public bool JobElectronicInvoice { get; set; }
        public string JobOriginStreetAddress3 { get; set; }
        public string JobOriginStreetAddress4 { get; set; }
        public string JobDeliveryStreetAddress3 { get; set; }
        public string JobDeliveryStreetAddress4 { get; set; }
        public string JobSellerStreetAddress3 { get; set; }
        public string JobSellerStreetAddress4 { get; set; }
        public string JobShipFromStreetAddress3 { get; set; }
        public string JobShipFromStreetAddress4 { get; set; }

        public int? JobCubesUnitTypeId { get; set; }
        public string JobCubesUnitTypeIdName { get; set; }
        public int? JobWeightUnitTypeId { get; set; }
        public string JobWeightUnitTypeIdName { get; set; }
        public decimal JobTotalWeight { get; set; }
        public string JobElectronicInvoiceSONumber { get; set; }
        public string JobElectronicInvoicePONumber { get; set; }
        public int? JobPreferredMethod { get; set; }
        public string JobPreferredMethodName { get; set; }
        public decimal JobMileage { get; set; }

        public int? StatusId { get; set; }
        public long Id { get; set; }
        public bool JobIsSchedule { get; set; }
    }
}