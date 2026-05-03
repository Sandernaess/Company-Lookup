using System.Text.Json.Serialization;

namespace CompanyLookup.Api.External.Brreg.Models.Enhet
{
    public class EnhetSearchResponse
    {
        [JsonPropertyName("_embedded")]
        public EnhetEmbeddedResponse? Embedded { get; set; }
    }
}
