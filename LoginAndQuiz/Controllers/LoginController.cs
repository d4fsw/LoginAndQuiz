using Microsoft.AspNetCore.Mvc;
using QuizLoginAPI.Models;
using QuizLoginAPI.Data;
using System.Text.RegularExpressions;

namespace QuizLoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] LoginDataModel request)
        {
            var errors = ValidateLoginRequest(request);
            if (errors.Count > 0)
            {
                return BadRequest(new { message = "Validation failed", errors });
            }
            
            return Ok(new { message = "Login successful" });
                      
        }

        private List<string> ValidateLoginRequest(LoginDataModel request)
        {
            var errors = new List<string>();
           
            // Username validation
            if (string.IsNullOrWhiteSpace(request.Username))
                errors.Add("Username is required.");
            else if (CountVowels(request.Username) < 2)
                    errors.Add("Username must contain at least 2 vowels (a, e, i, o, u).");

            int usernameLength = request.Username.Length;
            int sum = StaticData.NumberList.Take(usernameLength).Sum();

            // Password validation
            if (!request.Password.HasValue)
                errors.Add("Password is required.");
            else if (request.Password >= sum)
                errors.Add("Password is incorrect");
            
            return errors;
        }

        private int CountVowels(string input)
        {
            return input.Count(c => "aeiou".Contains(c));
        }
    }
}
