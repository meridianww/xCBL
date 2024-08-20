using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using xCBLSoapWebService.M4PL;

namespace xCBLSoapWebService
{
	public class ProcessRequisition
	{
		private MeridianResult _meridianResult = null;

		#region Requisition Request

		/// <summary>
		/// Method to pass xCBL XML data to the web serivce
		/// </summary>
		/// <param name="currentOperationContext">Operation context inside this XmlElement the xCBL XML data to parse</param>
		/// <returns>XElement - XML Message Acknowledgement response indicating Success or Failure</returns>
		internal MeridianResult ProcessRequisitionDocument(OperationContext currentOperationContext)
		{
			_meridianResult = new MeridianResult();
			_meridianResult.IsSchedule = false;
			_meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_SUCCESS;
			XCBL_User xCblServiceUser = new XCBL_User();
            bool isRejected = false;
            MeridianSystemLibrary.LogTransaction("No WebUser", "No FTPUser", "ProcessRequisitionDocument", "01.01", "Success - New SOAP Request Received", "Requisition Document Process", "No FileName", "No Requisition ID", "No Order Number", null, "Success");
			if (CommonProcess.IsAuthenticatedRequest(currentOperationContext, ref xCblServiceUser))
			{
				MeridianSystemLibrary.LogTransaction(xCblServiceUser.WebUsername, xCblServiceUser.FtpUsername, "IsAuthenticatedRequest", "01.02", "Success - Authenticated request", "Requisition Document Process", "No FileName", "No Requisition ID", "No Order Number", null, "Success");
                ProcessData processData = ProcessRequisitionRequestAndCreateFiles(currentOperationContext, xCblServiceUser, out bool isCSVCreationRejected);

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableXCBLShippingScheduleForAWCToSyncWithM4PL"])) // Check if IsRejected required for Requisition
                {
                    var response = M4PL.M4PLService.CallM4PLAPI<List<long>>(new XCBLToM4PLRequest() { EntityId = (int)XCBLRequestType.Requisition, Request = processData.Requisition }, "XCBL/XCBLSummaryHeader");
                }

                if (processData == null || string.IsNullOrEmpty(processData.RequisitionID) || string.IsNullOrEmpty(processData.OrderNumber))
					_meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_FAILURE;
				else
				{
					processData.FtpUserName = xCblServiceUser.FtpUsername;
					processData.FtpPassword = xCblServiceUser.FtpPassword;
					processData.FtpServerInFolderPath = xCblServiceUser.FtpServerInFolderPath;
					processData.FtpServerOutFolderPath = xCblServiceUser.FtpServerOutFolderPath;
					processData.LocalFilePath = xCblServiceUser.LocalFilePath;
					_meridianResult.WebUserName = xCblServiceUser.WebUsername;
					_meridianResult.WebPassword = xCblServiceUser.WebPassword;
					_meridianResult.WebHashKey = xCblServiceUser.Hashkey;

					if (!isCSVCreationRejected)
					{
						if (!CreateLocalCsvFile(processData))
							_meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_FAILURE;
					}
					else
					{
						_meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_SUCCESS;
					}
					_meridianResult.UniqueID = processData.RequisitionID;
					return _meridianResult;
				}
			}
			else
			{
				_meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_FAILURE;
				MeridianSystemLibrary.LogTransaction("No WebUser", "No FTPUser", "IsAuthenticatedRequest", "03.01", "Error - New SOAP Request not authenticated", "UnAuthenticated Request", "No FileName", "No Requisition ID", "No Order Number", null, "Error 03.01 - Invalid Credentials");
			}
			return _meridianResult;
		}

		#region TODO: Need to remove, added to test AWC requisition request failure

		/// <summary>
		/// Method to pass xCBL XML data to the web serivce
		/// </summary>
		/// <param name="currentOperationContext">Operation context inside this XmlElement the xCBL XML data to parse</param>
		/// <returns>XElement - XML Message Acknowledgement response indicating Success or Failure</returns>
		internal MeridianResult ProcessRequisitionDocumentAWCTest(OperationContext currentOperationContext)
		{
			_meridianResult = new MeridianResult();
			_meridianResult.IsSchedule = false;
			_meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_SUCCESS;
			XCBL_User xCblServiceUser = new XCBL_User();

			xCblServiceUser.WebUsername = "AWCTest";

			MeridianSystemLibrary.LogTransaction("AWCTest", "No FTPUser", "ProcessRequisitionDocument", "01.01", "Success - New SOAP Request Received", "Requisition Document Process", "No FileName", "No Requisition ID", "No Order Number", null, "Success");
			//MeridianSystemLibrary.LogTransaction(xCblServiceUser.WebUsername, xCblServiceUser.FtpUsername, "IsAuthenticatedRequest", "01.02", "Success - Authenticated request", "Requisition Document Process", "No FileName", "No Requisition ID", "No Order Number", null, "Success");

			var requestMessage = currentOperationContext.RequestContext.RequestMessage.ToString().ReplaceSpecialCharsWithSpace(false);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(requestMessage);

			XmlNamespaceManager xmlNsManager = new XmlNamespaceManager(xmlDoc.NameTable);
			xmlNsManager.AddNamespace("default", "rrn:org.xcbl:schemas/xcbl/v4_0/applicationintegration/v1_0/applicationintegration.xsd");
			xmlNsManager.AddNamespace("core", "rrn:org.xcbl:schemas/xcbl/v4_0/core/core.xsd");

			XmlNodeList requisitionElement = xmlDoc.GetElementsByTagName(MeridianGlobalConstants.XCBL_REQUISITION_HEADER);

			//Find the Requisition tag and getting the Inner Xml of its Node
			XmlNodeList requisitionNode_xml = xmlDoc.GetElementsByTagName(MeridianGlobalConstants.XCBL_Requisition_XML_Http);//Http Request creating this tag
			if (requisitionNode_xml.Count == 0)
			{
				requisitionNode_xml = xmlDoc.GetElementsByTagName(MeridianGlobalConstants.XCBL_Requisition_XML_Https);//Https Request creating this tag
			}

			MeridianSystemLibrary.LogTransaction(xCblServiceUser.WebUsername, xCblServiceUser.FtpUsername,
				"ProcessRequisitionDocumentAWCTest", "01.03",
				"Success - Logged ProcessRequisitionDocumentAWCTest Request",
				"Requisition Document Process", "", /*processData.RequisitionID*/"", /*processData.OrderNumber*/"", /*processData.XmlDocument*/xmlDoc, "Success");

			return _meridianResult;
		}

