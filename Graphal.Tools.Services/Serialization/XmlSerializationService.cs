using System.IO;
using System.Text;
using System.Xml.Serialization;

using Graphal.Tools.Abstractions.Serialization;

namespace Graphal.Tools.Services.Serialization
{
    public class XmlSerializationService : IXmlSerializationService
    {
        public string Serialize<T>(T model)
        {
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, model);
                writer.Flush();
                stream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public T Deserialize<T>(string value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                writer.Write(value);
                writer.Flush();
                stream.Position = 0;
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}