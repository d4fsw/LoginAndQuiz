using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginAndQuizAPI.Tests.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Points { get; set; }
        public List<string> CorrectAnswers { get; set; } = new();
    }
}
