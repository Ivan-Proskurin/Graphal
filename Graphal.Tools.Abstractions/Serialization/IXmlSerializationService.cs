namespace Graphal.Tools.Abstractions.Serialization
{
    public interface IXmlSerializationService
    {
        string Serialize<T>(T model);

        T Deserialize<T>(string value);
    }
}