		#endregion TODO: Need to remove, added to test AWC requisition request failure

		/// <summary>
		/// To Process request and create csv and xml files.
		/// </summary>
		/// <param name="operationContext">Current OperationContext</param>
		/// <returns></returns>
		private ProcessData ProcessRequisitionRequestAndCreateFiles(OperationContext operationContext, XCBL_User xCblServiceUser, out bool checkIsCSVCreationRejected)
		{
			checkIsCSVCreationRejected = false;
			try
			{
				ProcessData processData = ValidateRequisitionXmlDocument(operationContext.RequestContext, xCblServiceUser);
				if (processData != null && !string.IsNullOrEmpty(processData.RequisitionID)
					&& !string.IsNullOrEmpty(processData.OrderNumber)
				   && !string.IsNullOrEmpty(processData.CsvFileName))
				{
					if (MeridianGlobalConstants.CONFIG_TransitDirectionCodedOtherATDCAAIsRejectCSV.Equals(MeridianGlobalConstants.XCBL_YES_FLAG, StringComparison.OrdinalIgnoreCase))
					{
						bool transitDirectionCodedOtherFlag = string.IsNullOrEmpty(processData.Requisition.TransitDirectionCodedOther) ? true
							: (processData.Requisition.TransitDirectionCodedOther.ToUpper() == "ATD" || processData.Requisition.TransitDirectionCodedOther.ToUpper() == "CAA") ? false
							: true;

						if (!transitDirectionCodedOtherFlag)
						{
							checkIsCSVCreationRejected = true;
							MeridianSystemLibrary.LogTransaction(processData.WebUserName, processData.FtpUserName, "ProcessRequestAndCreateFiles", "02.27",
								"Reject - TransitDirectionCodedOther ATD/CAA Parsed requested xml for CSV file", string.Format("Reject - Past Due Date got for Order '{0}' Parsed requested xml for CSV file",
								processData.OrderNumber), null, processData.ScheduleID, processData.OrderNumber, processData.XmlDocument, "Reject 02.27");

							return processData;
						}
					}

					if (!string.IsNullOrEmpty(processData.Requisition.RequestedShipByDate) && !Convert.ToDateTime(processData.Requisition.RequestedShipByDate).VerifyDatetimeExpaire())
					{
						checkIsCSVCreationRejected = true;
						MeridianSystemLibrary.LogTransaction(processData.WebUserName, processData.FtpUserName, "ProcessRequestAndCreateFiles", "02.26",
					   "Reject - Past Due Date Parsed requested xml for CSV file", string.Format("Reject - Past Due Date got for Order '{0}' Parsed requested xml for CSV file", processData.OrderNumber),
					   null, processData.ScheduleID, processData.OrderNumber, processData.XmlDocument, "Reject 02.26");
					}
					if (!checkIsCSVCreationRejected)
						MeridianSystemLibrary.LogTransaction(xCblServiceUser.WebUsername, xCblServiceUser.FtpUsername, "ProcessRequestAndCreateFiles", "01.03", string.Format("Success - Parsed requested xml for CSV file {0}", processData.RequisitionID), "Requisition Document Process", processData.CsvFileName, processData.RequisitionID, processData.OrderNumber, processData.XmlDocument, "Success");

					checkIsCSVCreationRejected = true;
					return processData;
				}
			}
			catch (Exception ex)
			{
				MeridianSystemLibrary.LogTransaction(xCblServiceUser.WebUsername, xCblServiceUser.FtpUsername, "ValidateRequisitionXmlDocument", "03.02", "Error - Incorrect request ", string.Format("Exception - Invalid request xml {0}", ex.Message), "No file Name", "No Requisition Id", "No Order Number", null, "Error 03.02 - Invalid request xml");
			}

			return new ProcessData();
		}

