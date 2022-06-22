namespace Trivia
{
    public class Question {
        
        public QuestionType Type { get; set; }
        public string QuestionText { get; }

        public Question(QuestionType type, int index) {
            Type = type;
            QuestionText = $"{type.ToString()} Question {index}";
        }
    }
}