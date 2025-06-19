    namespace connect_four_game
{

    //Board Class
    public class Board
    {
        private char[,] grid;
        public int Rows { get; } = 6;
        public int Columns { get; } = 7;

        //Constructor for board
        public Board()
        {
            grid = new char[Rows, Columns];

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
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

        //Dropping X and O
        public bool DroppingXO(int column, char disc)
        {
            if (column < 0 || column >= Columns)
                return false;

            {
                for (int row = Rows - 1; row >= 0; row--)
                {
                    if (grid[row, column] == '#')
                    {
                        grid[row, column] = disc;
                        return true;
                    }
                }
            }
            return false;
        }
    }

    //Player Class
    public class Players
    {
        public string Name { get; private set; }
        public char Disc { get; private set; }

        //Player Constructor
        public Players(string name, char disc)
        {
            Name = name;
            Disc = disc;
        }

        public void PlayerMakeMove(Board board)
        {
            bool validMove = false;

            while (!validMove)
            {
                Console.WriteLine($"{Name}'s Turn ({Disc})");
                Console.Write("Please enter your move (1-7): ");
                string input = Console.ReadLine();

                if(int.TryParse(input, out int column))
                {
                    validMove = board.DroppingXO(column - 1, Disc);
                    if (!validMove)
                    {
                        Console.WriteLine("Invalid move! Column is full.");

                    }
                } else
                {
                    Console.Write("Invalid input. Please enter your move (1-7): ");
                }

            }
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(); // new board

            //Creating Players
            Players player1 = new Players("Player 1", 'X');
            Players player2 = new Players("Player 2", 'O');

            Players currentPlayer = player1; //current player move

            while (true)
            {
                board.DisplayBoard();
                currentPlayer.PlayerMakeMove(board);

                if(currentPlayer == player1)
                {
                    currentPlayer = player2;
                } else
                {
                    currentPlayer = player1;
                }
            }
        }
    }
}
