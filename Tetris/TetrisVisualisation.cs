using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class TetrisVisualisation
    {
        private TetrisEngine _tetrisEngine;
        private bool[,] field;
        private int maxX;
        private int maxY;

        private bool DrawingInProcess = false;

        Action MoveLeft;

        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        const string UNDERLINE = "\x1B[4m";
        const string RESET = "\x1B[0m";
        const string RED = "\x1b[43m";
        const string GREEN = "\x1b[42m";


        public TetrisVisualisation(TetrisEngine tetrisEngine)
        {
            _tetrisEngine = tetrisEngine;
            field = _tetrisEngine.getField();
            maxX = field.GetUpperBound(1) + 1;
            maxY = field.GetUpperBound(0) + 1;

            MoveLeft = () => _tetrisEngine.MoveLeft();

            _tetrisEngine.OnRedraw += Draw1;
        }

        public void StartVisualization()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            uint mode;
            GetConsoleMode(handle, out mode);
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(handle, mode);

            _tetrisEngine.StartGame();
                        
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(maxX*2+14, maxY+14);
            Console.SetWindowSize(maxX*2+12, maxY+12);
            Console.CursorVisible = false;

            
            while (true)
            {
                ConsoleKey keyPressed;
                //Draw(_tetrisEngine.getField());
                //_tetrisEngine.NextFrame();
                if (Console.KeyAvailable)
                    keyPressed = Console.ReadKey(true).Key;
                else
                    keyPressed = default;

                switch (keyPressed)
                {
                    case ConsoleKey.RightArrow: _tetrisEngine.MoveRight(); break;
                    case ConsoleKey.LeftArrow: MoveLeft(); break;
                    case ConsoleKey.Spacebar: _tetrisEngine.Rotate(); break;
                    case ConsoleKey.DownArrow: _tetrisEngine.Speed = 50; break;
                    default:
                        _tetrisEngine.Speed = _tetrisEngine._setupSpeed;
                        break;
                }
                //Thread.Sleep(50);

            }

            Console.ReadLine();
        }

        private void Draw1(TetrisEngine sender, GameEngineEventArgs e)
        {
            string fig = GREEN + "XX";


            if (!DrawingInProcess)
            {
                DrawingInProcess = true;
                Console.SetCursorPosition(0, 0);
                var maxX = e._gamefield.GetUpperBound(1) + 1;
                var maxY = e._gamefield.GetUpperBound(0) + 1;
                var s1 = new StringBuilder();
                s1.Append('\n');
                s1.Append('\n');
                s1.Append("     ");
                for (int i = 0; i < maxX*2 + 2; i++)
                    s1.Append(RED + " " + RESET);
                s1.Append('\n');
                for (int y = 0; y < maxY; y++)
                {
                    s1.Append("     "+RED+" "+RESET);
                    for (int x = 0; x < maxX; x++)
                        s1.Append(e._gamefield[y, x] ? (GREEN+"  "+RESET) : "  ");
                    s1.Append(RED + " " + RESET);
                    s1.Append('\n');
                }
                s1.Append("     ");
                for (int i = 0; i < maxX*2 + 2; i++)
                    s1.Append(RED + " " + RESET);

                Console.Write(s1);
                DrawingInProcess = false;
            }            
        }
    }
}
