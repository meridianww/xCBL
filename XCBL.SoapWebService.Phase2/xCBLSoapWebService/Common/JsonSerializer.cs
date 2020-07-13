using System;
using System.IO;
using System.Text;

namespace xCBLSoapWebService.Common
{
	public static class JsonSerializer
	{
		private static readonly Newtonsoft.Json.JsonSerializer _jsonSerializer = new Newtonsoft.Json.JsonSerializer();

		public static string Serialize(object data, bool formatted = false)
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				JsonSerializer.Serialize(data, (TextWriter)stringWriter, formatted);
				return stringWriter.ToString();
			}
		}

		public static void Serialize(object data, Stream stream, Encoding encoding, bool formatted = false)
		{
			JsonSerializer.Serialize(data, (TextWriter)new StreamWriter(stream, encoding)
			{
				AutoFlush = true
			}, formatted);
		}

		public static void Serialize(object data, TextWriter writer, bool formatted = false)
		{
			JsonSerializer._jsonSerializer.Serialize(writer, data);
		}

		public static T Deserialize<T>(Stream stream, Encoding encoding)
		{
			return (T)JsonSerializer.Deserialize(stream, encoding, typeof(T));
		}

		public static object Deserialize(Stream stream, Encoding encoding, Type type)
		{
			return JsonSerializer.Deserialize((TextReader)new StreamReader(stream, encoding), type);
		}

		public static T Deserialize<T>(string data)
		{
			return (T)JsonSerializer.Deserialize(data, typeof(T));
		}

		public static object Deserialize(string data, Type type)
		{
			using (StringReader stringReader = new StringReader(data))
				return JsonSerializer.Deserialize((TextReader)stringReader, type);
		}

		public static object Deserialize(TextReader reader, Type type)
		{
			return JsonSerializer._jsonSerializer.Deserialize(reader, type);
		}
	}
}