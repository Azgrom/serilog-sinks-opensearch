using System.Runtime.Serialization;

public sealed class OpenSearchActionPayload
{
    public OpenSearchActionPayload(string indexName, string pipeline = null, string id = null, string mappingType = null)
    {
        IndexName = indexName;
        Pipeline = pipeline;
        Id = id;
        MappingType = mappingType;
    }

    [DataMember(Name = "_type")]
    public string MappingType { get; }

    [DataMember(Name = "_index")]
    public string IndexName { get; }

    [DataMember(Name = "pipeline")]
    public string Pipeline { get; }

    [DataMember(Name = "_id")]
    public string Id { get; }
}