using System.Text.Json.Serialization;

namespace QuizLoginAPI.Models
{
    public class QuizModel
    {
        [JsonPropertyName("quiz_id")]
        public int Quiz_Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("questions")]
        public List<QuestionModel> Questions { get; set; }
    }

    public class QuestionModel
    {
        [JsonPropertyName("q_id")]
        public int Q_Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("img")]
        public string Img { get; set; }

        [JsonPropertyName("question_type")]
        public string Question_Type { get; set; }

        [JsonPropertyName("possible_answers")]
        public List<AnswerModel> Possible_Answers { get; set; }

        [JsonPropertyName("correct_answer")]
        public object Correct_Answer { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }
    }

    public class AnswerModel
    {
        [JsonPropertyName("a_id")]
        public int A_Id { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }
    }
}
