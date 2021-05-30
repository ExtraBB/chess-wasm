namespace ChessWasm.Services
{
    public static class Utils
    {
        public static int RowFileToPosition(int row, int file) 
        {
            return row * 8 + file;
        }
    }
}