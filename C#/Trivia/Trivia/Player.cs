namespace Trivia
{
    public class Player
    {
        private readonly string name;
        private int place;
        private int purse;
        private bool isPenaltyBox;

        public Player(string name) {
            this.name = name;
            place = 0;
            purse = 0;
            isPenaltyBox = false;
        }

        public void InPenaltyBox() {
            isPenaltyBox = true;
        }

        public bool ItsInPenaltyBox => isPenaltyBox;
        public string PlayerName => name;

        public void RollTo(int roll) {
            place += roll;
            if (place > 11)
            {
                place = 0;
            }

            isPenaltyBox = false;
        }

        public void WinCheese()
        {
            purse ++;
        }

        public bool IWon => purse == 6;
        public int CurrentScore => purse;
        public int CurrentPosition => place;
    }
}