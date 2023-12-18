using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace BattleBoats
{
    internal class Program
    {
        static void Main(string[] args)
        {


            int gameNum = 0;
            string winningPerson = "not yet";//
            string oddPlayer = "player";
            string evenPlayer = "computer";
            char[,] playerGrid = new char[8, 8];
            char[,] hitsAndMissesGrid = new char[8, 8];
            char[,] computerGrid = new char[8, 8];
            int payerSunkBoats = 0;
            int computerSunkBoats = 0;

            playerGrid = IntialiseComputerGrid(playerGrid);// set random boats for testing
            computerGrid = IntialiseComputerGrid(computerGrid);
            hitsAndMissesGrid = SetBlankGrid(hitsAndMissesGrid);

            WriteGame(gameNum, oddPlayer, evenPlayer, playerGrid, hitsAndMissesGrid, computerGrid, payerSunkBoats, computerSunkBoats);
            LoadGame();


            //Console.WriteLine(ownGrid[arrayCoodinateas[0],arrayCoodinateas[1]]);
        }

        static string DisplayMenu()
        {
            Console.WriteLine("Type the number for the option you want: \n" +
                "1. Show the instructions\n" +
                "2. Start a new game\n" +
                "3. Load game from a file\n" +
                "4. Quit");
           string mode = Console.ReadLine();
            if (mode == "1")
            {
                DisplayInstructions();
            }
            if (mode == "2")
            {
                Game();
            }
            if( mode == "3")
            {
                //LoadGame()
            }
            if(mode == "4")
            {
                return "Wants to quit";
            }
            return "Wants to continue";
        }
        


        static void DisplayInstructions()
        {
            Console.WriteLine("This is the game of battle boats where you have a 8x8 grid where you can place your boats anywhere within this grid.\n " +
                "Your opponent is the computer, and they have a similar gird with boats in unknown locations.\n" +
                " Your aim is to destroy your opponent’s boats before they destroy yours taking turns to guess different locations to send missiles.\n" +
                " On your grid your boats are marked as \'B\', boats that have been hit are marked as \'D\'and places where the computer has fired missiles but missed are marked as \'M\'\n" +
                "On your \'hits and misses\' grid boats you have sucsesfully hit are marked as \'H\' and previous guesses where you have missed are marked as \'M\' \n " +
                "Select New game to start playing!");
        }

        static void LoadGame()
        {
            int gameNum = 0;
            string oddPlayer = "";
            string evenPlayer = "";
            int playerSunkBoats = 0;
            int computerSunkBoats = 0;
            char[,] playerGrid = new char[8, 8];
            char[,] hitsAndMissesGrid = new char[8, 8];
            char[,] computerGrid = new char[8, 8];
            char[][,] grids = { playerGrid, hitsAndMissesGrid, computerGrid };

            string filePath = "savedGame.txt";
            using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open)))
            {
                
                gameNum = Convert.ToInt32(reader.ReadLine());
                oddPlayer = reader.ReadLine();
                evenPlayer = reader.ReadLine();
                playerSunkBoats = Convert.ToInt32(reader.ReadLine());
                computerSunkBoats = Convert.ToInt32(reader.ReadLine());
                for (int f = 0; f < grids.Length; f++)
                {
                    for (int i = 0; i < grids[f].GetLength(0); i++)
                    {
                        string line = reader.ReadLine();
                        Console.WriteLine(line);//
                        for (int j = 0; j < grids[f].GetLength(1); j++)
                        {
                            grids[f][i, j] = line[j];
                        }

                    }
                    reader.ReadLine(); // skip a line . should I keep it for readability?
                }

            }
            Console.WriteLine($"game num {gameNum}");
            Console.WriteLine($"even player {oddPlayer}");
            Console.WriteLine($"even player {evenPlayer}");
            DisplayGrid(playerGrid);
            DisplayGrid(hitsAndMissesGrid);
            DisplayGrid(computerGrid);
        }

        static void WriteGame(int gameNum,string oddPlayer,string evenPlayer, char[,] playerGrid , char[,] hitsAndMissesGrid , char[,] computerGrid,int playerSunkBoats,int computerSunkBoats)
        {
            string filePath = "savedGame.txt";
            // should probably ask for the file path?
            using (StreamWriter writer = new StreamWriter(File.Open(filePath,FileMode.Create)))
            {
                writer.WriteLine(gameNum);
                writer.WriteLine(oddPlayer);
                writer.WriteLine(evenPlayer);
                writer.WriteLine(playerSunkBoats);
                writer.WriteLine(computerSunkBoats);
                for (int i = 0;i<playerGrid.GetLength(0);i++)
                {
                    for (int j = 0; j < playerGrid.GetLength(1); j++)
                    {
                        writer.Write(playerGrid[i,j]);
                    }
                    writer.WriteLine();
                }
                writer.WriteLine();
                for (int i = 0; i < hitsAndMissesGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < hitsAndMissesGrid.GetLength(1); j++)
                    {
                        writer.Write(hitsAndMissesGrid[i, j]);
                    }
                    writer.WriteLine();
                }
                writer.WriteLine();
                for (int i = 0; i < computerGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < computerGrid.GetLength(1); j++)
                    {
                        writer.Write(computerGrid[i, j]);
                    }
                    writer.WriteLine();
                }
            }
        }
        static void Game()
        {
            int gameNum = 0;
            string oddPlayer = "player";
            string evenPlayer = "computer";
            int payerSunkBoats = 0;
            int computerSunkBoats = 0;
            char[,] playerGrid = new char[8, 8];
            char[,] hitsAndMissesGrid = new char[8, 8];
            char[,] computerGrid = new char[8, 8];

            string winningPerson = "not yet";//
            playerGrid = IntialisePlayerGrid(playerGrid);
            computerGrid = IntialiseComputerGrid(computerGrid);
            hitsAndMissesGrid = SetBlankGrid(hitsAndMissesGrid);
            // set the grids

            while (winningPerson != "player" && winningPerson != "computer")
            {

                string playerWon = "";
                string computerWon = "";
                gameNum++;
                
                if (gameNum % 2 == 0) 
                    // even turn number
                {
                    if (evenPlayer == "player")
                    {
                        playerWon = PlayerTurn(ref computerGrid,ref hitsAndMissesGrid, ref payerSunkBoats);
                    }
                    else
                    {
                        Console.WriteLine("Computer turn ");
                        computerWon = ComputerTurn(ref computerGrid, ref playerGrid, ref computerSunkBoats);
                    }
                }
                else
                {
                    if (oddPlayer == "player")
                    {
                        playerWon = PlayerTurn(ref computerGrid, ref hitsAndMissesGrid, ref payerSunkBoats);
                    }
                    else
                    {
                        Console.WriteLine("Computer turn ");
                        computerWon = ComputerTurn(ref computerGrid, ref playerGrid, ref computerSunkBoats);
                    }
                }

                if (playerWon == "yes" || computerWon == "yes" )
                    
                {
                    (oddPlayer, evenPlayer) = (evenPlayer, oddPlayer);
                }
                //swap them using tuples
                if (computerSunkBoats >= 5)
                {
                    winningPerson = "computer";
                }
                if (payerSunkBoats >= 5)
                {
                    winningPerson = "player";
                }

            }

            Console.WriteLine($"the winner was {winningPerson}! who won in {gameNum} turns");
            /// test this asap
        }

        static string PlayerTurn(ref char[,] computerGrid, ref char[,] hitsAndMissesGrid, ref int sunkBoats)
        {
            int[] attackNums = {-1,-1};
            string playerwon = "no";
            

            Console.WriteLine("Player turn! here are your hits and misses:");
            DisplayGrid(hitsAndMissesGrid);
            while (attackNums[0] == -1 ) 
                // -1 means an invalid coordinate
            {

                Console.WriteLine("Enter the coordinates of where you want to send your missile (in the form numberLetter):");
                attackNums = GetCoodrinates(Console.ReadLine());

                if (attackNums[0] != -1 && hitsAndMissesGrid[attackNums[1], attackNums[0]] != '*')
                {
                    Console.WriteLine("You've already guessed that square!");
                    attackNums[0] = -1;
                    attackNums[1] = -1;
                }
            }

            if (computerGrid[attackNums[1],attackNums[0]] == 'B')
            {
                hitsAndMissesGrid[attackNums[1], attackNums[0]] = 'H';
                Console.WriteLine("You hit a computer's boat! (and as it was only one square it sank immediatley)");
                Console.WriteLine("You get an extra go !");
                sunkBoats++;
                playerwon = "yes";


            }
            else
            {
                hitsAndMissesGrid[attackNums[1], attackNums[0]] = 'M';
                Console.WriteLine("You missed!");
            }

            return playerwon;

        }

        static string ComputerTurn(ref char[,] computerGrid, ref char[,] playerGrid, ref int sunkBoats)
        {
            int[] attackNums = new int[2];
            string computerWon = "no";
            Random rand = new Random();
            Console.WriteLine("Computer turn! :");
            do
            {
                attackNums[1] = rand.Next(computerGrid.GetLength(0));
                attackNums[0] = rand.Next(computerGrid.GetLength(1));
            }
            while (playerGrid[attackNums[1], attackNums[0]] == 'D' || playerGrid[attackNums[1], attackNums[0]] == 'M');

            if (playerGrid[attackNums[1], attackNums[0]] == 'B')
            {
                playerGrid[attackNums[1], attackNums[0]] = 'D';
                Console.WriteLine("the computer hit one of your boats! (and as it was only one square it sank immediatley)");
                DisplayGrid(playerGrid);
                Console.WriteLine("The computer gets and extra go!");
                sunkBoats++;
                computerWon = "yes";


            }
            else
            {
                playerGrid[attackNums[1], attackNums[0]] = 'M';
                Console.WriteLine("the computer sent a missile but it missed!");
                DisplayGrid(playerGrid);
            }

            return computerWon;
        }

        static void DisplayGrid(char[,] grid)
        {
            char startingLetter = 'A';
            // sets the letter to start at
            Console.Write("   ");
            for(int f  = 0; f < grid.GetLength(1); f++)
            {
                Console.Write($"{f} ");
                // print a num for each collum you have
            }
            Console.Write("\n");
            for (int i  = 0; i < grid.GetLength(0); i++)
            {
                Console.Write($"{Convert.ToChar(startingLetter+i)}. ");
                // at the start of each line add the line index to the starting letter to get the current letter
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write($"{grid[i, j]} ");
                    // write each row on the same line
                }
                Console.Write("\n");
            }
        }

        static char[,] IntialisePlayerGrid(char[,] playerGrid)
      
        {
            SetBlankGrid(playerGrid);
            // if I add different types of boats I am going to have to significantly change this
            int numberOfBoats = 5;

            DisplayGrid(playerGrid);

            for(int i = 1; i <= numberOfBoats; i++)
            {
                int[] boatCoordinates = new int[2];
                int[] inputCoordinates = { -1, -1 };
                while (inputCoordinates[0] == -1 || playerGrid[inputCoordinates[1],inputCoordinates[0]] == 'B') 
                // -1 if invalid input coordinates and if they attempt to place one on top of a boat
                {
                    Console.WriteLine($"enter your coordinates in the form: NumberLetter for the placment of boat number {i} (only place boats in empty spaces):");
                    inputCoordinates = GetCoodrinates(Console.ReadLine());
                }
                playerGrid[inputCoordinates[1],inputCoordinates[0]] = 'B';
                Console.WriteLine("Boat placed!");
                DisplayGrid(playerGrid);
            }
            

            return playerGrid;
        }


        static char[,] IntialiseComputerGrid(char[,] computerGrid)
        //where do I want to  initialise the computer grid?
        {
            SetBlankGrid(computerGrid);
            // if I add different types of boats I am going to have to significantly change this
            int numberOfBoats = 5;

            Random rand = new Random();

            for (int i = 1; i <= numberOfBoats; i++)
            {
                int[] Coordinates = new int[2];
                do 
                {
                    Coordinates[1] = rand.Next(computerGrid.GetLength(0));
                    Coordinates[0] = rand.Next(computerGrid.GetLength(1));
                }
                while (computerGrid[Coordinates[1], Coordinates[0]] == 'B') ;


                computerGrid[Coordinates[1], Coordinates[0]] = 'B';
                Console.WriteLine("Boat placed! on computer");
                DisplayGrid(computerGrid);
            }

  
            return computerGrid;
        }

        static char[,] SetBlankGrid(char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[i, y] = '*';
                }
            }
            return grid;
        }


        static int[] GetCoodrinates(string coordinates)
        {
            // gridsize? -- change multiple things num cannot be larger than 9
            int[] arrayCooridnates = new int[2];
            int gridSize = 8;
            coordinates = coordinates.ToUpper();
            if (coordinates == "" || coordinates.Length > 2 || coordinates[0]<'0'||coordinates[0]-'0' >= gridSize || coordinates[1] >= ('A' + gridSize )|| coordinates[1] < ('A'))
                // 
            {
                arrayCooridnates[0] = -1;
                arrayCooridnates[1] = -1;
                
            }
            else
            {
                arrayCooridnates[0] = Convert.ToInt32(coordinates[0].ToString());

                arrayCooridnates[1] = Convert.ToInt32((coordinates[1] - 'A').ToString());
            }
            return arrayCooridnates;
        }
    }
}