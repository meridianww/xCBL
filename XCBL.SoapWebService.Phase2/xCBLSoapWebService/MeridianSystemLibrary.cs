//Copyright (2016) Meridian Worldwide Transportation Group
//All Rights Reserved Worldwide
//====================================================================================================================================================
//Program Title:                                Meridian xCBL Web Service - AWC Timberlake
//Programmer:                                   Nathan Fujimoto
//Date Programmed:                              1/8/2016
//Program Name:                                 Meridian xCBL Web Service
//Purpose:                                      The module contains Meridian System Library Methods for Database calls to SYST010MeridianXCBLService
//                                              The XCBL_User class is an object to store all the authentication information for the user and to assist in transaction logging
//
//==================================================================================================================================================== 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;

namespace xCBLSoapWebService
{
    public static partial class MeridianSystemLibrary
    {
        #region Meridian SYST000XCBLService Database Methods

        /// <summary>
        /// This function will insert a record into the MER010TransactionLog table on the Meridian Development Server.
        /// </summary>
        /// <param name="scheduleId">string - Current ScheduleId</param>
        /// <param name="orderNumber">string - Current Order Number</param>
        /// <param name="approve01">string - Approve Flag 01</param>
        /// <param name="approve02">string - Approve Flag 02</param>
        /// <param name="approve03">string - Approve Flag 03</param>
        /// <param name="approve04">string - Approve Flag 04</param>
        /// <param name="approve05">string - Approve Flag 05</param>
        /// <param name="pending01">string - Pending Flag 01</param>
        /// <param name="pending02">string - Pending Flag 02</param>
        /// <param name="pending03">string - Pending Flag 03</param>
        /// <param name="pending04">string - Pending Flag 04</param>
        /// <param name="pending05">string - Pending Flag 05</param>
        /// <param name="requestType">string - Current Request Type</param>
        public static int LogPBS(string scheduleId, string orderNumber, string approve01, string approve02, string approve03, string approve04, string approve05, string pending01, string pending02, string pending03, string pending04, string pending05, string requestType, string rejected01, string comment)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(MeridianGlobalConstants.XCBL_DATABASE_SERVER_URL))
                {
                    sqlConnection.Open();

                    // Try to insert the record into the MER010TransactionLog table
                    using (SqlCommand sqlCommand = new SqlCommand(MeridianGlobalConstants.XCBL_SP_InsPBSLog, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add("@scheduleId", SqlDbType.NVarChar).Value = scheduleId;
                        sqlCommand.Parameters.Add("@orderNumber", SqlDbType.NVarChar).Value = orderNumber;
                        sqlCommand.Parameters.Add("@approve01", SqlDbType.NVarChar).Value = approve01;
                        sqlCommand.Parameters.Add("@approve02", SqlDbType.NVarChar).Value = approve02;
                        sqlCommand.Parameters.Add("@approve03", SqlDbType.NVarChar).Value = approve03;
                        sqlCommand.Parameters.Add("@approve04", SqlDbType.NVarChar).Value = approve04;
                        sqlCommand.Parameters.Add("@approve05", SqlDbType.NVarChar).Value = approve05;
                        sqlCommand.Parameters.Add("@pending01", SqlDbType.NVarChar).Value = pending01;
                        sqlCommand.Parameters.Add("@pending02", SqlDbType.NVarChar).Value = pending02;
                        sqlCommand.Parameters.Add("@pending03", SqlDbType.NVarChar).Value = pending03;
                        sqlCommand.Parameters.Add("@pending04", SqlDbType.NVarChar).Value = pending04;
                        sqlCommand.Parameters.Add("@pending05", SqlDbType.NVarChar).Value = pending05;
                        sqlCommand.Parameters.Add("@requestType", SqlDbType.NVarChar).Value = requestType;
                        sqlCommand.Parameters.Add("@rejected01", SqlDbType.NVarChar).Value = rejected01;
                        sqlCommand.Parameters.Add("@comment", SqlDbType.NVarChar).Value = comment;
                        return sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// This function will insert a record into the MER010TransactionLog table on the Meridian Development Server.
        /// </summary>
        /// <param name="webUser">string - The xCBL Web Service Username consuming the web service</param>
        /// <param name="ftpUser">string - The FTP Username currently assigned to the web username</param>
        /// <param name="methodName">string - The method name of where the transaction record is being called</param>
        /// <param name="messageNumber">string - The Message Number of the specific message to insert</param>
        /// <param name="messageDescription">string - The Message Description for the transaction record to provide more information or detail</param>
        /// <param name="microsoftDescription">string - The Exception Message supplied by Microsoft when an error is encountered</param>
        /// <param name="filename">string - The Filename of the xCBL file to upload</param>
        /// <param name="documentId">string - The Document ID assigned to the xCBL file</param>
        public static int LogTransaction(string webUser, string ftpUser, string methodName, string messageNumber, string messageDescription, string microsoftDescription, string filename, string documentId, string TranOrderNo, XmlDocument TranXMLData, string TranMessageCode, string TransPBSFile = null)
        {
            try
            {
                // 
                if (webUser == null) webUser = string.Empty;
                if (ftpUser == null) ftpUser = string.Empty;



                //Set up a new StringReader populated with the XmlDocument object's outer Xml

                XmlNodeReader srObject = TranXMLData == null ? null : new XmlNodeReader(TranXMLData);

                using (SqlConnection sqlConnection = new SqlConnection(MeridianGlobalConstants.XCBL_DATABASE_SERVER_URL))
                {
                    sqlConnection.Open();

                    // Try to insert the record into the MER010TransactionLog table
                    using (SqlCommand sqlCommand = new SqlCommand(MeridianGlobalConstants.XCBL_SP_InsTransactionLog, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add("@TransactionDate", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                        sqlCommand.Parameters.Add("@TransactionWebUser", SqlDbType.NVarChar).Value = webUser;
                        sqlCommand.Parameters.Add("@TransactionFtpUser", SqlDbType.NVarChar).Value = ftpUser;
                        sqlCommand.Parameters.Add("@TransactionMethodName", SqlDbType.NVarChar).Value = methodName;
                        sqlCommand.Parameters.Add("@TransactionMessageNumber", SqlDbType.NVarChar).Value = messageNumber;
                        sqlCommand.Parameters.Add("@TransactionMessageDescription", SqlDbType.NVarChar).Value = messageDescription;
                        sqlCommand.Parameters.Add("@TransactionMSDescription", SqlDbType.NVarChar).Value = microsoftDescription;
                        sqlCommand.Parameters.Add("@TransactionWebFilename", SqlDbType.NVarChar).Value = filename;
                        sqlCommand.Parameters.Add("@TransactionWebDocumentID", SqlDbType.NVarChar).Value = documentId;
                        sqlCommand.Parameters.Add("@TranOrderNo", SqlDbType.NVarChar).Value = TranOrderNo;
                        sqlCommand.Parameters.Add("@TranXMLData", SqlDbType.Xml).Value = srObject;
                        sqlCommand.Parameters.Add("@TranMessageCode", SqlDbType.NVarChar).Value = TranMessageCode;
                        sqlCommand.Parameters.Add("@TranPBSFile", SqlDbType.NVarChar).Value = TransPBSFile;
                        return sqlCommand.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        ///This function will retrieve the authentication information for a specified xCBL Web Service username and password found in MER000Authentication table if the user is enabled
        /// </summary>
        /// <param name="username">string - The username assigned to the xCBL web service credentials</param>
        /// <param name="password">string - The password assigned to the xCBL web service credentials</param>
        /// <returns>XCBL_User - XCBL_User class object that contains the authentication information for the record matching the username and password</returns>
        public static XCBL_User sysGetAuthenticationByUsernameAndPassword(string webUsername, string webPassword)
        {

            // If either the username or password are empty then return null for the method
            if (string.IsNullOrEmpty(webUsername) || string.IsNullOrEmpty(webPassword))
                return null;

            // Try to retrieve the authentication record based on the specified username and password
            try
            {
                DataSet dsRecords = new DataSet();

                using (SqlConnection sqlConnection = new SqlConnection(MeridianGlobalConstants.XCBL_DATABASE_SERVER_URL))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(MeridianGlobalConstants.XCBL_SP_GetXcblAuthenticationUser, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.Add("@webUsername", SqlDbType.NVarChar).Value = webUsername;
                        sqlCommand.Parameters.Add("@webPassword", SqlDbType.NVarChar).Value = webPassword;

                        // Fill the data adapter with the sql query results
                        using (SqlDataAdapter sdaAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            sdaAdapter.Fill(dsRecords);
                        }
                    }
                }
                // Parse the authentication record to a XCBL_User class object
                XCBL_User user = new XCBL_User()
                {
                    WebUsername = dsRecords.Tables[0].Rows[0].ItemArray[1].ToString(),
                    WebPassword = dsRecords.Tables[0].Rows[0].ItemArray[2].ToString(),
                    Hashkey = dsRecords.Tables[0].Rows[0].ItemArray[3].ToString(),
                    FtpUsername = dsRecords.Tables[0].Rows[0].ItemArray[4].ToString(),
                    FtpPassword = dsRecords.Tables[0].Rows[0].ItemArray[5].ToString(),
                    FtpServerInFolderPath = dsRecords.Tables[0].Rows[0].ItemArray[6].ToString(),
                    FtpServerOutFolderPath = dsRecords.Tables[0].Rows[0].ItemArray[7].ToString(),
                    LocalFilePath = dsRecords.Tables[0].Rows[0].ItemArray[8].ToString(),
                    WebContactName = dsRecords.Tables[0].Rows[0].ItemArray[9].ToString(),
                    WebContactCompany = dsRecords.Tables[0].Rows[0].ItemArray[10].ToString(),
                    WebContactEmail = dsRecords.Tables[0].Rows[0].ItemArray[11].ToString(),
                    WebContactPhone1 = dsRecords.Tables[0].Rows[0].ItemArray[12].ToString(),
                    WebContactPhone2 = dsRecords.Tables[0].Rows[0].ItemArray[13].ToString(),
                    Enabled = Boolean.Parse(dsRecords.Tables[0].Rows[0].ItemArray[14].ToString())
                };

                return user;
            }
            catch (Exception ex)
            {
                // If there was an error encountered in retrieving the authentication record then try to insert a record in MER010TransactionLog table to record the issue
                try
                {
                    LogTransaction(webUsername, "", "sysGetAuthenticationByUsername", "00.00", "Warning - Cannot retrieve record from MER000Authentication table", ex.InnerException.ToString(), "", "", "", new XmlDocument(), "Warning 26 - DB Connection");
                }
                catch
                {
                }
                return null;
            }
        }

        /// <summary>
        ///This function will retrieve the authentication information for a specified xCBL Web Service username and password found in MER000Authentication table if the user is enabled
        /// </summary>
        /// <param name="username">string - username assigned to the xCBL web service credentials</param>
        /// <param name="shippingScheduleId">string - shipping schedule Id</param>
        /// <param name="orderNumber">string - Order number relate to shipping schedule Id</param>
        /// <returns>XCBL_User - XCBL_User class object that contains the authentication information for the record matching the username and password</returns>
        public static string GetShippingScheduleRequest(string webUsername, string shippingScheduleId, string orderNumber)
        {

            // If either the username or shippingScheduleId or orderNumber are empty then return null for the method
            if (string.IsNullOrWhiteSpace(webUsername) || string.IsNullOrWhiteSpace(shippingScheduleId) || string.IsNullOrWhiteSpace(orderNumber))
                return null;

            // Try to retrieve the authentication record based on the specified username and password
            try
            {
                DataSet dsRecords = new DataSet();

                using (SqlConnection sqlConnection = new SqlConnection(MeridianGlobalConstants.XCBL_DATABASE_SERVER_URL))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(MeridianGlobalConstants.XCBL_SP_Get_Shipping_Schedule_Request, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.Add("@webUsername", SqlDbType.NVarChar).Value = webUsername;
                        sqlCommand.Parameters.Add("@shippingScheduleId", SqlDbType.NVarChar).Value = shippingScheduleId;
                        sqlCommand.Parameters.Add("@orderNumber", SqlDbType.NVarChar).Value = orderNumber;

                        // Fill the data adapter with the sql query results
                        using (SqlDataAdapter sdaAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            sdaAdapter.Fill(dsRecords);
                        }
                    }
                }

                if (dsRecords.Tables.Count > 0 && dsRecords.Tables[0].Rows.Count > 0)
                {
                    return dsRecords.Tables[0].Rows[0].ItemArray[1].ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                try
                {
                    LogTransaction(webUsername, "", "GetShippingScheduleRequest", "06.06", "Error - While retriving shipping schedule request from DB", ex.InnerException.ToString(), "", "", "", null, "Error 06.06 - Couldn't Retrieve SSR");
                }
                catch
                {
                }
                return string.Empty;
            }
        }

        #endregion

        public static ProcessData GetNewProcessData(this XCBL_User xCblServiceUser)
        {
            var processData = new ProcessData
            {
                ScheduleID = "No Schedule Id",
                RequisitionID = "No Requisition Id",
                ScheduleResponseID = "No Schedule Response Id",
                OrderNumber = "No Order Number",
                CsvFileName = "No FileName",
                XmlFileName = "No FileName",
                ShippingSchedule = new ShippingSchedule(),
                Requisition = new Requisition(),
                ShippingScheduleResponse = new ShippingScheduleResponse(),
                WebUserName = xCblServiceUser.WebUsername,
                FtpUserName = xCblServiceUser.FtpUsername
            };
            return processData;
        }

        /// <summary>
        /// This function will return the Success / Failure of the Action performed which tranferring the CSV file.
        /// </summary>
        /// <param name="status">string - Holds the Error Message - status</param>
        /// <param name="uniqueId">string - Unique id of the XCBL file which is to be uploaded(ScheduleId - For ShippingSchedule, RequisitionId - For Requisition Request)</param>
        /// <returns></returns>
        public static string GetMeridian_Status(string status, string uniqueId, bool isShippingSchedule = true, bool isPastDate = false)
        {
            StringBuilder messageResponse = new StringBuilder();
            messageResponse.AppendLine(MeridianGlobalConstants.XML_HEADER);
            messageResponse.AppendLine(isShippingSchedule ? MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_OPEN_TAG : MeridianGlobalConstants.MESSAGE_REQUISITION_ACKNOWLEDGEMENT_OPEN_TAG);
            messageResponse.AppendLine(string.Format(MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_REFERENCE_NUMBER_OPEN_TAG + "{0}" + MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_REFERENCE_NUMBER_CLOSE_TAG, uniqueId));
            if (isPastDate)
                messageResponse.AppendLine(string.Format(MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_COMMENT_OPEN_TAG + "{0}" + MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_COMMENT_CLOSE_TAG, MeridianGlobalConstants.XCBL_RESPONSE_TYPE_CODED_SHIPPING_SCHEDULE_RESPONSE_REJECTED));
            messageResponse.AppendLine(string.Format(MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_NOTE_OPEN_TAG + "{0}" + MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_NOTE_CLOSE_TAG, status));
            messageResponse.AppendLine(MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_CLOSE_TAG);
            return messageResponse.ToString();
        }

        public static XmlNode GetNodeByNameAndLogWarningTrans(this XmlNode fromNode, XmlNamespaceManager nsMgr, string nodeName, string warningNumber, ProcessData processData, string uniqueId, string methodName = "")
        {
            try
            {
                XmlNode foundNode = fromNode.SelectSingleNode(nodeName, nsMgr);
                if (foundNode == null)
                    LogTransaction(processData.WebUserName, processData.FtpUserName, !string.IsNullOrEmpty(methodName) ? methodName : "ValidateScheduleShippingXmlDocument", string.Format("02.{0}", warningNumber), string.Format("Warning - There was an exception retrieving {0} xml node or tag.", nodeName), string.Format("Warning - There was an exception retrieving {0} xml node or tag.", nodeName), processData.CsvFileName, uniqueId, processData.OrderNumber, processData.XmlDocument, string.Format("Warning {0} - Issue with node {1}.", "GetNodeByNameAndLogWarningTrans", nodeName));
                return foundNode;
            }
            catch (Exception e)
            {
                LogTransaction(processData.WebUserName, processData.FtpUserName, !string.IsNullOrEmpty(methodName) ? methodName : "ValidateScheduleShippingXmlDocument", string.Format("02.{0}", warningNumber), string.Format("Warning - There was an exception retrieving {0} xml node or tag.", nodeName), Convert.ToString(e.Message), processData.CsvFileName, uniqueId, processData.OrderNumber, processData.XmlDocument, string.Format("Warning {0} - Issue with node {1}.", "GetNodeByNameAndLogWarningTrans", nodeName));
                return null;
            }
        }

        public static XmlNode GetNodeByNameAndInnerTextLogWarningTrans(this XmlNode fromNode, XmlNamespaceManager nsMgr, string nodeName, string warningNumber, ProcessData processData, string uniqueId, string methodName = "")
        {
            try
            {
                XmlNode foundNode = fromNode.SelectSingleNode(nodeName, nsMgr);
                if (foundNode == null || string.IsNullOrEmpty(foundNode.InnerText))
                    LogTransaction(processData.WebUserName, processData.FtpUserName, !string.IsNullOrEmpty(methodName) ? methodName : "ValidateScheduleShippingXmlDocument", string.Format("02.{0}", warningNumber), string.Format("Warning - There was an exception retrieving {0} xml node or tag.", nodeName), string.Format("Warning - There was an exception retrieving {0} xml node or tag.", nodeName), processData.CsvFileName, uniqueId, processData.OrderNumber, processData.XmlDocument, string.Format("Warning {0} - Issue with node {1}", warningNumber, nodeName));
                return foundNode;
            }
            catch (Exception e)
            {
                LogTransaction(processData.WebUserName, processData.FtpUserName, !string.IsNullOrEmpty(methodName) ? methodName : "ValidateScheduleShippingXmlDocument", string.Format("02.{0}", warningNumber), string.Format("Warning - There was an exception retrieving {0} xml node or tag.", nodeName), Convert.ToString(e.Message), processData.CsvFileName, uniqueId, processData.OrderNumber, processData.XmlDocument, string.Format("Warning {0} - Issue with node {1}", warningNumber, nodeName));
                return null;
            }
        }

        public static XmlNode GetNodeByNameAndLogErrorTrans(this XmlNode fromNode, XmlNamespaceManager nsMgr, string nodeName, string errorNumber, ProcessData processData, string uniqueId, string methodName = "")
        {
            try
            {

                XmlNode foundNode = fromNode.SelectSingleNode(nodeName, nsMgr);//
                if (foundNode == null || string.IsNullOrEmpty(foundNode.InnerText))
                    LogTransaction(processData.WebUserName, processData.FtpUserName, !string.IsNullOrEmpty(methodName) ? methodName : "ValidateScheduleShippingXmlDocument", string.Format("03.{0}", errorNumber), string.Format("Error - There was an exception retrieving {0} xml node or tag or empty.", nodeName), string.Format("Error - There was an exception retrieving {0} xml node or tag or empty.", nodeName), processData.CsvFileName, uniqueId, processData.OrderNumber, processData.XmlDocument, string.Format("Error 03.{0} - Issue with node {1}", errorNumber, nodeName));
                return foundNode;
            }
            catch (Exception e)
            {
                LogTransaction(processData.WebUserName, processData.FtpUserName, !string.IsNullOrEmpty(methodName) ? methodName : "ValidateScheduleShippingXmlDocument", string.Format("03.{0}", errorNumber), string.Format("Error - There was an exception retrieving {0} xml node or tag or empty.", nodeName), Convert.ToString(e.Message), processData.CsvFileName, uniqueId, processData.OrderNumber, processData.XmlDocument, string.Format("Error 03.{0} - Issue with node {1}", errorNumber, nodeName));
                return null;
            }
        }

        /// <summary>
        /// Sets the ShippingSchedule Other Schedule Reference properties used to output the xCBL data
        /// </summary>
        /// <param name="shippingSchedule">The Reference Description text to output</param>
        /// <param name="referenceTypeCodedOther">The ShippingSchedule object that is set</param>
        /// <param name="referenceDescription">The index of the Other Schedule Reference item</param>
        public static void SetOtherScheduleReferenceDesc(this ShippingSchedule shippingSchedule, string referenceTypeCodedOther, string referenceDescription)
        {
            string referenceTypeCoded = string.Format("Other_{0}", referenceTypeCodedOther).ToLower().Trim();

            switch (referenceTypeCoded)
            {
                case "other_firststop":
                    shippingSchedule.Other_FirstStop = referenceDescription;
                    break;
                case "other_before7":
                    shippingSchedule.Other_Before7 = referenceDescription;
                    break;
                case "other_before9":
                    shippingSchedule.Other_Before9 = referenceDescription;
                    break;
                case "other_before12":
                    shippingSchedule.Other_Before12 = referenceDescription;
                    break;
                case "other_sameday":
                    shippingSchedule.Other_SameDay = referenceDescription;
                    break;
                case "other_homeowneroccupied":
                    shippingSchedule.Other_OwnerOccupied = referenceDescription;
                    break;
                case "other_workordernumber":
                    shippingSchedule.WorkOrderNumber = referenceDescription;
                    shippingSchedule.Other_7 = referenceDescription;
                    break;
                case "other_ssid":
                    shippingSchedule.SSID = referenceDescription;
                    shippingSchedule.Other_8 = referenceDescription;
                    break;
                case "other_7":
                    shippingSchedule.Other_7 = referenceDescription;
                    break;
                case "other_8":
                    shippingSchedule.Other_8 = referenceDescription;
                    break;
                case "other_9":
                    shippingSchedule.Other_9 = referenceDescription;
                    break;
                case "other_10":
                    shippingSchedule.Other_10 = referenceDescription;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the ShippingSchedule Contact Numbers properties used to output the xCBL data
        /// </summary>
        /// <param name="contactNumber">The Contact Number text</param>
        /// <param name="shippingSchedule">The ShippingSchedule object that is set</param>
        /// <param name="contactNumberIndex">The index of the Contact Number item</param>
        public static void SetContactNumbers(this ShippingSchedule shippingSchedule, string contactNumber, int contactNumberIndex)
        {
            switch (contactNumberIndex)
            {
                case 0:
                    shippingSchedule.ContactNumber_1 = contactNumber;
                    break;
                case 1:
                    shippingSchedule.ContactNumber_2 = contactNumber;
                    break;
                case 2:
                    shippingSchedule.ContactNumber_3 = contactNumber;
                    break;
                case 3:
                    shippingSchedule.ContactNumber_4 = contactNumber;
                    break;
                case 4:
                    shippingSchedule.ContactNumber_5 = contactNumber;
                    break;
                case 5:
                    shippingSchedule.ContactNumber_6 = contactNumber;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The function which replaces the special characters (Comma, Carriage return and Line Feed) to space found in the xml String
        /// </summary>
        /// <param name="value">Xml Data</param>
        public static string ReplaceSpecialCharsWithSpace(this string value, bool isDoubleQuoatCheck = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    char charLineFeed = (char)10;
                    char charCarriageReturn = (char)13;
                    char charComma = (char)44;
                    char charDoubleQuoat = (char)34;
                    if (value.IndexOf(charCarriageReturn) != -1)
                        value = value.Replace(charCarriageReturn.ToString(), " ");

                    if (value.IndexOf(charLineFeed) != -1)
                        value = value.Replace(charLineFeed.ToString(), " ");

                    if (value.IndexOf(charComma) != -1)
                        value = value.Replace(charComma.ToString(), " ");

                    if (isDoubleQuoatCheck && value.IndexOf(charDoubleQuoat) != -1)
                        value = value.Replace(charDoubleQuoat.ToString(), string.Empty);
                }
                return value;
            }
            catch
            {
                return value;
            }
        }

        public static string ExtractNumericOrderNumber(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            string[] orderNumberSplitted = value.Trim().Split(' ');

            if (orderNumberSplitted.Length > 1)
                return orderNumberSplitted[1];

            return
                value.Trim().Substring(value.Length - 8);
        }

        public static bool VerifyDatetimeExpaire(this DateTime deliveryDate)
        {
            if (DateTime.Compare(deliveryDate, DateTime.UtcNow) > 0)
                return true;
            return false;
        }
    }
}