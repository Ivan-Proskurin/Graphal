namespace Graphal.Tools.Abstractions.Serialization
{
    public interface IJsonSerializationService
    {
        string Serialize<T>(T model);

        T Deserialize<T>(string value);
    }
}