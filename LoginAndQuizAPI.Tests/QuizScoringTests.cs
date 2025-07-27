
using LoginAndQuizAPI.Tests.Models;

namespace LoginAndQuizAPI.Tests
{
   public class QuizScoringTests
   {
        private readonly List<Question> _questions = new()
        {
            new Question
            {
                Id = 1,
                Type = "single",
                Points = 5,
                CorrectAnswers = new List<string> { "3" }
            },
            new Question
            {
                Id = 2,
                Type = "multiple",
                Points = 5,
                CorrectAnswers = new List<string> { "1", "4" }
            },
            new Question
            {
                Id = 3,
                Type = "truefalse",
                Points = 3,
                CorrectAnswers = new List<string> { "true" }
            }
        };

        private bool EvaluateAnswer(Question q, List<string> userAnswers)
        {
            var correct = q.CorrectAnswers.OrderBy(x => x).ToList();
            var given = userAnswers.OrderBy(x => x).ToList();
            return correct.SequenceEqual(given);
        }

        private (int Score, int TotalPoints, int Percentage) CalculateScore(List<UserAnswer> answers)
        {
            int score = 0;
            int total = _questions.Sum(q => q.Points);

            foreach (var q in _questions)
            {
                var user = answers.FirstOrDefault(a => a.QuestionId == q.Id);
                if (user != null && EvaluateAnswer(q, user.Answers))
                    score += q.Points;
            }

            int percentage = total == 0 ? 0 : (int)Math.Round((double)score / total * 100);
            return (score, total, percentage);
        }

        // TESTS
        [Fact]
        public void All_Correct_Answers_Returns_100()
        {
            var userAnswers = new List<UserAnswer>
            {
                new UserAnswer { QuestionId = 1, Answers = new List<string> { "3" } },
                new UserAnswer { QuestionId = 2, Answers = new List<string> { "1", "4" } },
                new UserAnswer { QuestionId = 3, Answers = new List<string> { "true" } }
            };

            var (score, total, percent) = CalculateScore(userAnswers);

            Assert.Equal(13, score);
            Assert.Equal(13, total);
            Assert.Equal(100, percent);
        }

        [Fact]
        public void One_Wrong_Answer_Lowers_Score()
        {
            var userAnswers = new List<UserAnswer>
            {
                new UserAnswer { QuestionId = 1, Answers = new List<string> { "3" } },
                new UserAnswer { QuestionId = 2, Answers = new List<string> { "1" } }, // incomplete
                new UserAnswer { QuestionId = 3, Answers = new List<string> { "true" } }
            };

            var (score, total, percent) = CalculateScore(userAnswers);

            Assert.Equal(8, score); // missed 5 points from q2
            Assert.Equal(13, total);
            Assert.Equal(62, percent);
        }

        [Fact]
        public void No_Answers_Returns_Zero()
        {
            var userAnswers = new List<UserAnswer>(); // empty

            var (score, total, percent) = CalculateScore(userAnswers);

            Assert.Equal(0, score);
            Assert.Equal(13, total);
            Assert.Equal(0, percent);
        }

    }
}
