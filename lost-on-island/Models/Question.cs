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

        // Vrátí náhodné poškození způsobené hráči na základě odpovědi
        public int EvaluateAnswer(string userAnswer)
        {
            if (userAnswer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase))
            {
                // Hráč odpověděl správně, náhodné poškození mezi 1 a 2
                return _random.Next(1, 3); // Vrátí 1 nebo 2
            }
            else
            {
                // Hráč odpověděl nesprávně, náhodné poškození mezi 4 a 8
                return _random.Next(4, 9); // Vrátí hodnotu mezi 4 a 8
            }
        }
    }
}
