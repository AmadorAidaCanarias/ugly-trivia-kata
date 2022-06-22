namespace Trivia {
    public class Game {
        private readonly Reporter reporter;
        private readonly QuestionService questionService;
        private readonly PlayerService playerService;


        public Game(Reporter reporter) {
            this.reporter = reporter;
            questionService = new QuestionService(this.reporter);
            playerService = new PlayerService(this.reporter);
        }

        public bool Add(string playerName) {
            return playerService.Add(playerName);
        }

        public void Roll(int roll)
        {
            ReportRoll(roll);

            if (playerService.ItsCurrentPlayerInPenaltyBox) {
                if (roll % 2 != 0) {
                    playerService.GetOutofThePenaltyBox();
                    playerService.RollPlayerTo(roll);
                    questionService.AskQuestion(playerService.CurrentPlayerPosition);
                }
                else {
                    playerService.StaysInThePenaltyBox();
                }
            }
            else {
                playerService.RollPlayerTo(roll);
                questionService.AskQuestion(playerService.CurrentPlayerPosition);
            }
        }

        private void ReportRoll(int roll)
        {
            reporter.Report(playerService.CurrentPlayerName + " is the current player");
            reporter.Report("They have rolled a " + roll);
        }

        public bool WasCorrectlyAnswered() {
            if (playerService.CurrentPlayerCanPlay) return playerService.WinCheese();
            playerService.NextPlayer();
            return true;
        }

        public bool WrongAnswer() {
            return playerService.WrongAnswer();
        }
    }

}
