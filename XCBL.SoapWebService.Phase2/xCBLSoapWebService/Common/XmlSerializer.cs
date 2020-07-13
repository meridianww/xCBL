using System;
using System.IO;
using System.Text;
using System.Xml;

namespace xCBLSoapWebService.Common
{
	public static class XmlSerializer
	{
		public static string Serialize(object data, bool formatted = false)
		{
			StringWriter stringWriter = new StringWriter();
			XmlSerializer.Serialize(data, (TextWriter)stringWriter);
			return stringWriter.ToString();
		}

		public static void Serialize(object data, string outputFileName)
		{
			XmlWriter writer = XmlWriter.Create(outputFileName);
			XmlSerializer.Serialize(data, writer);
			writer.Close();
		}

		public static void Serialize(object data, Stream stream, Encoding encoding, bool formatted = false)
		{
			XmlSerializer.Serialize(data, XmlWriter.Create(stream, new XmlWriterSettings()
			{
				Indent = formatted,
				Encoding = encoding
			}));
		}

		public static void Serialize(object data, XmlWriter writer)
		{
			new System.Xml.Serialization.XmlSerializer(data.GetType()).Serialize(writer, data);
		}

		public static void Serialize(object data, TextWriter writer)
		{
			new System.Xml.Serialization.XmlSerializer(data.GetType()).Serialize(writer, data);
		}

		public static object Deserialize(string inputFileName, Type type)
		{
			XmlReader reader = XmlReader.Create(inputFileName);
			object obj = XmlSerializer.Deserialize(reader, type);
			reader.Close();
			return obj;
		}

		public static T Deserialize<T>(string data)
		{
			return (T)XmlSerializer.Deserialize(XmlReader.Create((TextReader)new StringReader(data)), typeof(T));
		}

		public static T Deserialize<T>(Stream stream, Encoding encoding)
		{
			return (T)XmlSerializer.Deserialize(stream, encoding, typeof(T));
		}

		public static object Deserialize(Stream stream, Encoding encoding, Type type)
		{
			return XmlSerializer.Deserialize(XmlReader.Create(stream), type);
		}

		public static object Deserialize(XmlReader reader, Type type)
		{
			return new System.Xml.Serialization.XmlSerializer(type).Deserialize(reader);
		}
	}
}