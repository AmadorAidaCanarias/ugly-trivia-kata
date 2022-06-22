using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia {
    public class Game {
        private readonly List<string> _players = new List<string>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;
        private readonly Reporter reporter;
        private readonly QuestionService questionService;

        public Game()
        {
            reporter = new Reporter();
            questionService = new QuestionService(reporter);
        }

        public bool Add(string playerName) {
            _players.Add(playerName);
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;

            reporter.Report(playerName + " was added");
            reporter.Report("They are player number " + _players.Count);
            return true;
        }

        public int HowManyPlayers() {
            return _players.Count;
        }

        public void Roll(int roll) {
            reporter.Report(_players[_currentPlayer] + " is the current player");
            reporter.Report("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer]) {
                if (roll % 2 != 0) {
                    _isGettingOutOfPenaltyBox = true;

                    reporter.Report(_players[_currentPlayer] + " is getting out of the penalty box");
                    _places[_currentPlayer] = _places[_currentPlayer] + roll;
                    if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                    reporter.Report(_players[_currentPlayer]
                                     + "'s new location is "
                                     + _places[_currentPlayer]);
                    reporter.Report("The category is " + questionService.CurrentCategory(_places[_currentPlayer]));
                    questionService.AskQuestion(_places[_currentPlayer]);
                }
                else {
                    reporter.Report(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else {
                _places[_currentPlayer] = _places[_currentPlayer] + roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                reporter.Report(_players[_currentPlayer]
                                 + "'s new location is "
                                 + _places[_currentPlayer]);
                reporter.Report("The category is " + questionService.CurrentCategory(_places[_currentPlayer]));
                questionService.AskQuestion(_places[_currentPlayer]);
            }
        }

        public bool WasCorrectlyAnswered() {
            if (_inPenaltyBox[_currentPlayer]) {
                if (_isGettingOutOfPenaltyBox) {
                    reporter.Report("Answer was correct!!!!");
                    _purses[_currentPlayer]++;
                    reporter.Report(_players[_currentPlayer]
                                     + " now has "
                                     + _purses[_currentPlayer]
                                     + " Gold Coins.");

                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;

                    return DidPlayerWin();
                }
                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;
                return true;
            }
            reporter.Report("Answer was corrent!!!!");
            _purses[_currentPlayer]++;
            reporter.Report(_players[_currentPlayer]
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;

            return DidPlayerWin();
        }

        public bool WrongAnswer() {
            reporter.Report("Question was incorrectly answered");
            reporter.Report(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }


        private bool DidPlayerWin() {
            return !(_purses[_currentPlayer] == 6);
        }
    }

}
