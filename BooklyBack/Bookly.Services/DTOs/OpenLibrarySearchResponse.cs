using System.Text.Json.Serialization;

namespace Bookly.Services.DTOs;

public class OpenLibrarySearchResponse
{
    [JsonPropertyName("numFound")]
    public int NumFound { get; set; }

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("docs")]
    public List<OpenLibraryWorkDoc> Docs { get; set; } = new();
}
