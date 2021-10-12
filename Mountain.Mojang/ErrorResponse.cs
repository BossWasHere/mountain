using System.Text;
using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct ErrorResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
        [JsonPropertyName("cause")]
        public string Cause { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("ErrorResponse;");
            builder.Append("Error=").Append(Error);
            builder.Append(",ErrorMessage=").Append(ErrorMessage);
            builder.Append(",Cause=").Append(Cause);

            return builder.ToString();
        }
    }
}
