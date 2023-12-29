﻿using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace BattleBoats
{
    internal class Program
    {
        static void Main(string[] args)
        {


            string mode = "Wants to continue";
            while (mode == "Wants to continue")
            {
                mode = DisplayMenu();
            }

        }

        static string DisplayMenu()
        {
            Console.WriteLine(
                "Welcome to:" +
                "\r\n______       _   _   _       ______             _       \r\n| ___ \\     | | | | | |      | ___ \\           | |      \r\n| |_/ / __ _| |_| |_| | ___  | |_/ / ___   __ _| |_ ___ \r\n| ___ \\/ _` | __| __| |/ _ \\ | ___ \\/ _ \\ / _` | __/ __|\r\n| |_/ / (_| | |_| |_| |  __/ | |_/ / (_) | (_| | |_\\__ \\\r\n\\____/ \\__,_|\\__|\\__|_|\\___| \\____/ \\___/ \\__,_|\\__|___/\r\n                                                        \r\n                                                        \r\n" +
                "Type the number for the option you want: \n" +
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
                NewGame();
            }
            if( mode == "3")
            {
                LoadGame();
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
                        for (int j = 0; j < grids[f].GetLength(1); j++)
                        {
                            grids[f][i, j] = line[j];
                        }

                    }
                    reader.ReadLine(); // reads individual line between grids
                }

            }
            Console.WriteLine("game loaded!");
            Thread.Sleep(300);
            Game(gameNum, oddPlayer, evenPlayer, playerSunkBoats, computerSunkBoats, playerGrid, hitsAndMissesGrid, computerGrid);

        }

        static void WriteGame(int gameNum, string oddPlayer, string evenPlayer, char[,] playerGrid, char[,] hitsAndMissesGrid, char[,] computerGrid, int playerSunkBoats, int computerSunkBoats)
        {
            string filePath = "savedGame.txt";
            char[][,] grids = { playerGrid, hitsAndMissesGrid, computerGrid };
            // should probably ask for the file path?
            using (StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.WriteLine(gameNum);
                writer.WriteLine(oddPlayer);
                writer.WriteLine(evenPlayer);
                writer.WriteLine(playerSunkBoats);
                writer.WriteLine(computerSunkBoats);
                for (int f = 0; f < grids.Length; f++)
                {
                    for (int i = 0; i < grids[f].GetLength(0); i++)
                    {
                        for (int j = 0; j < grids[f].GetLength(1); j++)
                        {
                            writer.Write(grids[f][i, j]);
                        }
                        writer.WriteLine();// new line
                    }
                    writer.WriteLine();//new line
                }

            }
            Console.WriteLine($"Game wirtten to file! as {filePath}");

        }


        static void NewGame()
        {
            Console.Clear(); // get rid of the menu
            int gameNum = 0;
            string oddPlayer = "player";
            string evenPlayer = "computer";
            int playerSunkBoats = 0;
            int computerSunkBoats = 0;
            char[,] playerGrid = new char[8, 8];
            char[,] hitsAndMissesGrid = new char[8, 8];
            char[,] computerGrid = new char[8, 8];
            computerGrid = IntialiseComputerGrid(computerGrid);
            hitsAndMissesGrid = SetBlankGrid(hitsAndMissesGrid);
            playerGrid = IntialisePlayerGrid(playerGrid, hitsAndMissesGrid, computerGrid);
            // sets the grids

            Game( gameNum,oddPlayer, evenPlayer, playerSunkBoats, computerSunkBoats, playerGrid,  hitsAndMissesGrid,computerGrid);

        }
        static void Game(int gameNum, string oddPlayer , string evenPlayer, int playerSunkBoats , int computerSunkBoats, char[,] playerGrid , char[,] hitsAndMissesGrid, char[,] computerGrid)
        {

            string gameState = "not yet";//
            string gameSaved = "not yet";

            while (gameState != "you" && gameState != "the computer" && gameState != "saved")
            {

                string playerWon = "";
                string computerWon = "";
                gameNum++;
                
                if (gameNum % 2 == 0) 
                    // even turn number
                {
                    if (evenPlayer == "player")
                    {
                        playerWon = PlayerTurn(ref computerGrid,ref hitsAndMissesGrid, ref playerSunkBoats, ref playerGrid);
                    }
                    else
                    {
                        computerWon = ComputerTurn(ref computerGrid, ref playerGrid, ref computerSunkBoats, ref hitsAndMissesGrid);
                    }
                }
                else
                {
                    if (oddPlayer == "player")
                    {
                        playerWon = PlayerTurn(ref computerGrid, ref hitsAndMissesGrid, ref playerSunkBoats, ref playerGrid);
                    }
                    else
                    {
                        computerWon = ComputerTurn(ref computerGrid, ref playerGrid, ref computerSunkBoats, ref hitsAndMissesGrid);
                        // is there a reason I am passing by reference this is kind of a messs
                    }
                }

                if (playerWon == "save")
                {
                    WriteGame(gameNum, oddPlayer, evenPlayer, playerGrid, hitsAndMissesGrid, computerGrid, playerSunkBoats, computerSunkBoats);
                    gameState = "saved";

                }

                if (playerWon == "yes" || computerWon == "yes" )
                    
                {
                    (oddPlayer, evenPlayer) = (evenPlayer, oddPlayer);
                }
                //swap them using tuples
                if (computerSunkBoats >= 5)
                {
                    gameState = "the computer";
                }
                if (playerSunkBoats >= 5)
                {
                    gameState = "you";
                }

            }
            if (gameState != "saved")
            {
                Console.WriteLine($"the winner was {gameState}! who won in {gameNum} turns");
            }
        }

        static string PlayerTurn(ref char[,] computerGrid, ref char[,] hitsAndMissesGrid, ref int sunkBoats, ref char[,] playerGrid)
            //add player gird
        {
            int[] attackNums = {-1,-1};
            string playerwon = "no";

            while (attackNums[0] == -1 ) 
                // -1 means an invalid coordinate
            {
                DisplayGrids(playerGrid, hitsAndMissesGrid, computerGrid);
                Console.WriteLine("Your turn!\n");

                Console.WriteLine("Enter the coordinates of where you want to send your missile (in the form numberLetter) or type save to save to a file:");
                string input = Console.ReadLine();

                if(input == "save")
                {
                    return new string("save");
                }

                attackNums = GetCoodrinates(input);

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
                Thread.Sleep(3000);
                sunkBoats++;
                playerwon = "yes";


            }
            else
            {
                hitsAndMissesGrid[attackNums[1], attackNums[0]] = 'M';
                Console.WriteLine("You missed!");
                Thread.Sleep(3000);
            }

            return playerwon;

        }

        static string ComputerTurn(ref char[,] computerGrid, ref char[,] playerGrid, ref int sunkBoats, ref char[,] hitsAndMissesGrid)
            // add hits and misses grid
        {
            int[] attackNums = new int[2];
            string computerWon = "no";
            Random rand = new Random();
            DisplayGrids(playerGrid, hitsAndMissesGrid, computerGrid);
            Console.Write("Computer thinking");
            for (int i = 0; i <= 3; i++)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
            Console.Write("\n");
            do
            {
                attackNums[1] = rand.Next(computerGrid.GetLength(0));
                attackNums[0] = rand.Next(computerGrid.GetLength(1));
            }
            while (playerGrid[attackNums[1], attackNums[0]] == 'D' || playerGrid[attackNums[1], attackNums[0]] == 'M');

            if (playerGrid[attackNums[1], attackNums[0]] == 'B')
            {
                playerGrid[attackNums[1], attackNums[0]] = 'D';
                DisplayGrids(playerGrid, hitsAndMissesGrid, computerGrid);
                Console.WriteLine("the computer hit one of your boats! (and as it was only one square it sank immediatley)");
                Console.WriteLine("The computer gets and extra go!");
                sunkBoats++;
                computerWon = "yes";


            }
            else
            {
                playerGrid[attackNums[1], attackNums[0]] = 'M';
                DisplayGrids(playerGrid, hitsAndMissesGrid,computerGrid);
                Console.WriteLine("the computer sent a missile but it missed!");
 
            }
            Thread.Sleep(3000);
            return computerWon;
        }


        static void DisplayGrids(char[,] playerGrid, char[,] hitsAndMissesGrid, char[,] computerGrid)
            // computer grid just for testing
            // I was thinking that to go down you mins top but it was plus (you can go infinitley down!)
        {
            Console.Clear();
            char startingLetter = 'A';
            // sets the letter to start at
            int gridSpacing = 5;
            char[][,] grids = { playerGrid, hitsAndMissesGrid, computerGrid };
            string[] girdnames = { "Your grid", "Your hits and misses", "The computer's grid" };
            //the order wich you pass the grids matters!
            int top = Console.CursorTop;
            int left = 0;
            Console.WriteLine(top);
            for (int d = 0; d < grids.Length; d++)
                //cycle through all of the grids
            {
                int currnentTop = top;
                Console.SetCursorPosition(left, currnentTop);// move the cursor
                Console.WriteLine($"{girdnames[d]}:");
                currnentTop += 1;
                Console.SetCursorPosition(left, currnentTop);// new line
                Console.Write("   ");
                for (int f = 0; f < grids[d].GetLength(1); f++)
                {
                    Console.Write($"{f} ");
                    // print a num for each collum you have
                }
                // New line!
                currnentTop += 1;
                Console.SetCursorPosition(left, currnentTop);
                for (int i = 0; i < grids[d].GetLength(0); i++)
                {
                    Console.Write($"{Convert.ToChar(startingLetter + i)}. ");
                    // at the start of each line add the line index to the starting letter to get the current letter
                    for (int j = 0; j < grids[d].GetLength(1); j++)
                    {
                        switch(grids[d][i, j])
                        {
                            case '*':
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 'B':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 'D':
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case 'M':
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 'H':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                        }
                        Console.Write($"{grids[d][i, j]} ");
                        // write each row on the same line
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    currnentTop += 1;
                    Console.SetCursorPosition(left, currnentTop);//move down a row

                }
                left+= (grids[d].GetLength(0)*2)+3+gridSpacing;// *2 and +3 account for spaces and axes lables

            }
            Console.SetCursorPosition(0, Console.CursorTop);
            // set the cursor back to the start
        }

        // replace the times when you just display the player grid with displaying all grids
        static char[,] IntialisePlayerGrid(char[,] playerGrid,char[,] hitsAndMissesGrid, char[,] computerGrid)
      
        {
            SetBlankGrid(playerGrid);
            // if I add different types of boats I am going to have to significantly change this
            int numberOfBoats = 5;

            //DisplayGrid(playerGrid);

            for(int i = 1; i <= numberOfBoats; i++)
            {
                int[] boatCoordinates = new int[2];
                int[] inputCoordinates = { -1, -1 };
                while (inputCoordinates[0] == -1 ) 
                // -1 if invalid input coordinates 
                {
                    DisplayGrids(playerGrid, hitsAndMissesGrid, computerGrid);
                    Console.WriteLine($"enter your coordinates for the placment of boat number {i} e.g 5A (only place boats in empty spaces):");
                    inputCoordinates = GetCoodrinates(Console.ReadLine());

                    if(inputCoordinates[0] != -1 && playerGrid[inputCoordinates[1], inputCoordinates[0]] == 'B')
                    {
                        inputCoordinates[0] = -1;
                        inputCoordinates[1] = -1;
                        Console.WriteLine("You already have a boat there!");
                        Thread.Sleep(1000);
                        // if boat already exists on square set to -1
                    }
                }
                playerGrid[inputCoordinates[1],inputCoordinates[0]] = 'B';
                //DisplayGrids(playerGrid,hitsAndMissesGrid,computerGrid);
                Console.WriteLine("\nBoat placed!");
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
                //DisplayGrid(computerGrid);
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
            if (coordinates == "" || coordinates.Length != 2 || coordinates[0]<'0'||coordinates[0]-'0' >= gridSize || coordinates[1] >= ('A' + gridSize )|| coordinates[1] < ('A'))
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