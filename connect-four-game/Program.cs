using System;
using System.Text.RegularExpressions;
using connect_four_game;

namespace connect_four_game
{
    // Board Class
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
                    grid[r, c] = '*';
                }
            }
        }

        //Display Board
        public void DisplayBoard()
        {
            for (int r = 0; r < Rows; r++)
            {
                Console.Write("|");
                for (int c = 0; c < Columns; c++)
                {
                    Console.Write(" " + grid[r, c] + " ");
                }
                Console.WriteLine("|");
            }

            //Print numbers 1 - 7
            Console.Write(" ");
            for (int n = 1; n <= Columns; n++)
            {
                Console.Write(" " + n + " ");
            }
            Console.WriteLine("\n");
        }

        //Dropping X and O
        public bool DroppingXO(int column, char disc)
        {
            if (column < 0 || column >= Columns)
            {
                return false;
            }

            for (int row = Rows - 1; row >= 0; row--)
            {
                if (grid[row, column] == '*')
                {
                    grid[row, column] = disc;
                    return true;
                }
            }

            return false;
        }

        //Check for wins horizontal, vertical, diagonal left and right.
        public bool CheckWin(char disc)
        {
            //Horizontal
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == disc && grid[r, c + 1] == disc &&
                        grid[r, c + 2] == disc && grid[r, c + 3] == disc)
                        return true;

            //Vertical
            for (int c = 0; c < Columns; c++)
                for (int r = 0; r <= Rows - 4; r++)
                    if (grid[r, c] == disc && grid[r + 1, c] == disc &&
                        grid[r + 2, c] == disc && grid[r + 3, c] == disc)
                        return true;
            //Diagonal /
            for (int r = 3; r < Rows; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == disc && grid[r - 1, c + 1] == disc &&
                        grid[r - 2, c + 2] == disc && grid[r - 3, c + 3] == disc)
                        return true;
            //Diagonal \
            for (int r = 0; r <= Rows - 4; r++)
                for (int c = 0; c <= Columns - 4; c++)
                    if (grid[r, c] == disc && grid[r + 1, c + 1] == disc &&
                        grid[r + 2, c + 2] == disc && grid[r + 3, c + 3] == disc)
                        return true;

            return false;
        }

        //Check if the board is full
        public bool IsFull()
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[0, c] == '*')
                {
                    return false;
                }
            }
            return true;
        }
    } //End of Board Class

    // Player Abstract Class
    public abstract class Players
    {
        public string Name { get; private set; }
        public char Disc { get; private set; }

        public Players(string name, char disc)
        {
            Name = name;
            Disc = disc;
        }

        public abstract void PlayerMakeMove(Board board);
    } // End of Player Abstract Class

    // Human Player Class
    public class HumanPlayer : Players
    {
        public HumanPlayer(string name, char disc) : base(name, disc) { }

        public override void PlayerMakeMove(Board board)
        {
            bool validMove = false;

            while (!validMove)
            {
                Console.WriteLine($"{Name}'s Turn '{Disc}'");
                Console.Write("Enter move (1-7): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int column))
                {
                    if (column >= 1 && column <= 7)
                    {
                        validMove = board.DroppingXO(column - 1, Disc);
                        if (!validMove)
                        {
                            Console.WriteLine("\nColumn full. Try again.");
                            board.DisplayBoard();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid turn. Choose between 1-7.");
                        board.DisplayBoard();
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input.");
                    board.DisplayBoard();

                }
            }
        }
    } //End of Human Player Class

    //AI Player Class
    public class AIPlayer : Players
    {
        private static Random rand = new Random();

        public AIPlayer(string name, char disc) : base(name, disc)
        {
        }

        public override void PlayerMakeMove(Board board)
        {
            Console.WriteLine($"{Name}'s Turn '{Disc}' (AI)");
            int column;
            do
            {
                column = rand.Next(0, board.Columns);
            }
            while (!board.DroppingXO(column, Disc));
            Console.WriteLine($"AI chose column {column + 1}\n");
        }
    } //End of AI Player Class

    // GameManager Class
    public class GameManager
    {
        public class GameResult
        {
            public string Winner { get; set; }
            public string Loser { get; set; }

            public override string ToString()
            {
                return $"Winner : {Winner} \n" +
                    $"Loser: {Loser}";
            }
        }
        private List<GameResult> _gameHistory = new List<GameResult>();

        private Players _player1;
        private Players _player2;
        private Players _currentPlayer;
        private Board _board;

        public void Start()
        {
            Console.WriteLine("\nWelcome to Connect Four!");
            string selectInput;

            do
            {
                Console.WriteLine("Player select: ");
                Console.WriteLine("1. 1 Player (Human vs AI)");
                Console.WriteLine("2. 2 Players (Human vs Human)");
                Console.Write("Choose an option: ");
                selectInput = Console.ReadLine();

                if (selectInput != "1" && selectInput != "2")
                {
                    Console.WriteLine("\nInvalid input. Please select between 1 or 2.");
                    Console.WriteLine();
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
                    $"1 upper case \n" +
                    $"1 lower case \n" +
                    $"and 1 digit");
                Console.WriteLine();
                Console.Write("Name: ");
                name = Console.ReadLine();

                if (!regex.IsMatch(name))
                {
                    Console.WriteLine("Invalid format. Try again.");
                }
                else
                {
                    Console.WriteLine($"{label} Name: {name}");
                }
            } while (!regex.IsMatch(name));

            return name;
        }

        //Game Loop to handle player moves, define winner/loser/tie checking
        private void RunGameLoop()
        {
            while (true)
            {
                Console.WriteLine();
                _board.DisplayBoard();
                _currentPlayer.PlayerMakeMove(_board);
                
                if (_board.CheckWin(_currentPlayer.Disc))
                {
                    _board.DisplayBoard();
                    Console.WriteLine($"{_currentPlayer.Name} wins!");

                    //Save game result
                    string loser;
                    if (_currentPlayer == _player1)
                    {
                        loser = _player2.Name;
                    }
                    else
                    {
                        loser = _player1.Name;
                    }

                    _gameHistory.Add(new GameResult
                    {
                        Winner = _currentPlayer.Name,
                        Loser = loser
                    });

                    break;
                }

                if (_board.IsFull())
                {
                    _board.DisplayBoard();
                    Console.WriteLine("It's a tie!");

                    _gameHistory.Add(new GameResult
                    {
                        Winner = "None",
                        Loser = "None"
                    });

                    break;

                }

                SwitchPlayer();
            }

            AfterGame();
        }

        //Show History
        private void ShowHistory()
        {
            Console.WriteLine("\n---Past Games---\n");

            int count = 1;
            foreach (var game in _gameHistory)
            {
                Console.WriteLine($"Game {count++}");
                //\n" +
                //$"Winner : {game.Winner} \n" +
                //$"Loser: {game.Loser}");
                Console.WriteLine(game);
                Console.WriteLine("\n------------");
            }

            Console.WriteLine();

            AfterGame();
        }

        //Ask what to do after a game ends
        private void AfterGame()
        {
            while (true)
            {
                Console.WriteLine("\n1. Play Again");
                Console.WriteLine("2. Show Past Games");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Start();
                    break;

                }
                else if (choice == "2")
                {
                    ShowHistory();

                }
                else if (choice == "3")
                {
                    Console.WriteLine("Thank you for playing!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
        }

        //Switching between players
        private void SwitchPlayer()
        {
            if (_currentPlayer == _player1)
            {
                _currentPlayer = _player2;
            }
            else
            {
                _currentPlayer = _player1;
            }
        }
    } //End of Game Manager Class

    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager game = new GameManager();
            game.Start();
        }
    }
}