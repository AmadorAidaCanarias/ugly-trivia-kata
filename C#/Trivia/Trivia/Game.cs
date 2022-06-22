using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia {
    public class Game
    {

        private readonly List<Player> players = new List<Player>();

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;
        private readonly Reporter reporter;
        private readonly QuestionService questionService;
        private Player CurrentPlayer => players[_currentPlayer];

        public Game(Reporter reporter)
        {
            this.reporter = reporter;
            questionService = new QuestionService(this.reporter);
        }

        public bool Add(string playerName)
        {
            var player = new Player(playerName);
            players.Add(player);
            
            reporter.Report($"{ player.PlayerName } was added");
            reporter.Report($"They are player number {players.Count}");
            return true;
        }

        public void Roll(int roll) {
            reporter.Report(CurrentPlayer.PlayerName + " is the current player");
            reporter.Report("They have rolled a " + roll);

            if (CurrentPlayer.ItsInPenaltyBox) {
                if (roll % 2 != 0) {
                    _isGettingOutOfPenaltyBox = true;

                    reporter.Report(CurrentPlayer + " is getting out of the penalty box");
                    CurrentPlayer.RollTo(roll);

                    reporter.Report(CurrentPlayer.PlayerName
                                     + "'s new location is "
                                     + CurrentPlayer.CurrentPosition);
                    reporter.Report("The category is " + questionService.CurrentCategory(CurrentPlayer.CurrentPosition));
                    questionService.AskQuestion(CurrentPlayer.CurrentPosition);
                }
                else {
                    reporter.Report(CurrentPlayer.PlayerName + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                CurrentPlayer.RollTo(roll);

                reporter.Report(CurrentPlayer.PlayerName
                                 + "'s new location is "
                                 + CurrentPlayer.CurrentPosition);
                reporter.Report("The category is " + questionService.CurrentCategory(CurrentPlayer.CurrentPosition));
                questionService.AskQuestion(CurrentPlayer.CurrentPosition);
            }
        }

        public bool WasCorrectlyAnswered() {
            if (CurrentPlayer.ItsInPenaltyBox) {
                if (_isGettingOutOfPenaltyBox) {
                    reporter.Report("Answer was correct!!!!");
                    CurrentPlayer.WinCheese();
                    reporter.Report(CurrentPlayer.PlayerName
                                     + " now has "
                                     + CurrentPlayer.CurrentScore
                                     + " Gold Coins.");

                    _currentPlayer++;
                    if (_currentPlayer == players.Count) _currentPlayer = 0;

                    return DidPlayerWin();
                }
                _currentPlayer++;
                if (_currentPlayer == players.Count) _currentPlayer = 0;
                return true;
            }
            reporter.Report("Answer was correct!!!!");
            CurrentPlayer.WinCheese();
            reporter.Report(CurrentPlayer.PlayerName
                            + " now has "
                            + CurrentPlayer.CurrentScore
                            + " Gold Coins.");

            _currentPlayer++;
            if (_currentPlayer == players.Count) _currentPlayer = 0;

            return DidPlayerWin();
        }

        public bool WrongAnswer() {
            reporter.Report("Question was incorrectly answered");
            reporter.Report(CurrentPlayer.PlayerName + " was sent to the penalty box");
            CurrentPlayer.InPenaltyBox();

            _currentPlayer++;
            if (_currentPlayer == players.Count) _currentPlayer = 0;
            return true;
        }


        private bool DidPlayerWin() {
            return !CurrentPlayer.IWon;
        }
    }

}
