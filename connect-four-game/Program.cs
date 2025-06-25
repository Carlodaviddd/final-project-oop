using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
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
            Console.Clear();
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

        public void AnimateDrop(int column, char disc)
{
    for (int row = 0; row < Rows; row++)
    {
        if (row > 0)
            grid[row - 1, column] = '*';  // Clear previous position

        // If bottom or next row is occupied, place disc here
        if (row == Rows - 1 || grid[row + 1, column] != '*')
        {
            grid[row, column] = disc;
            DisplayBoard();
            Thread.Sleep(200);  // Pause to show final position
            break;
        }
        else
        {
            grid[row, column] = disc;
            DisplayBoard();
            Thread.Sleep(200);
        }
    }
}


        //Dropping X and O
        public bool DroppingXO(int column, char disc)
        {
       if (column < 0 || column >= Columns)
        return false;

    // Check if column full
    if (grid[0, column] != '*')
        return false;

    AnimateDrop(column, disc);
    return true;
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
        // Returns row index where disc would fall in column; -1 if full
        public int GetAvailableRow(int column)
        {
            for (int row = Rows - 1; row >= 0; row--)
                if (grid[row, column] == '*')
                    return row;
            return -1;
        }
       public char GetCell(int row, int column)
{
    return grid[row, column];
}

public void SetCell(int row, int column, char value)
   {
    grid[row, column] = value;
   }
} //End of Board Class


    // GameManager Class (controls game flow)
    public class GameManager
    {
        //Store past games
        public class GameResult
        {
            public string Winner { get; set; }
            public string Loser { get; set; }
            public TimeSpan Duration { get; set; }

            public override string ToString()
            {
                return $"Winner : {Winner} \n" +
                    $"Loser: {Loser} \n" +  
                    $"Total Game Duration: {Duration.Minutes} minutes {Duration.Seconds} seconds";
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

            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. 1 Player (Human vs AI)");
                Console.WriteLine("2. 2 Players (Human vs Human)");
                Console.WriteLine("3. Game Instructions");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");
                string selectInput = Console.ReadLine();

                if (selectInput == "1")
                {
                    _board = new Board();
                    _player1 = new HumanPlayer(GetValidName("Player 1"), 'X');
                    _player2 = new AIPlayer("AI", 'O');

                    Random rand = new Random();
                    _currentPlayer = rand.Next(2) == 0 ? _player1 : _player2;
                    Console.WriteLine($"\n{_currentPlayer.Name} will start first.\n");

                    RunGameLoop();
                }
                else if (selectInput == "2")
                {
                    _board = new Board();
                    _player1 = new HumanPlayer(GetValidName("Player 1"), 'X');
                    _player2 = new HumanPlayer(GetValidName("Player 2"), 'O');
                    _currentPlayer = _player1;

                    RunGameLoop();
                }
                else if (selectInput == "3")
                {
                    ShowInstructions();
                }
                else if (selectInput == "4")
                {
                    Console.WriteLine("\nThank you for playing!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please select a valid option.\n");
                }
            }
        }

        // Game Instructions
        private void ShowInstructions()
{
    Console.WriteLine("\n--- GAME INSTRUCTIONS ---\n");
    Console.WriteLine("Objective:");
    Console.WriteLine("Be the first player to connect 4 discs horizontally, vertically, or diagonally.\n");

    Console.WriteLine("How to Play:");
    Console.WriteLine("1. Select game mode: 1 Player (Human vs AI) or 2 Players (Human vs Human).");
    Console.WriteLine("2. Enter valid player names (must contain 1 uppercase, 1 lowercase, and 1 digit).");
    Console.WriteLine("3. On your turn, enter a column number (1-7) to drop your disc.");
    Console.WriteLine("4. Human players have 30 seconds per turn.");
    Console.WriteLine("5. AI makes its move automatically.\n");

    Console.WriteLine("Winning:");
    Console.WriteLine("First to connect 4 discs wins. If the board fills with no winner, it's a tie.\n");

    Console.WriteLine("After Game:");
    Console.WriteLine("You can choose to play again, view past games, or exit.\n");

    Console.WriteLine("----------------------------\n");
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

        //Game Loop to handle player moves, define winner/loser/tie
        private void RunGameLoop()
        {
            var gameStopwatch = new Stopwatch();
            gameStopwatch.Start();
            while (true)
            {
                Console.WriteLine();
                _board.DisplayBoard();
                _currentPlayer.PlayerMakeMove(_board); //player action
                
                //Check win
                if (_board.CheckWin(_currentPlayer.Disc))
                {
                    _board.DisplayBoard();
                    Console.WriteLine($"{_currentPlayer.Name} wins!");
                    gameStopwatch.Stop();

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
                        Loser = loser,
                        Duration = gameStopwatch.Elapsed    
                    });

                    break;
                }

                //Check tie
                if (_board.IsFull())
                {
                    _board.DisplayBoard();
                    Console.WriteLine("It's a tie!");
                    gameStopwatch.Stop();

                    _gameHistory.Add(new GameResult
                    {
                        Winner = "None",
                        Loser = "None",
                        Duration = gameStopwatch.Elapsed     
                    });

                    break;

                }

                SwitchPlayer(); //switching players
            }
          gameStopwatch.Stop();
          Console.WriteLine($"\nTotal Game Duration: {gameStopwatch.Elapsed.Minutes} minutes {gameStopwatch.Elapsed.Seconds} seconds\n");
            AfterGame(); //post game
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
     int timeLimit = 30 * 1000; // 30 seconds

     while (!validMove)
     {
         Console.WriteLine($"{Name}'s Turn '{Disc}' - You have 30 seconds to make a move.");
         Console.Write("Enter move (1-7): ");
         string input = ReadLineWithTimeout(timeLimit);

         if (input == null)
         {
             Console.WriteLine("\nTime's up! You missed your turn.");
             break;
         }

         if (int.TryParse(input, out int column))
         {
             if (column >= 1 && column <= 7)
             {
                 validMove = board.DroppingXO(column - 1, Disc);
                 if (validMove)
                 {
                     Console.WriteLine($"{Name} chose column {column}");
                     Thread.Sleep(1500);
                 }
                 else
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
        private string ReadLineWithTimeout(int timeLimit)
    {
        Task<string> task = Task.Run(() => Console.ReadLine());
        bool completedInTime = task.Wait(timeLimit);

        if (completedInTime)
        {
            return task.Result;
        }
        else
        {
            return null;
        }
    }
} //End of Human Player Class

    
    //AI Player Class
  // AI Player Class
public class AIPlayer : Players
{
    private static Random rand = new Random();

    public AIPlayer(string name, char disc) : base(name, disc) { }

    public override void PlayerMakeMove(Board board)
    {
        Console.WriteLine($"{Name}'s Turn '{Disc}' (AI)");

        char opponentDisc = Disc == 'X' ? 'O' : 'X';
        int column = ChooseBestMove(board, Disc, opponentDisc);

        board.DroppingXO(column, Disc);
        Console.WriteLine($"AI chose column {column + 1}\n");
        Thread.Sleep(1500);
    }

    private int ChooseBestMove(Board board, char aiDisc, char opponentDisc)
    {
        // 1. Try to win
        for (int col = 0; col < board.Columns; col++)
        {
            int row = board.GetAvailableRow(col);
            if (row == -1) continue;

            board.SetCell(row, col, aiDisc);
            bool canWin = board.CheckWin(aiDisc);
            board.SetCell(row, col, '*');

            if (canWin)
                return col;
        }

        // 2. Block opponent's winning move
        for (int col = 0; col < board.Columns; col++)
        {
            int row = board.GetAvailableRow(col);
            if (row == -1) continue;

            board.SetCell(row, col, opponentDisc);
            bool opponentCanWin = board.CheckWin(opponentDisc);
            board.SetCell(row, col, '*');

            if (opponentCanWin)
                return col;
        }

        // 3. Pick center column if possible
        int center = board.Columns / 2;
        if (board.GetAvailableRow(center) != -1)
            return center;

        // 4. Pick random valid column
        List<int> validCols = new List<int>();
        for (int col = 0; col < board.Columns; col++)
            if (board.GetAvailableRow(col) != -1)
                validCols.Add(col);

        return validCols[rand.Next(validCols.Count)];
    }
} //End of AI Player Class

    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager game = new GameManager();
            game.Start();
        }
    }
}
