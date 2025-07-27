namespace LoginAndQuizAPI.Tests
{
    public class LoginValidatorTests
    {
        [Theory]
        [InlineData("johnas", true)]
        [InlineData("JOHN", false)]
        [InlineData("user123", false)]
        [InlineData("valid", true)]
        public void Username_Validation_Works(string username, bool expected)
        {
            var isValid = System.Text.RegularExpressions.Regex.IsMatch(username, "^[a-z]+$");
            Assert.Equal(expected, isValid);
        }

        [Theory]
        [InlineData("199", true)]
        [InlineData("100", true)]
        [InlineData("99", false)]
        [InlineData("abc", false)]
        [InlineData("1000", false)]
        public void Password_Validation_Works(string password, bool expected)
        {
            var isValid = int.TryParse(password, out int pw) && pw >= 100 && pw <= 999;
            Assert.Equal(expected, isValid);
        }
    }
}