using System.Collections.Generic;

namespace Trivia
{
    public class PlayerService {
        private readonly List<Player> players;
        private int currentPlayerPosition;
        private readonly Reporter reporter;
        private bool _isGettingOutOfPenaltyBox;
        private Player CurrentPlayer => players[currentPlayerPosition];


        public string CurrentPlayerName => CurrentPlayer.PlayerName;
        public int CurrentPlayerPosition => CurrentPlayer.Position;
        public int CurrentPlayerScore => CurrentPlayer.Score;
        public bool ItsCurrentPlayerInPenaltyBox => CurrentPlayer.ItsInPenaltyBox;
        public bool CurrentPlayerCanPlay => (CurrentPlayer.ItsInPenaltyBox == false) || (_isGettingOutOfPenaltyBox);


        public PlayerService(Reporter reporter) {
            players = new List<Player>();
            this.reporter = reporter;
        }

        public void GetOutofThePenaltyBox() {
            _isGettingOutOfPenaltyBox = true;
            reporter.Report(CurrentPlayerName + " is getting out of the penalty box");
        }

        public void StaysInThePenaltyBox() {
            reporter.Report(CurrentPlayerName + " is not getting out of the penalty box");
            _isGettingOutOfPenaltyBox = false;
        }

        public bool Add(string playerName) {
            var player = new Player(playerName);
            players.Add(player);
            currentPlayerPosition = players.Count - 1;
            ReportPlayerAdded();
            return true;
        }

        private void ReportPlayerAdded() {
            reporter.Report($"{CurrentPlayer.PlayerName} was added");
            reporter.Report($"They are player number {players.Count}");
        }

        public bool WrongAnswer() {
            ReportWrong();
            CurrentPlayer.GoToPenaltyBox();
            NextPlayer();
            return true;
        }

        private void ReportWrong() {
            reporter.Report("Question was incorrectly answered");
            reporter.Report(CurrentPlayer.PlayerName + " was sent to the penalty box");

        }

        public void NextPlayer() {
            currentPlayerPosition++;
            if (currentPlayerPosition == players.Count) {
                currentPlayerPosition = 0;
            }
        }

        public void RollPlayerTo(int roll) {
            CurrentPlayer.RollTo(roll);

            reporter.Report(CurrentPlayerName
                            + "'s new location is "
                            + CurrentPlayerPosition);
        }

        public bool WinCheese() {
            reporter.Report("Answer was correct!!!!");
            CurrentPlayer.WinCheese();
            reporter.Report(CurrentPlayerName
                            + " now has "
                            + CurrentPlayerScore
                            + " Gold Coins.");

            NextPlayer();
            return !CurrentPlayer.IWon;
        }
    }
}