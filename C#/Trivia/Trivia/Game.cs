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

        public void Roll(int roll) {
            MovementInformation(roll);
            if (playerService.ItsCurrentPlayerInPenaltyBox) {
                RollPlayerInPenaltyBox(roll);
            }
            else {
                RollAndAsk(roll);
            }
        }

        private void RollPlayerInPenaltyBox(int roll) {
            if (roll % 2 != 0) {
                playerService.GetOutofThePenaltyBox();
                RollAndAsk(roll);
            }
            else {
                playerService.StaysInThePenaltyBox();
            }
        }

        private void RollAndAsk(int roll) {
            playerService.RollPlayerTo(roll);
            questionService.AskQuestion(playerService.CurrentPlayerPosition);
        }

        private void MovementInformation(int roll) {
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
