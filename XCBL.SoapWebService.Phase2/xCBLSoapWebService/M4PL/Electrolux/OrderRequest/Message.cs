﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xCBLSoapWebService.M4PL.Electrolux.OrderRequest
{
	[XmlRoot(ElementName = "message")]
	public class Message
	{
		[XmlElement(ElementName = "subject")]
		public string Subject { get; set; }
		[XmlElement(ElementName = "payloadType")]
		public string PayloadType { get; set; }
	}
}
