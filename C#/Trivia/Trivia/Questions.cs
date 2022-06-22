using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Questions {
        private readonly LinkedList<Question> questions;

        public Questions()
        {
            questions = new LinkedList<Question>();
        }

        public void AddQuestion(QuestionType type, int index) {
            questions.AddLast(new Question(type, index));
        }

        public void RemoveLast(QuestionType type)
        {
            LinkedList<Question> typeQuestions = questions.Where(row => row.Type == type) as LinkedList<Question>;
            typeQuestions?.RemoveLast();
        }

        public string MakeAsk(QuestionType type) {
            LinkedList<Question> typeQuestions = questions.Where(row => row.Type == type) as LinkedList<Question>;
            return typeQuestions?.First().QuestionText;
        }
    }
}