		/// <summary>
		/// To Parse sent SOAP XML and make list of Process data
		/// </summary>
		/// <param name="requestContext"> Current OperationContext's RequestContext</param>
		/// <param name="xCblServiceUser">Service User</param>
		/// <returns>List of process data</returns>
		private ProcessData ValidateRequisitionXmlDocument(RequestContext requestContext, XCBL_User xCblServiceUser)
		{
			var requestMessage = requestContext.RequestMessage.ToString().ReplaceSpecialCharsWithSpace(false);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(requestMessage);

			XmlNamespaceManager xmlNsManager = new XmlNamespaceManager(xmlDoc.NameTable);
			xmlNsManager.AddNamespace("default", "rrn:org.xcbl:schemas/xcbl/v4_0/applicationintegration/v1_0/applicationintegration.xsd");
			xmlNsManager.AddNamespace("core", "rrn:org.xcbl:schemas/xcbl/v4_0/core/core.xsd");

			XmlNodeList requisitionElement = xmlDoc.GetElementsByTagName(MeridianGlobalConstants.XCBL_REQUISITION_HEADER);

			//Find the Requisition tag and getting the Inner Xml of its Node
			XmlNodeList requisitionNode_xml = xmlDoc.GetElementsByTagName(MeridianGlobalConstants.XCBL_Requisition_XML_Http);//Http Request creating this tag
			if (requisitionNode_xml.Count == 0)
			{
				requisitionNode_xml = xmlDoc.GetElementsByTagName(MeridianGlobalConstants.XCBL_Requisition_XML_Https);//Https Request creating this tag
			}

			if (requisitionElement != null)
			{
				// There should only be one element in the Requisition request, but this should handle multiple ones
				foreach (XmlNode element in requisitionElement)
				{
					var processData = xCblServiceUser.GetNewProcessData();
					processData.XmlDocument = xmlDoc;
					_meridianResult.XmlDocument = xmlDoc;

					var requisitionId = element.GetNodeByNameAndLogErrorTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_NUMBER, "03", processData, processData.RequisitionID, "ValidateRequisitionXmlDocument");
					var requisitionIssuedDate = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_ISSUE_DATE, "01", processData, processData.RequisitionID, "ValidateRequisitionXmlDocument");

					//Requisition Header Information --start
					if (requisitionId != null && !string.IsNullOrEmpty(requisitionId.InnerText))
					{
						processData.RequisitionID = requisitionId.InnerText.ReplaceSpecialCharsWithSpace();
						processData.Requisition.ReqNumber = processData.RequisitionID;

						if (requisitionIssuedDate != null && !string.IsNullOrEmpty(requisitionIssuedDate.InnerText))
							processData.Requisition.RequisitionIssueDate = requisitionIssuedDate.InnerText.ReplaceSpecialCharsWithSpace();

						if (string.IsNullOrEmpty(processData.Requisition.ReqNumber))
							break;
						else
						{
							GetRequisitionTypes(xmlNsManager, element, processData);
							GetOtherRequisitionReferences(xmlNsManager, element, processData);
							GetPurposes(xmlNsManager, element, processData);
							GetRequestedShipByDate(xmlNsManager, element, processData);
							GetRequisitionParty(xmlNsManager, element, processData);
							GetListOfTransportRouting(xmlNsManager, element, processData);
							return processData;
						}
					}
				}
			}
			else
				MeridianSystemLibrary.LogTransaction(xCblServiceUser.WebUsername, xCblServiceUser.FtpUsername, "ValidateRequisitionXmlDocument", "03.02", "Error - Requisition Header XML tag missing or incorrect", "Exception - Invalid request xml", "No file Name", "No Requisition Number", "No Order Number", xmlDoc, "Error 03.02 - Invalid request xml");
			return new ProcessData();
		}

		/// <summary>
		/// To create CSV file
		/// </summary>
		/// <param name="processData">Process data</param>
		/// <returns></returns>
		private bool CreateLocalCsvFile(ProcessData processData)
		{
			bool result = false;
			try
			{
				if (processData != null && !string.IsNullOrEmpty(processData.RequisitionID)
					 && !string.IsNullOrEmpty(processData.OrderNumber)
					&& !string.IsNullOrEmpty(processData.CsvFileName))
				{
					var record = string.Format(MeridianGlobalConstants.REQUISITION_CSV_HEADER_NAMES_FORMAT,
						processData.Requisition.ReqNumber, processData.Requisition.RequisitionIssueDate,
						processData.Requisition.RequisitionTypeCoded, processData.Requisition.RequisitionTypeCodedOther,
						processData.Requisition.Other_WorkOrder_RefNum, processData.Requisition.Other_OriginalOrder_RefNum,
						processData.Requisition.Other_Cabinets_RefNum, processData.Requisition.Other_Parts_RefNum,
						processData.Requisition.Other_NewOrderNumber_RefNum, processData.Requisition.Other_BOL_RefNum,
						processData.Requisition.Other_Manifest_RefNum, processData.Requisition.Other_RequisitionType_RefNum,
						processData.Requisition.Other_Domicile_RefNum, processData.Requisition.PurposeCoded,
						processData.Requisition.PurposeCodedOther, processData.Requisition.RequestedShipByDate,
						"", processData.Requisition.ShipToParty_Name1, processData.Requisition.ShipToParty_Street,
						processData.Requisition.ShipToParty_StreetSupplement1, processData.Requisition.ShipToParty_PostalCode,
						processData.Requisition.ShipToParty_City, processData.Requisition.ShipToParty_RegionCoded,
						"", processData.Requisition.ShipFromParty_Name1, processData.Requisition.ShipFromParty_Street,
						processData.Requisition.ShipFromParty_StreetSupplement1, processData.Requisition.ShipFromParty_PostalCode,
						processData.Requisition.ShipFromParty_City, processData.Requisition.ShipFromParty_RegionCoded,
						processData.Requisition.QuantityValue_Cabinets, processData.Requisition.UOMCoded_Cabinets,
						processData.Requisition.UOMCodedOther_Cabinets, processData.Requisition.QuantityQualifierCoded_Cabinets,
						processData.Requisition.QuantityQualifierCodedOther_Cabinets, processData.Requisition.QuantityValue_Parts,
						processData.Requisition.UOMCoded_Parts, processData.Requisition.UOMCodedOther_Parts,
						processData.Requisition.QuantityQualifierCoded_Parts, processData.Requisition.QuantityQualifierCodedOther_Parts,
						processData.Requisition.ShippingInstructions, processData.Requisition.TransitDirectionCoded,
						processData.Requisition.TransitDirectionCodedOther, processData.Requisition.StartTransportLocation_GPSSystem,
						processData.Requisition.StartTransportLocation_Latitude, processData.Requisition.StartTransportLocation_Longitude,
						processData.Requisition.StartTransportLocation_LocationID, processData.Requisition.EndTransportLocation_GPSSystem,
						processData.Requisition.EndTransportLocation_Latitude, processData.Requisition.EndTransportLocation_Longitude,
						processData.Requisition.EndTransportLocation_LocationID, processData.Requisition.Other_WorkOrder_RefNum.ExtractNumericOrderNumber());

					StringBuilder strBuilder = new StringBuilder(MeridianGlobalConstants.REQUISITION_CSV_HEADER_NAMES);
					strBuilder.AppendLine();
					strBuilder.AppendLine(record);
					string csvContent = strBuilder.ToString();

					_meridianResult.FtpUserName = processData.FtpUserName;
					_meridianResult.FtpPassword = processData.FtpPassword;
					_meridianResult.FtpServerInFolderPath = processData.FtpServerInFolderPath;
					_meridianResult.FtpServerOutFolderPath = processData.FtpServerOutFolderPath;
					_meridianResult.LocalFilePath = processData.LocalFilePath;
					_meridianResult.WebUserName = processData.WebUserName;
					_meridianResult.UniqueID = processData.RequisitionID;
					_meridianResult.OrderNumber = processData.OrderNumber;
					_meridianResult.FileName = processData.CsvFileName;

					//if (MeridianGlobalConstants.CONFIG_CREATE_LOCAL_CSV == MeridianGlobalConstants.SHOULD_CREATE_LOCAL_FILE)
					//{
					//	if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableXCBLRequisitionForAWCToSyncWithM4PL"]))
					//	{
					//		var response = M4PL.M4PLService.CallM4PLAPI<List<long>>(new XCBLToM4PLRequest() { EntityId = (int)XCBLRequestType.Requisition, Request = processData.Requisition }, "XCBL/XCBLSummaryHeader");
					//	}
					//	_meridianResult.UploadFromLocalPath = true;
					//	return CommonProcess.CreateFile(csvContent, _meridianResult);
					//}
					//else
					//{
						byte[] content = Encoding.UTF8.GetBytes(csvContent);
						int length = content.Length;

						if (!string.IsNullOrEmpty(processData.CsvFileName) && length > 40)
						{
							_meridianResult.Content = content;
							result = true;
						}
						else
						{
							MeridianSystemLibrary.LogTransaction(processData.WebUserName, processData.FtpUserName, "CreateLocalCsvFile", "03.06", ("Error - Creating CSV File because of Stream " + length), string.Format("Error - Creating CSV File {0} with error of Stream", processData.CsvFileName), processData.CsvFileName, processData.RequisitionID, processData.OrderNumber, processData.XmlDocument, "Error 03.06 - Create CSV");
						}
					//}
				}
				else
				{
					MeridianSystemLibrary.LogTransaction(processData.WebUserName, processData.FtpUserName, "CreateLocalCsvFile", "03.06", "Error - Creating CSV File because of Process DATA", string.Format("Error - Creating CSV File {0} with error of Process DATA", processData.CsvFileName), processData.CsvFileName, processData.RequisitionID, processData.OrderNumber, processData.XmlDocument, "Error 03.06 - Create CSV");
				}
			}
			catch (Exception ex)
			{
				MeridianSystemLibrary.LogTransaction(processData.WebUserName, processData.FtpUserName, "CreateLocalCsvFile", "03.06", "Error - Creating CSV File", string.Format("Error - Creating CSV File {0} with error {1}", processData.CsvFileName, ex.Message), processData.CsvFileName, processData.RequisitionID, processData.OrderNumber, processData.XmlDocument, "Error 03.06 - Create CSV");
			}

			return result;
		}

		#region XML Parsing

		private void GetRequisitionTypes(XmlNamespaceManager xmlNsManager, XmlNode element, ProcessData processData)
		{
			XmlNode requisitionType = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_TYPE, "02", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_TYPES);

			if (requisitionType != null)
			{
				XmlNode requisitionTypeCoded = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_TYPE_CODED, "03", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_TYPES);
				if (requisitionTypeCoded != null)
					processData.Requisition.RequisitionTypeCoded = requisitionTypeCoded.InnerText.ReplaceSpecialCharsWithSpace();
				XmlNode requisitionTypeCodedOther = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_TYPE_CODED_OTHER, "04", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_TYPES);
				if (requisitionTypeCodedOther != null)
					processData.Requisition.RequisitionTypeCodedOther = requisitionTypeCodedOther.InnerText.ReplaceSpecialCharsWithSpace();
			}
		}

		private void GetPurposes(XmlNamespaceManager xmlNsManager, XmlNode element, ProcessData processData)
		{
			XmlNode purposeCoded = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_PURPOSE_CODED, "06", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_PURPOSES);
			if (purposeCoded != null)
				processData.Requisition.PurposeCoded = purposeCoded.InnerText.ReplaceSpecialCharsWithSpace();
			XmlNode purposeCodedOther = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_PURPOSE_CODED_OTHER, "07", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_PURPOSES);
			if (purposeCodedOther != null)
				processData.Requisition.PurposeCodedOther = purposeCodedOther.InnerText.ReplaceSpecialCharsWithSpace();
		}

		private void GetRequestedShipByDate(XmlNamespaceManager xmlNsManager, XmlNode element, ProcessData processData)
		{
			XmlNode requisitionDate = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_DATES, "08", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUESTED_SHIP_BY_DATE);

			if (requisitionDate != null)
			{
				XmlNode requestedShipByDate = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUESTED_SHIP_BY_DATE, "09", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUESTED_SHIP_BY_DATE);
				if (requestedShipByDate != null)
					processData.Requisition.RequestedShipByDate = requestedShipByDate.InnerText.ReplaceSpecialCharsWithSpace();
			}
		}

		private void GetRequisitionParty(XmlNamespaceManager xmlNsManager, XmlNode element, ProcessData processData)
		{
			XmlNode requisitionParty = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_PARTY, "10", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
			if (requisitionParty != null)
			{
				XmlNode shipToParty = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY, "11", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
				if (shipToParty != null)
				{
					XmlNode requestedShipToPartyNameAddress = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS, "12", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
					if (requestedShipToPartyNameAddress != null)
					{
						XmlNode requestedShipToPartyNameAddressName1 = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS_NAME1, "13", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipToPartyNameAddressName1 != null)
							processData.Requisition.ShipToParty_Name1 = requestedShipToPartyNameAddressName1.InnerText.Replace(",", "").ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipToPartyNameAddressStreet = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS_STREET, "14", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipToPartyNameAddressStreet != null)
							processData.Requisition.ShipToParty_Street = requestedShipToPartyNameAddressStreet.InnerText.Replace(",", "").ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipToPartyNameAddressStreetSpplement = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS_STREET_SUPPLEMENT1, "15", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipToPartyNameAddressStreetSpplement != null)
							processData.Requisition.ShipToParty_StreetSupplement1 = requestedShipToPartyNameAddressStreetSpplement.InnerText.Replace(",", "").ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipToPartyNameAddressPostalCode = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS_POSTAL_CODE, "16", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipToPartyNameAddressPostalCode != null)
							processData.Requisition.ShipToParty_PostalCode = requestedShipToPartyNameAddressPostalCode.InnerText.ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipToPartyNameAddressCity = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS_CITY, "17", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipToPartyNameAddressCity != null)
							processData.Requisition.ShipToParty_City = requestedShipToPartyNameAddressCity.InnerText.ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipToPartyNameAddressRegion = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS_REGION, "18", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipToPartyNameAddressRegion != null)
						{
							XmlNode requestedShipToPartyNameAddressRegionCoded = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_TO_PARTY_NAME_ADDRESS_REGION_REGIONCODED, "19", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
							if (requestedShipToPartyNameAddressRegionCoded != null)
								processData.Requisition.ShipToParty_RegionCoded = requestedShipToPartyNameAddressRegionCoded.InnerText.ReplaceSpecialCharsWithSpace();
						}
					}
				}

				XmlNode shipFromParty = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY, "20", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
				if (shipFromParty != null)
				{
					XmlNode requestedShipFromPartyNameAddress = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS, "21", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
					if (requestedShipFromPartyNameAddress != null)
					{
						XmlNode requestedShipFromPartyNameAddressName1 = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS_NAME1, "22", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipFromPartyNameAddressName1 != null)
							processData.Requisition.ShipFromParty_Name1 = requestedShipFromPartyNameAddressName1.InnerText.Replace(",", "").ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipFromPartyNameAddressStreet = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS_STREET, "23", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipFromPartyNameAddressStreet != null)
							processData.Requisition.ShipFromParty_Street = requestedShipFromPartyNameAddressStreet.InnerText.Replace(",", "").ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipFromPartyNameAddressStreetSpplement = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS_STREET_SUPPLEMENT1, "24", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipFromPartyNameAddressStreetSpplement != null)
							processData.Requisition.ShipFromParty_StreetSupplement1 = requestedShipFromPartyNameAddressStreetSpplement.InnerText.Replace(",", "").ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipFromPartyNameAddressPostalCode = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS_POSTAL_CODE, "25", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipFromPartyNameAddressPostalCode != null)
							processData.Requisition.ShipFromParty_PostalCode = requestedShipFromPartyNameAddressPostalCode.InnerText.ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipFromPartyNameAddressCity = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS_CITY, "26", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipFromPartyNameAddressCity != null)
							processData.Requisition.ShipFromParty_City = requestedShipFromPartyNameAddressCity.InnerText.ReplaceSpecialCharsWithSpace();

						XmlNode requestedShipFromPartyNameAddressRegion = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS_REGION, "27", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
						if (requestedShipFromPartyNameAddressRegion != null)
						{
							XmlNode requestedShipFromPartyNameAddressRegionCoded = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_REQUISITION_SHIP_FROM_PARTY_NAME_ADDRESS_REGION_REGIONCODED, "28", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_REQUISITION_PARTY);
							if (requestedShipFromPartyNameAddressRegionCoded != null)
								processData.Requisition.ShipFromParty_RegionCoded = requestedShipFromPartyNameAddressRegionCoded.InnerText.ReplaceSpecialCharsWithSpace();
						}
					}
				}
			}
		}

		private void GetListOfTransportRouting(XmlNamespaceManager xmlNsManager, XmlNode element, ProcessData processData)
		{
			XmlNode shippingInstruction = element.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_SHIPPING_INSTRUCTIONS, "29", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
			if (shippingInstruction != null)
				processData.Requisition.ShippingInstructions = shippingInstruction.InnerText.Replace(",", "").ReplaceSpecialCharsWithSpace();

			XmlNode listOfTransportQuantities = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_TRANSPORT_QUANTITIES, "30", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
			if (listOfTransportQuantities != null)
			{
				XmlNode xnlistOfQuantityCoded = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_LIST_OF_QUANTITY_CODED, "31", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
				if (xnlistOfQuantityCoded != null)
				{
					XmlNodeList xnQuantityCoded = xnlistOfQuantityCoded.ChildNodes; // 2 Nodes Always
					for (int iQuantityCodedIndex = 0; iQuantityCodedIndex < xnQuantityCoded.Count; iQuantityCodedIndex++)
					{
						XmlNodeList xnCurrentQuantityCodedChildren = xnQuantityCoded[iQuantityCodedIndex].ChildNodes;

						if ((xnCurrentQuantityCodedChildren.Count == 4)
							&& (xnCurrentQuantityCodedChildren[0].Name.Trim().Equals(string.Format(MeridianGlobalConstants.TAG_PREFIXED_WITH_CORE, MeridianGlobalConstants.XCBL_QUANTITYVALUE), StringComparison.OrdinalIgnoreCase))
							&& (xnCurrentQuantityCodedChildren[1].Name.Trim().Equals(string.Format(MeridianGlobalConstants.TAG_PREFIXED_WITH_CORE, MeridianGlobalConstants.XCBL_UNITOFMEASUREMENT), StringComparison.OrdinalIgnoreCase))
							&& (xnCurrentQuantityCodedChildren[2].Name.Trim().Equals(string.Format(MeridianGlobalConstants.TAG_PREFIXED_WITH_CORE, MeridianGlobalConstants.XCBL_QUANTITYQUALIFIERCODED), StringComparison.OrdinalIgnoreCase))
							&& (xnCurrentQuantityCodedChildren[3].Name.Trim().Equals(string.Format(MeridianGlobalConstants.TAG_PREFIXED_WITH_CORE, MeridianGlobalConstants.XCBL_QUANTITYQUALIFIERCODEDOTHER), StringComparison.OrdinalIgnoreCase))
							)
						{
							var currentQuantityValue = xnCurrentQuantityCodedChildren[0].InnerText.ReplaceSpecialCharsWithSpace();
							XmlNodeList xnUnitOfMeasurementChildren = xnCurrentQuantityCodedChildren[1].ChildNodes;
							string uomCoded = null;
							string uomCodedOther = null;
							if ((xnUnitOfMeasurementChildren != null) && (xnUnitOfMeasurementChildren.Count > 0))
							{
								uomCoded = xnUnitOfMeasurementChildren[0].InnerText.ReplaceSpecialCharsWithSpace();
								if (xnUnitOfMeasurementChildren.Count > 1)
									uomCodedOther = xnUnitOfMeasurementChildren[1].InnerText.ReplaceSpecialCharsWithSpace();
							}
							var currentQuantityQualifierCoded = xnCurrentQuantityCodedChildren[2].InnerText.ReplaceSpecialCharsWithSpace();
							var currentQuantityQualifierCodedOther = xnCurrentQuantityCodedChildren[3].InnerText.ReplaceSpecialCharsWithSpace();

							if (!string.IsNullOrWhiteSpace(currentQuantityQualifierCodedOther))
							{
								switch (currentQuantityQualifierCodedOther.ToLower())
								{
									case MeridianGlobalConstants.XCBL_QUANTITY_QUALIFIER_CABINETS:
										processData.Requisition.QuantityValue_Cabinets = currentQuantityValue;
										processData.Requisition.UOMCoded_Cabinets = uomCoded;
										processData.Requisition.UOMCodedOther_Cabinets = uomCodedOther;
										processData.Requisition.QuantityQualifierCoded_Cabinets = currentQuantityQualifierCoded;
										processData.Requisition.QuantityQualifierCodedOther_Cabinets = currentQuantityQualifierCodedOther;
										break;

									case MeridianGlobalConstants.XCBL_QUANTITY_QUALIFIER_PARTS:
										processData.Requisition.QuantityValue_Parts = currentQuantityValue;
										processData.Requisition.UOMCoded_Parts = uomCoded;
										processData.Requisition.UOMCodedOther_Parts = uomCodedOther;
										processData.Requisition.QuantityQualifierCoded_Parts = currentQuantityQualifierCoded;
										processData.Requisition.QuantityQualifierCodedOther_Parts = currentQuantityQualifierCodedOther;

										break;
								}
							}
						}
					}
				}
			}

			XmlNode xnTransitDirection = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_TRANSIT_DIRECTION, "32", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
			if (xnTransitDirection != null)
			{
				XmlNode xnTransitDirectionCoded = xnTransitDirection.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_TRANSIT_DIRECTION_CODED, "33", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
				if (xnTransitDirectionCoded != null)
					processData.Requisition.TransitDirectionCoded = xnTransitDirectionCoded.InnerText.ReplaceSpecialCharsWithSpace();

				XmlNode xnTransitDirectionCodedOther = xnTransitDirection.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_TRANSIT_DIRECTION_CODED_OTHER, "34", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
				if (xnTransitDirectionCodedOther != null)
					processData.Requisition.TransitDirectionCodedOther = xnTransitDirectionCodedOther.InnerText.ReplaceSpecialCharsWithSpace();
			}

			XmlNode xnTransportLocationList = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_TRANSPORT_LOCATION_LIST, "35", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
			if (xnTransportLocationList != null)
			{
				XmlNode xnStartTransport = xnTransportLocationList.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_START_TRANSPORT, "36", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
				if (xnStartTransport != null)
				{
					XmlNode xnLocation = xnStartTransport.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_START_TRANSPORT_LOCATION, "37", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
					if (xnLocation != null)
					{
						XmlNode xnStartGPSCoordinates = xnLocation.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_START_TRANSPORT_LOCATION_GPS_COORDINATES, "38", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
						if (xnStartGPSCoordinates != null)
						{
							XmlNode xnGPSSystem = xnStartGPSCoordinates.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_START_TRANSPORT_LOCATION_GPS_COORDINATES_GPS_SYSTEM, "39", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
							if (xnGPSSystem != null)
								processData.Requisition.StartTransportLocation_GPSSystem = xnGPSSystem.InnerText.ReplaceSpecialCharsWithSpace();

							XmlNode xnLatitude = xnStartGPSCoordinates.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_START_TRANSPORT_LOCATION_GPS_COORDINATES_LATITUDE, "40", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
							if (xnLatitude != null)
								processData.Requisition.StartTransportLocation_Latitude = xnLatitude.InnerText.ReplaceSpecialCharsWithSpace();

							XmlNode xnLongitude = xnStartGPSCoordinates.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_START_TRANSPORT_LOCATION_GPS_COORDINATES_LONGITUDE, "41", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
							if (xnLongitude != null)
								processData.Requisition.StartTransportLocation_Longitude = xnLongitude.InnerText.ReplaceSpecialCharsWithSpace();
						}
					}

					XmlNode xnStartLocationID = xnStartTransport.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_START_TRANSPORT_LOCATION_ID, "42", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
					if (xnStartLocationID != null)
						processData.Requisition.StartTransportLocation_LocationID = xnStartLocationID.InnerText.ReplaceSpecialCharsWithSpace();
				}

				XmlNode xnEndTransport = xnTransportLocationList.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_END_TRANSPORT, "43", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
				if (xnEndTransport != null)
				{
					XmlNode xnLocation = xnEndTransport.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_END_TRANSPORT_LOCATION, "44", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
					if (xnLocation != null)
					{
						XmlNode xnEndGPSCoordinates = xnLocation.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_END_TRANSPORT_LOCATION_GPS_COORDINATES, "45", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
						if (xnEndGPSCoordinates != null)
						{
							XmlNode xnGPSSystem = xnEndGPSCoordinates.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_END_TRANSPORT_LOCATION_GPS_COORDINATES_GPS_SYSTEM, "46", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
							if (xnGPSSystem != null)
								processData.Requisition.EndTransportLocation_GPSSystem = xnGPSSystem.InnerText.ReplaceSpecialCharsWithSpace();

							XmlNode xnLatitude = xnEndGPSCoordinates.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_END_TRANSPORT_LOCATION_GPS_COORDINATES_LATITUDE, "47", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
							if (xnLatitude != null)
								processData.Requisition.EndTransportLocation_Latitude = xnLatitude.InnerText.ReplaceSpecialCharsWithSpace();

							XmlNode xnLongitude = xnEndGPSCoordinates.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_END_TRANSPORT_LOCATION_GPS_COORDINATES_LONGITUDE, "48", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
							if (xnLongitude != null)
								processData.Requisition.EndTransportLocation_Longitude = xnLongitude.InnerText.ReplaceSpecialCharsWithSpace();
						}
					}

					XmlNode xnEndLocationID = xnEndTransport.GetNodeByNameAndInnerTextLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_END_TRANSPORT_LOCATION_ID, "49", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_LIST_OF_TRANSPORT_ROUTING);
					if (xnEndLocationID != null)
						processData.Requisition.EndTransportLocation_LocationID = xnEndLocationID.InnerText.ReplaceSpecialCharsWithSpace();
				}
			}
		}

		private void GetOtherRequisitionReferences(XmlNamespaceManager xmlNsManager, XmlNode element, ProcessData processData)
		{
			XmlNode xnOtherRequisitionReferences = element.GetNodeByNameAndLogWarningTrans(xmlNsManager, MeridianGlobalConstants.XCBL_OTHER_REQUISITION_REFERENCES, "05", processData, processData.RequisitionID, MeridianGlobalConstants.METHOD_GET_OTHER_REQUISITION_REFERENCE);
			if (xnOtherRequisitionReferences != null)
			{
				XmlNodeList xnReferenceCoded = xnOtherRequisitionReferences.ChildNodes; // 9 Nodes Always
				for (int iReferenceCodedIndex = 0; iReferenceCodedIndex < xnReferenceCoded.Count; iReferenceCodedIndex++)
				{
					XmlNodeList xnCurrentReferenceCodedChildren = xnReferenceCoded[iReferenceCodedIndex].ChildNodes;
					if ((xnCurrentReferenceCodedChildren.Count == 3)
						&& (xnCurrentReferenceCodedChildren[1].Name.Trim().Equals(string.Format(MeridianGlobalConstants.TAG_PREFIXED_WITH_CORE, MeridianGlobalConstants.XCBL_REFERENCE_TYPECODE_OTHER), StringComparison.OrdinalIgnoreCase))
						&& (xnCurrentReferenceCodedChildren[2].Name.Trim().Equals(string.Format(MeridianGlobalConstants.TAG_PREFIXED_WITH_CORE, MeridianGlobalConstants.XCBL_PRIMARY_REFERENCE), StringComparison.OrdinalIgnoreCase)))
					{
						var currentReferenceTypeCodedOther = xnCurrentReferenceCodedChildren[1].InnerText.ReplaceSpecialCharsWithSpace();
						XmlNodeList xnPrimaryReferenceChildren = xnCurrentReferenceCodedChildren[2].ChildNodes;
						string xnRefNumber = null;
						string xnRefDate = null;
						if ((xnPrimaryReferenceChildren != null) && (xnPrimaryReferenceChildren.Count > 0))
						{
							xnRefNumber = xnPrimaryReferenceChildren[0].InnerText.ReplaceSpecialCharsWithSpace();
							if (xnPrimaryReferenceChildren.Count > 1)
								xnRefDate = xnPrimaryReferenceChildren[1].InnerText.ReplaceSpecialCharsWithSpace();
						}

						if (!string.IsNullOrWhiteSpace(currentReferenceTypeCodedOther) && !string.IsNullOrWhiteSpace(xnRefNumber))
						{
							switch (currentReferenceTypeCodedOther.Replace(" ", "").ToLower())
							{
								case MeridianGlobalConstants.XCBL_WORK_ORDER:
									processData.Requisition.Other_WorkOrder_RefNum = xnRefNumber;
									if (xnRefDate != null)
										processData.Requisition.Other_WorkOrder_RefDate = xnRefDate;
									break;

								case MeridianGlobalConstants.XCBL_ORIGINAL_ORDER:
									processData.Requisition.Other_OriginalOrder_RefNum = xnRefNumber;
									if (xnRefDate != null)
										processData.Requisition.Other_OriginalOrder_RefDate = xnRefDate;
									break;

								case MeridianGlobalConstants.XCBL_CABINETS:
									processData.Requisition.Other_Cabinets_RefNum = xnRefNumber;
									break;

								case MeridianGlobalConstants.XCBL_PARTS:
									processData.Requisition.Other_Parts_RefNum = xnRefNumber;
									break;

								case MeridianGlobalConstants.XCBL_BOL:
									processData.Requisition.Other_BOL_RefNum = xnRefNumber;
									break;

								case MeridianGlobalConstants.XCBL_MANIFEST:
									processData.Requisition.Other_Manifest_RefNum = xnRefNumber;
									break;

								case MeridianGlobalConstants.XCBL_NEW_ORDER_NUMBER:
									processData.Requisition.Other_NewOrderNumber_RefNum = xnRefNumber;
									processData.OrderNumber = processData.Requisition.Other_NewOrderNumber_RefNum;
									//To Give Order Number and File Name
									var formattedNewOrder = xnRefNumber.Replace(" ", "");
									string fileNameFormat = DateTime.Now.ToString(MeridianGlobalConstants.XCBL_FILE_DATETIME_FORMAT);
									processData.CsvFileName = string.Concat(MeridianGlobalConstants.XCBL_AWC_REQUISITION_FILE_PREFIX, fileNameFormat, formattedNewOrder, MeridianGlobalConstants.XCBL_FILE_EXTENSION);
									processData.XmlFileName = string.Concat(MeridianGlobalConstants.XCBL_AWC_REQUISITION_FILE_PREFIX, fileNameFormat, formattedNewOrder, MeridianGlobalConstants.XCBL_XML_EXTENSION);
									break;

								case MeridianGlobalConstants.XCBL_REQUISTION_TYPE:
									processData.Requisition.Other_RequisitionType_RefNum = xnRefNumber;
									break;

								case MeridianGlobalConstants.XCBL_DOMICILE:
									processData.Requisition.Other_Domicile_RefNum = xnRefNumber;
									break;
							}
						}
					}
				}
			}
		}

		#endregion XML Parsing

		#endregion Requisition Request
	}
}