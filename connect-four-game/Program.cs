    namespace connect_four_game
{
    public class Board
    {
        private char[,] grid;
        public int Rows { get; } = 6;
        public int Columns { get; } = 7;

        //Board
        public Board()
        {
            grid = new char[Rows, Columns];

            for(int r = 0; r < Rows; r++)
            {
                for(int c = 0; c < Columns; c++)
                {
                    grid[r, c] = '#';
                }
            }
        }

        //Display Board
        public void DisplayBoard()
        {
            for (int r = 0; r < Rows; r++)
            {
                Console.Write("|"); //Left Boarder
                for (int c = 0; c < Columns; c++)
                {
                    Console.Write(" " + grid[r, c] + " ");
                }
                Console.WriteLine("|"); // Right Border
                }

            //Print number 1 - 7
            Console.Write(" ");
            for (int c = 1; c <= Columns; c++)
            {
                Console.Write(" " + c + " ");
            }
            Console.WriteLine("\n");
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.DisplayBoard();

            Console.WriteLine("Hello World!");
        }
    }
}
