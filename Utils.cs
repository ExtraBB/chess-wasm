using System;

namespace ChessWasm
{
    public static class Utils
    {
        public static int RowFileToPosition(int row, int file) 
        {
            return row * 8 + file;
        }

        public static int PositionToRow(int position) 
        {
            return position / 8;
        }

        public static int PositionToFile(int position) 
        {
            return position % 8;
        }
    }
}