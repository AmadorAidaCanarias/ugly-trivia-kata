namespace Trivia
{
    public class QuestionService
    {
        private readonly Reporter reporter;
        private readonly Questions questions = new Questions();


        public QuestionService(Reporter reporter)
        {
            InitQuestions();
            this.reporter = reporter;
        }

        private void InitQuestions()
        {
            for (var i = 0; i < 50; i++)
            {
                questions.AddQuestion(QuestionType.Pop, i);
                questions.AddQuestion(QuestionType.Science, i);
                questions.AddQuestion(QuestionType.Sport, i);
                questions.AddQuestion(QuestionType.Rock, i);
            }
        }


        public void AskQuestion(int place)
        {
            var currentCategoryType = CurrentCategoryType(place);
            reporter.Report("The category is " + CurrentCategory(place));
            reporter.Report(questions.MakeAsk(currentCategoryType));
            questions.RemoveLast(currentCategoryType);
        }

        private QuestionType CurrentCategoryType(int place) =>
            place switch
            {
                0 => QuestionType.Pop,
                4 => QuestionType.Pop,
                8 => QuestionType.Pop,
                1 => QuestionType.Science,
                5 => QuestionType.Science,
                9 => QuestionType.Science,
                2 => QuestionType.Sport,
                6 => QuestionType.Sport,
                10 => QuestionType.Sport,
                _ => QuestionType.Rock
            };

        private string CurrentCategory(int place)
        {
            QuestionType currentType = CurrentCategoryType(place);
            return currentType.ToString();
        }
    }
}