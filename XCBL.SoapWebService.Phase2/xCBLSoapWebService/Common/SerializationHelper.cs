using System;
using System.IO;
using System.Text;

namespace xCBLSoapWebService.Common
{
	public static class SerializationHelper
	{
		private static readonly Encoding DefaultContentEncoding = Encoding.UTF8;

		public static byte[] Serialize(object content, string contentType = "application/json", Encoding contentEncoding = null)
		{
			if (content == null)
				return (byte[])null;
			byte[] numArray1 = (byte[])null;
			if (contentType == "application/json")
			{
				string s = JsonSerializer.Serialize(content, false);
				numArray1 = (contentEncoding ?? SerializationHelper.DefaultContentEncoding).GetBytes(s);
			}
			else if (contentType == "application/xml" || contentType == "text/xml")
			{
				string s = XmlSerializer.Serialize(content, false);
				numArray1 = (contentEncoding ?? SerializationHelper.DefaultContentEncoding).GetBytes(s);
			}
			else if (content is string)
				numArray1 = (contentEncoding ?? SerializationHelper.DefaultContentEncoding).GetBytes((string)content);
			else if (content.GetType() == typeof(byte[]))
			{
				byte[] numArray2 = (byte[])content;
				numArray1 = new byte[numArray2.Length];
				numArray2.CopyTo((Array)numArray1, 0);
			}
			return numArray1;
		}

		public static TResult Deserialize<TResult>(Stream contentStream, string contentTypeList = "application/json")
		{
			bool timedOut = false;
			return SerializationHelper.Deserialize<TResult>(contentStream, contentStream.Length, ref timedOut, contentTypeList);
		}

		public static TResult Deserialize<TResult>(
		  Stream contentStream,
		  long contentLength,
		  string contentTypeList = "application/json")
		{
			bool timedOut = false;
			return SerializationHelper.Deserialize<TResult>(contentStream, contentLength, ref timedOut, contentTypeList);
		}

		public static TResult Deserialize<TResult>(
		  Stream contentStream,
		  ref bool timedOut,
		  string contentTypeList = "application/json")
		{
			return SerializationHelper.Deserialize<TResult>(contentStream, contentStream.Length, ref timedOut, contentTypeList);
		}

		public static TResult Deserialize<TResult>(
		  Stream contentStream,
		  long contentLength,
		  ref bool timedOut,
		  string contentTypeList = "application/json")
		{
			TResult result = default(TResult);
			string str1 = string.Empty;
			if (contentTypeList != null)
			{
				string lower = contentTypeList.ToLower();
				char[] chArray = new char[1] { ';' };
				foreach (string str2 in lower.Split(chArray))
				{
					string str3 = str2.Trim();
					if (str3 == "application/json" || str3 == "application/xml" || str3 == "text/xml")
					{
						str1 = str3;
						break;
					}
				}
			}
			string str4 = str1;
			if (!(str4 == "application/json"))
			{
				if (str4 == "application/xml" || str4 == "text/xml")
				{
					result = XmlSerializer.Deserialize<TResult>(contentStream, (Encoding)null);
				}
				else
				{
					byte[] numArray = new byte[contentLength];
					int offset = 0;
					int count = contentLength < 8192L ? (int)contentLength : 8192;
					while ((long)offset < contentLength && contentStream.CanRead && !timedOut)
						offset += contentStream.Read(numArray, offset, count);
					contentStream.Close();
					// WebServiceResponseException responseException = new WebServiceResponseException("Unknown ContentType", contentTypeList, numArray);
				}
			}
			else
				result = JsonSerializer.Deserialize<TResult>(contentStream, Encoding.UTF8);
			return result;
		}
	}
}