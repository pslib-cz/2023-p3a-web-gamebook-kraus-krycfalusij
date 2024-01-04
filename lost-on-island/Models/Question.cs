namespace lost_on_island.Models
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        private Random _random = new Random();

        public Question(string text, List<string> options, string correctAnswer)
        {
            Text = text;
            Options = options;
            CorrectAnswer = correctAnswer;
        }
        public int EvaluateAnswer(string userAnswer)
        {
            if (userAnswer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase))
            {
                return _random.Next(1, 2); 
            }
            else
            {
                return _random.Next(4, 8); 
            }

        }
    }
}
