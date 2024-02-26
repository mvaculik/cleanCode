using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGameRefactored
{
    class Program
    {
        private static int screenWidth = Console.WindowWidth;
        private static int screenHeight = Console.WindowHeight;
        private static Random random = new Random();
        private static int score = 5;
        private static bool isGameOver = false;
        private static SnakePart snakeHead = new SnakePart(screenWidth / 2, screenHeight / 2, ConsoleColor.Red);
        private static List<SnakePart> snakeBody = new List<SnakePart>();
        private static string movementDirection = "RIGHT";
        private static Point berry = GenerateBerry();

        static void Main(string[] args)
        {
            Console.WindowHeight = 24;
            Console.WindowWidth = 48;
            SetupConsole();
            ShowStartScreen();
            
            while (!isGameOver)
            {
                RefreshScreen();
                CheckCollisions();
                UpdateSnakePosition();
                ProcessInput();
                System.Threading.Thread.Sleep(75);
            }

            EndGame();
        }

        private static void SetupConsole()
        {
            Console.CursorVisible = false;
        }

        private static void ShowStartScreen()
        {
            Console.Clear();
            Console.WriteLine("Press any key");
            Console.ReadKey(true);
        }

        private static void RefreshScreen()
        {
            Console.Clear();
            DrawBorders();
            DrawBerry();
            DrawSnake();
        }

        private static void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        private static void DrawBerry()
        {
            Console.SetCursorPosition(berry.X, berry.Y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        private static void DrawSnake()
        {
            foreach (var part in snakeBody)
            {
                Console.SetCursorPosition(part.XPos, part.YPos);
                Console.ForegroundColor = part.Color;
                Console.Write("■");
            }

            Console.SetCursorPosition(snakeHead.XPos, snakeHead.YPos);
            Console.ForegroundColor = snakeHead.Color;
            Console.Write("■");
        }

        private static void CheckCollisions()
        {
            if (snakeHead.XPos == 0 || snakeHead.XPos == screenWidth - 1 || snakeHead.YPos == 0 || snakeHead.YPos == screenHeight - 1)
            {
                isGameOver = true;
            }

            if (snakeHead.XPos == berry.X && snakeHead.YPos == berry.Y)
            {
                score++;
                berry = GenerateBerry();
            }

            if (snakeBody.Any(part => part.XPos == snakeHead.XPos && part.YPos == snakeHead.YPos))
            {
                isGameOver = true;
            }
        }

        private static void UpdateSnakePosition()
        {
            snakeBody.Add(new SnakePart(snakeHead.XPos, snakeHead.YPos, ConsoleColor.Green));
            MoveHead();

            if (snakeBody.Count > score)
            {
                snakeBody.RemoveAt(0);
            }
        }

        private static void MoveHead()
        {
            switch (movementDirection)
            {
                case "UP":
                    snakeHead.YPos--;
                    break;
                case "DOWN":
                    snakeHead.YPos++;
                    break;
                case "LEFT":
                    snakeHead.XPos--;
                    break;
                case "RIGHT":
                    snakeHead.XPos++;
                    break;
            }
        }


        private static void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow when movementDirection != "DOWN":
                        movementDirection = "UP";
                        break;
                    case ConsoleKey.DownArrow when movementDirection != "UP":
                        movementDirection = "DOWN";
                        break;
                    case ConsoleKey.LeftArrow when movementDirection != "RIGHT":
                        movementDirection = "LEFT";
                        break;
                    case ConsoleKey.RightArrow when movementDirection != "LEFT":
                        movementDirection = "RIGHT";
                        break;
                }
            }
        }


        private static void EndGame()
        {
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);

        }

        private static Point GenerateBerry()
        {
            return new Point(random.Next(1, screenWidth - 2), random.Next(1, screenHeight - 2));
        }

        class SnakePart
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor Color { get; set; }

            public SnakePart(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                Color = color;
            }
        }

        struct Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
