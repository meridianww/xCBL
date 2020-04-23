using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using xCBLSoapWebService.M4PL.Electrolux.OrderRequest;
using xCBLSoapWebService.M4PL.Electrolux.OrderResponse;

namespace xCBLSoapWebService.M4PL.Electrolux
{
    public class ProcessElectrolux
    {

        private MeridianResult _meridianResult = null;
        internal MeridianResult ProcessElectroluxDocument(OperationContext currentOperationContext)
        {

            _meridianResult = new MeridianResult();
            _meridianResult.IsSchedule = false;
            _meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_SUCCESS;
            XCBL_User xCblServiceUser = new XCBL_User();
            if (CommonProcess.IsAuthenticatedRequest(currentOperationContext, ref xCblServiceUser))
            {
                MeridianSystemLibrary.LogTransaction(xCblServiceUser.WebUsername, xCblServiceUser.FtpUsername, "IsAuthenticatedRequest", "01.02", "Success - Authenticated request", "Electrolux Process", "No FileName", "No Electrolux ID", "No Order Number", null, "Success");

                var requestContext = currentOperationContext.RequestContext;
                var requestMessage = requestContext.RequestMessage.ToString();//.ReplaceSpecialCharsWithSpace();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(requestMessage);

                XmlNodeList requisitionNode_xml = xmlDoc.GetElementsByTagName("fxEnvelope");//Http Request creating this tag

                string outerXml = requisitionNode_xml[0].OuterXml;
                string xmlwithouotNameSpace = RemoveAllNamespaces(outerXml);

                ElectroluxOrderDetails electroluxOrderDetails = null;
                XmlSerializer serializer = new XmlSerializer(typeof(ElectroluxOrderDetails));
                using (var stringReader = new StringReader(xmlwithouotNameSpace))
                {
                    electroluxOrderDetails = (ElectroluxOrderDetails)serializer.Deserialize(stringReader);
                }

                if (electroluxOrderDetails != null)
                {
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableXCBLForElectroluxToSyncWithM4PL"]))
                    {
                        var response = M4PL.M4PLService.CallM4PLAPI<List<OrderResponseResult>>(electroluxOrderDetails, "XCBL/Electrolux/OrderRequest");
                        if (response != null)
                        {
                            _meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_SUCCESS;

                        }
                    }
                }
                else
                    _meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_FAILURE;
            }
            else
            {
                _meridianResult.Status = MeridianGlobalConstants.MESSAGE_ACKNOWLEDGEMENT_FAILURE;
                MeridianSystemLibrary.LogTransaction("No WebUser", "No FTPUser", "IsAuthenticatedRequest", "03.01", "Error - New SOAP Request not authenticated", "UnAuthenticated Request", "No FileName", "No Requisition ID", "No Order Number", null, "Error 03.01 - Invalid Credentials");
            }
            return _meridianResult;
        }


        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

    }


}