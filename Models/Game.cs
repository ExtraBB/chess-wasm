namespace ChessWasm.Models
{
    public class Game
    {
        public Board Board { get; private set; }

        public Game() 
        {
            Board = new Board();
        }

        public void MakeMove(int from, int to) 
        {
            if(IsLegal(from, to)) 
            {
                Board.MakeMove(from, to);
            }
        }

        private bool IsLegal(int from, int to) 
        {
            return true;
        }
    }
}