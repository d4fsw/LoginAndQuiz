using System.Text.Json.Serialization;

namespace QuizLoginAPI.Models
{
    public class ResultRootModel
    {
        [JsonPropertyName("quiz_id")]
        public int Quiz_Id { get; set; }

        [JsonPropertyName("results")]
        public List<ResultModel> Results { get; set; }
    }

    public class ResultModel
    {
        [JsonPropertyName("r_id")]
        public int R_Id { get; set; }

        [JsonPropertyName("minpoints")]
        public int MinPoints { get; set; }

        [JsonPropertyName("maxpoints")]
        public int MaxPoints { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("img")]
        public string Img { get; set; }
    }
}
