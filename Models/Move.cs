namespace ChessWasm.Models
{
    public class Move
    {
        public int From { get; set; }
        public int To { get; set; }
        public Piece Player { get; set; }
    }
}