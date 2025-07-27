using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginAndQuizAPI.Tests.Models
{
    public class UserAnswer
    {
        public int QuestionId { get; set; }
        public List<string> Answers { get; set; } = new();
    }
}
