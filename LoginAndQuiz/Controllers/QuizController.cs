using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using QuizLoginAPI.Models;

namespace QuizLoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public QuizController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult GetQuiz()
        {
            var path = Path.Combine(_env.ContentRootPath, "Data", "quiz.json");
            if (!System.IO.File.Exists(path))
                return NotFound(new { message = "Quiz not found" });

            var json = System.IO.File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<QuizModel> (json);
            return Ok(data);
        }

        [HttpGet("result")]
        public IActionResult GetResult()
        {
            var path = Path.Combine(_env.ContentRootPath, "Data", "result.json");
            if (!System.IO.File.Exists(path))
                return NotFound(new { message = "Result file not found" });

            var json = System.IO.File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<ResultRootModel>(json);
            return Ok(data);
        }
    }
}
