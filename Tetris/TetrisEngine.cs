using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class TetrisEngine
    {
        public delegate void EventHandler(TetrisEngine sender, GameEngineEventArgs e);
        public event EventHandler? OnRedraw;
        public event EventHandler? OnGameOver;

        private int _maxX;
        private int _maxY;
        
        private int _currRotState;
        private List<Point> _currFigure;
        private Point _currFigPos;
        private bool figureFalling = false;


        public Timer _timer;

        public int _setupSpeed;
        private int _speed;
        public int Speed 
        { 
            get { return _speed; }
            set 
            {
                if (value != _speed)
                {
                    _speed = value;
                    _timer.Change(0, value);
                }    
            } 
        }

        public int Score { get; set; }

        private bool[,] field;
        
        public TetrisEngine(int maxX = 15, int maxY = 20, int initialspeed = 1000)
        {
            _maxX = maxX;
            _maxY = maxY;
            _setupSpeed = initialspeed;
            field = new bool[maxY,maxX];
        }

        private void TimerCallback(object sender)
        {
            if (figureFalling && CanMoveOrPlace(_currFigure, _currRotState, _currFigPos, new Point(0, 1)))
            { 
            }
            else
            {
                figureFalling = false;
                //figure fall down - check compleeted rows
                for (int i = _maxY - 1; i >= 0; i--)
                {
                    if (IsRowComplete(i))
                    {
                        //Delete row
                        DeleteRow(i);
                        ShiftFieldContence(i - 1);
                    }
                }
            }

            if (!figureFalling)
            {
                _currRotState = 0;
                _currFigure = Figures.figs[_currRotState];
                _currFigPos = new Point(_maxX / 2, 0);
                figureFalling = true;
            }
            OnRedraw?.Invoke(this, new GameEngineEventArgs(field, Score));
        }

        public void StartGame()
        {
            _currRotState = 0;
            _currFigure = Figures.figs[_currRotState];
            _currFigPos = new Point(_maxX/2, 0);
            CanMoveOrPlace(_currFigure, _currRotState, _currFigPos, new Point(0, 0));
            figureFalling = true;

            _timer = new Timer(TimerCallback, null, 0, _setupSpeed);
            
        }

        void DeleteRow(int row)
        {
            for (int i = 0; i < _maxX; i++)
                field[row, i] = false;
        }

        void RowShift(int row)
        {
            for (int i = 0; i < _maxX; i++)
            {
                field[row - 1, i] = field[row, i];
            }
        }

        void ShiftFieldContence(int row)
        {
            for (int i = row; i >=0; i--)
            {
                for (int j = 0; j < _maxX; j++)
                {
                        field[i+1, j] = field[i, j];
                }
            }
        }

        public bool IsRowComplete(int row)
        {
            for (var i = 0; i < _maxX; i++)
                if (!field[row,i]) return false;
            return true;
        }

        public bool[,] getField() =>
            field;

        private bool IsXValid(int input) =>
            input >=0 && input < _maxX;

        private bool IsYValid(int input) =>
            input >= 0 && input < _maxY;

        private bool IsValid(Point point) =>
            (point.X >= 0 && point.X < _maxX) &&
            (point.Y >= 0 && point.Y < _maxY);

        private bool CanMoveOrPlace(List<Point> figure,int currRotState, Point currRefPos, Point delta)
        {
            var draw = new List<Point>();
            var erase = new List<Point>();
            
            bool rotation = false;
            var newRotState = currRotState;
            if (delta.X == 1 && delta.Y == 1)
            {
                rotation = true;
                newRotState++;
                if (newRotState >= figure.Count)
                    newRotState = 0;
                delta.X = 0;
                delta.Y = 0;
            }

            var figPoints = Figures.figs[newRotState];
            var newRefPosX = currRefPos.X + delta.X;
            var newRefPosY = currRefPos.Y + delta.Y;

            //figure old position forming
            if (delta.X != 0 || delta.Y != 0 || rotation)
                for (int i = 0; i < figPoints.Count; i++)
                {
                    var xOld = currRefPos.X + Figures.figs[currRotState][i].X;
                    var yOld = currRefPos.Y + Figures.figs[currRotState][i].Y;

                    erase.Add(new Point(xOld, yOld));
                }

            Fill(erase, false);

            for (int i = 0; i < figPoints.Count; i++)
            {
                var x = newRefPosX + figPoints[i].X;
                var y = newRefPosY + figPoints[i].Y;

                if (!IsValid(new Point(x, y)) || field[y, x])
                {
                    Fill(erase, true);
                    return false;
                }                   
                else
                {
                    draw.Add(new Point(x, y));
                }                    
            }
            
            Fill(draw, true);
            _currFigPos = new Point(newRefPosX, newRefPosY);
            _currRotState = newRotState;
            return true;
        }

        private void Fill(List<Point> data, bool value)
        {
            for (int i = 0; i < data.Count; i++)
                field[data[i].Y, data[i].X] = value;
        }

        public void MoveRight()
        {
            CanMoveOrPlace(_currFigure, _currRotState, _currFigPos, new Point(1, 0));
            OnRedraw?.Invoke(this, new GameEngineEventArgs(field, Score));
        }

        public void MoveLeft()
        {
            CanMoveOrPlace(_currFigure, _currRotState, _currFigPos, new Point(-1, 0));
            OnRedraw?.Invoke(this, new GameEngineEventArgs(field, Score));
        }

        public void Rotate()
        {
            CanMoveOrPlace(_currFigure, _currRotState, _currFigPos, new Point(1, 1));
            OnRedraw?.Invoke(this, new GameEngineEventArgs(field, Score));
        }
    }
}
