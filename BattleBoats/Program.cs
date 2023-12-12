using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace BattleBoats
{
    internal class Program
    {
        static void Main(string[] args)
        {
        
           
            char[,] ownGrid = new char[8, 8];
            char[,] hitsAndMissesGrid = new char[8, 8];
            char[,] computerGrid = new char[8, 8];
            string mode = "";
            // just for testing will not be declared in main for the final thing


            while (mode!= "Wants to quit")
            {
                mode =DisplayMenu();
            }
            ownGrid =IntialisePlayerGrid(ownGrid);

            int[] arrayCoodinateas = {-1,-1};
            while (arrayCoodinateas[0] == -1) 
            {
                Console.WriteLine("enter your coordinates:");
                string coordinate = Console.ReadLine();
                arrayCoodinateas = GetCoodrinates(coordinate);
                foreach (int i in arrayCoodinateas) { Console.WriteLine(i); }
            }
            computerGrid = IntialiseComputerGrid(computerGrid);

            


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
                //NewGame();
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
                " Your aim is to destroy your opponent’s boats before they destroy yours taking turns to guess different locations.\n" +
                " Select New game to start.");
        }


        static void NewGame()
        {
            int gameNum = 0;
            string gameWon = "not yet";
            string oddPlayer = "player";
            string evenPlayer = "computer";
            char[,] ownGrid = new char[8, 8];
            char[,] hitsAndMissesGrid = new char[8, 8];
            char[,] computerGrid = new char[8, 8];
            string mode = "";

            ownGrid = IntialisePlayerGrid(ownGrid);
            computerGrid = IntialiseComputerGrid(computerGrid);
            hitsAndMissesGrid = SetBlankGrid(hitsAndMissesGrid);
            // set the grids

            while (gameWon != "yes")
            {
                gameNum++;
                if (gameNum % 2 == 0) 
                    // even turn number
                {
                    if (evenPlayer == "player")
                    {
                        //playerWon = PlayerTurn();
                    }
                    else
                    {
                        // ComputerTurn();
                    }
                }
                else
                {
                    if (oddPlayer == "player")
                    {
                        //playerWon = PlayerTurn();
                    }
                    else
                    {
                        // ComputerTurn();
                    }
                }

                //if (plaerWon == "yes")
                //{
                //    (oddPlayer, evenPlayer) = (evenPlayer, oddPlayer);
                //}
                // swap them using tuples
                
            }
        }

        static string PlayerTurn(char[,] computerGrid, char[,] ownGrid, char[,] hitsAndMissesGrid)
        {
            int[] attackNums = new int[2];
            Console.WriteLine("Player turn!:");
            do
            {
                Console.WriteLine("Enter the coordinates of where you want to send your missile (in the form numberLetter):");
                attackNums = GetCoodrinates(Console.ReadLine());
            } while (attackNums[0] == -1);

            if (computerGrid[attackNums[0],attackNums[1]] == 'b')
            {

            }

            return new string("");//placeholder

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
                int[] boatCoordinates = new int[2];
                int[] inputCoordinates = new int[2];
                do 
                // -1 if invalid input coordinates and if they attempt to place one on top of a boat
                {
                    inputCoordinates[1] = rand.Next(computerGrid.GetLength(0));
                    inputCoordinates[0] = rand.Next(computerGrid.GetLength(1));
                }
                while (computerGrid[inputCoordinates[1], inputCoordinates[0]] == 'B') ;


                computerGrid[inputCoordinates[1], inputCoordinates[0]] = 'B';
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
            if (coordinates == "" || coordinates.Length > 2 || coordinates[0]-'0' >= gridSize || coordinates[1] >= ('A' + gridSize))
                // convert the chars in this if statment to nums it is freaking out at their ascii values!
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