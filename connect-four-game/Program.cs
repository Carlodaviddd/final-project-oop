using System;
using System.Text.RegularExpressions;
using connect_four_game;

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
        // checks for wins in horizontal, vertical, diagonal left and right. 
      public bool CheckWin(char disc)
        {
            // Horizontal
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == disc && grid[r, c + 1] == disc &&
                        grid[r, c + 2] == disc && grid[r, c + 3] == disc)
                        return true;

            // Vertical
            for (int c = 0; c < Columns; c++)
                for (int r = 0; r <= Rows - 4; r++)
                    if (grid[r, c] == disc && grid[r + 1, c] == disc &&
                        grid[r + 2, c] == disc && grid[r + 3, c] == disc)
                        return true;

            // Diagonal /
            for (int r = 3; r < Rows; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == disc && grid[r - 1, c + 1] == disc &&
                        grid[r - 2, c + 2] == disc && grid[r - 3, c + 3] == disc)
                        return true;

            // Diagonal \
            for (int r = 0; r <= Rows - 4; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == disc && grid[r + 1, c + 1] == disc &&
                        grid[r + 2, c + 2] == disc && grid[r + 3, c + 3] == disc)
                        return true;

            return false;
        }

    // checks of columns are full. 
        public bool IsFull()
        {
            for (int c = 0; c < Columns; c++)
                if (grid[0, c] == '#') return false;
            return true;
        }
    }

    //Player Abstract Class
    public abstract class Players
    {
        public string Name { get; private set; }
        public char Disc { get; private set; }

        //Player Constructor
        public Players(string name, char disc)
        {
            Name = name;
            Disc = disc;
        }

        public abstract void PlayerMakeMove(Board board);
        }

    //Human vs Human
    public class HumanPlayer : Players
    {
        public HumanPlayer(string name, char disc) : base(name , disc) { }
        
        public override void PlayerMakeMove(Board board)
        {
            bool validMove = false;

            while (!validMove)
            {
                Console.WriteLine($"{Name}'s Turn '{Disc}'");
                Console.Write("Please enter your move (1-7): ");
                string input = Console.ReadLine();

                if(int.TryParse(input, out int column))
                {
                    if(column < 1 || column > 7)
                    {
                        Console.WriteLine("Invalid move! Please enter a number between 1 and 7");
                        board.DisplayBoard();
                        continue;
                    }

                    validMove = board.DroppingXO(column - 1, Disc);
                    if (!validMove)
                    {
                        Console.WriteLine("Invalid Move! Column is full. Try Again. \n");
                        board.DisplayBoard();
                    }
                } else
                {
                    Console.Write("Invalid input. Please enter your move (1-7): ");
                    board.DisplayBoard();
                }
            }
        }
    }

    // AIPlayer
    public class AIPlayer : Players
    {
        private static Random rand = new Random();

        public AIPlayer(string name, char disc) : base(name, disc) { }

        public override void PlayerMakeMove(Board board)
        {
            Console.WriteLine($"{Name}'s Turn '{Disc}' (AI)");
            int column;
            do
            {
                column = rand.Next(0, board.Columns);
            } while (!board.DroppingXO(column, Disc));
            Console.WriteLine($"AI chose column {column + 1}");
        }
    }

    //GameManager Class
    public class GameManager
    {
        private Players _player1;
        private Players _player2;
        private Players _currentPlayer;
        private Board _board;

        public void Start()
        {
            _board = new Board();
            Console.WriteLine("Welcome to Connect Four!");

            string selectInput;

            do
            {
                Console.WriteLine("Player select: ");
                Console.WriteLine("1. 1 Player (Human vs AI)");
                Console.WriteLine("2. 2 Players (Human vs Human");
                Console.Write("Choose an option: ");
                selectInput = Console.ReadLine();

                if (selectInput != "1" && selectInput != "2")
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            } while (selectInput != "1" && selectInput != "2");

            _board = new Board();
            _player1 = new HumanPlayer(GetValidName("Player 1"), 'X');

            if (selectInput == "1")
            {
                 _player2 = new AIPlayer("AI", 'O');
            }
            else
            {
                 _player2 = new HumanPlayer(GetValidName("Player 2"), 'O');
            }

            _player1 = new HumanPlayer(GetValidName("Player 1"), 'X');
            _player2 = new HumanPlayer(GetValidName("Player 2"), 'O');
            _currentPlayer = _player1;

            RunGameLoop();
        }

        //Set Player's Name Format
        private string GetValidName(string label)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
            string name;

            do
            {
                Console.WriteLine();
                Console.WriteLine($"Enter {label} Name \n" +
                    $"Must Contain: \n" +
                    $"1 uppercase \n" +
                    $"1 lowercase \n" +
                    $"and 1 digit");
                Console.WriteLine();
                Console.Write("Name: ");
                name = Console.ReadLine();
                if (!regex.IsMatch(name))
                {
                    Console.WriteLine("Invalid name format. Try again.");
                } else
                {
                    Console.WriteLine($"{label} Name: {name} \n");
                }
            } while (!regex.IsMatch(name));

            return name;
        }

        

        private void RunGameLoop()
        {
            while (true)
            {
                _board.DisplayBoard();
                _currentPlayer.PlayerMakeMove(_board);
                SwitchPlayer();
            }
        }

        //Switching Players
        private void SwitchPlayer()
        {
            if(_currentPlayer == _player1)
            {
                _currentPlayer = _player2;
            } else
            {
                _currentPlayer = _player1;
            }
        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        GameManager game = new GameManager();
        game.Start();
    }
}
