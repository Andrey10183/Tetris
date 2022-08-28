using System;
using System.Collections.Generic;
using System.Linq;
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
            _tetrisEngine.StartGame();
                        
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(maxX+14, maxY+14);
            Console.SetWindowSize(maxX+12, maxY+12);
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
                for (int i = 0; i < maxX + 2; i++)
                    s1.Append("X");
                s1.Append('\n');
                for (int y = 0; y < maxY; y++)
                {
                    s1.Append("     X");
                    for (int x = 0; x < maxX; x++)
                        s1.Append(e._gamefield[y, x] ? "X" : " ");
                    s1.Append("X");
                    s1.Append('\n');
                }
                s1.Append("     ");
                for (int i = 0; i < maxX + 2; i++)
                    s1.Append("X");

                Console.Write(s1);
                DrawingInProcess = false;
            }            
        }
    }
}
