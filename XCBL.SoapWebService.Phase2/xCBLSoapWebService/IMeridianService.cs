//Copyright (2016) Meridian Worldwide Transportation Group
//All Rights Reserved Worldwide
//====================================================================================================================================================
//Program Title:                                Meridian xCBL Web Service - AWC Timberlake
//Programmer:                                   Nathan Fujimoto
//Date Programmed:                              2/6/2016
//Program Name:                                 Meridian xCBL Web Service
//Purpose:                                      The web service uses XmlElement as the parameter for the method
//
//====================================================================================================================================================
using System;
using System.ServiceModel;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace xCBLSoapWebService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMeridianService" in both code and config file together.
	[ServiceContract(Namespace = "http://tempuri.org")]
	public interface IMeridianService
	{
		//[OperationContract]
		//[SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare)]
		//XElement SubmitDocument();

		//[OperationContract]
		//[SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare)]
		//XElement HelloWorld();

		[OperationContract(AsyncPattern = true)]
		[SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare)]
		IAsyncResult BeginSubmitDocument(AsyncCallback callback, object asyncState);

		XElement EndSubmitDocument(IAsyncResult result);

		[OperationContract(AsyncPattern = true)]
		[SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare)]
		IAsyncResult BeginRequisition(AsyncCallback callback, object asyncState);

		XElement EndRequisition(IAsyncResult result);

		[OperationContract(AsyncPattern = true)]
		[SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare)]
		IAsyncResult BeginShippingScheduleResponse(AsyncCallback callback, object asyncState);

		XElement EndShippingScheduleResponse(IAsyncResult result);

		[OperationContract(AsyncPattern = true)]
		[SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare)]
		IAsyncResult BeginOrderRequest(AsyncCallback callback, object asyncState);

		XElement EndOrderRequest(IAsyncResult result);
	}
}