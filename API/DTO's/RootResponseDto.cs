using System.Text.Json.Serialization;

namespace API.DTO_s
{
    public class RootResponseDto
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }

        public required List<QuizQuestionDto> Results { get; set; }
    }
}
