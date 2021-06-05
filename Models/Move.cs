namespace ChessWasm.Models
{
    public enum PromotionType : uint
    {
        Queen = 0,
        Knight = 1,
        Rook = 2,
        Bishop = 3
    }

    public enum SpecialMove : uint
    {
        None = 0,
        Promotion = 1,
        EnPassant = 2,
        Castling = 3
    }

    public class OldMove
    {
        public int From { get; set; }
        public int To { get; set; }
        public Piece Player { get; set; }
    }

    public class Move
    {
        public const int ToBits = 0x3F; // bit 0-5
        public const int FromBits = 0xFC0; // bit 6-11
        public const int PromotionBits = 0x3000; // bit 12-13
        public const int SpecialMoveBits = 0xC000; // bit 14-15

        private uint encodedMove;

        public uint To { get => encodedMove & ToBits; }
        public uint From { get => (encodedMove & FromBits) >> 6; }
        public PromotionType PromotionType { get => (PromotionType)((encodedMove & PromotionBits) >> 12); }
        public SpecialMove SpecialMove { get => (SpecialMove)((encodedMove & SpecialMoveBits) >> 14); }

        public Move(uint _encodedMove)
        {
            encodedMove = _encodedMove;
        }

        public Move(uint From, uint To, PromotionType promotion = PromotionType.Queen, SpecialMove specialMove = SpecialMove.None)
        {
            encodedMove = To | (From << 6) | ((uint)promotion << 12) | ((uint)specialMove << 14);
        }
    }
}