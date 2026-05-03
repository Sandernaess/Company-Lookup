using System.Text.Json.Serialization;

namespace CompanyLookup.Api.External.Brreg.Enhet
{
    public class EnhetSearchResponse
    {
        [JsonPropertyName("_embedded")]
        public EnhetEmbeddedResponse? Embedded { get; set; }
    }
}
