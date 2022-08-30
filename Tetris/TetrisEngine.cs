using System.Drawing;

namespace Tetris
{
    internal class TetrisEngine
    {
        public delegate void EventHandler(TetrisEngine sender, GameEngineEventArgs e);
        public event EventHandler? OnRedraw;
        public event EventHandler? OnGameOver;
        public event EventHandler? OnScoreEarned;

        private readonly int width;
        private readonly int height;
        private bool[,] field;

        private Random generateFigure = new Random();
        private int currentRotationState;
        private List<List<Point>> currentFigurePositions;
        private Point currentFigurePosition;
        private bool figureFalling = false;
        private bool gamePaused;

        public static Timer _timer;

        public int currentSpeed;
        private int initialSpeed;
        private int maxFallSpeed = 50;

        private int speed;       
        private int Speed 
        { 
            get { return speed; }
            set 
            {
                if (value != speed)
                {
                    speed = value;
                    _timer.Change(0, value);
                }    
            } 
        }
               
        public TetrisEngine(int width = 15, int height = 20, int initialspeed = 1000)
        {
            this.width = width;
            this.height = height;
            //currentSpeed = initialspeed;
            this.initialSpeed = currentSpeed = initialspeed;
            field = new bool[height,width];
        }

        private void NextFrame(object sender)
        {
            if (gamePaused) return;
            
            if (!(figureFalling && CanMoveOrPlace(currentFigurePositions, currentRotationState, currentFigurePosition, new Point(0, 1))))
            {
                //figure fall down - check compleeted rows
                figureFalling = false;
                var rowsCompleted = 0;
                for (int i = height - 1; i >= 0; i--)
                {
                    if (IsRowComplete(i))
                    {
                        //Delete row
                        rowsCompleted++;
                        DeleteRow(i);
                        ShiftFieldContence(i - 1);
                        i++;
                    }
                }
                if (rowsCompleted > 0) OnScoreEarned?.Invoke(this, new GameEngineEventArgs(field, rowsCompleted));
            }

            if (!figureFalling)
                InstantiateNewFigure();

            OnRedraw?.Invoke(this, new GameEngineEventArgs(field));
        }
        
        private void InstantiateNewFigure()
        {
            currentRotationState = 0;
            currentFigurePositions = Figures.figs[generateFigure.Next(Figures.figs.Count)];
            currentFigurePosition = new Point(width / 2, 1);
            figureFalling = true;
            if (!CanMoveOrPlace(currentFigurePositions, currentRotationState, currentFigurePosition, new Point(0, 0)))
            {
                Pause();
                OnGameOver?.Invoke(this, new GameEngineEventArgs(field));                
            }
        }

        public void StartGame()
        {           
            Clear();
            gamePaused = false;
            InstantiateNewFigure();            
            _timer = new Timer(NextFrame, null, 0, currentSpeed);
            Speed = initialSpeed;
        }

        public void Restart()
        {
            Clear();            
            Resume();
            InstantiateNewFigure();
        }

        void DeleteRow(int row)
        {
            for (int i = 0; i < width; i++)
                field[row, i] = false;
        }

        private void ShiftFieldContence(int row)
        {
            for (int i = row; i >=0; i--)
                for (int j = 0; j < width; j++)
                    field[i+1, j] = field[i, j];
        }

        private bool IsRowComplete(int row)
        {
            for (var i = 0; i < width; i++)
                if (!field[row,i]) return false;
            return true;
        }

        public bool[,] getField() =>
            field;

        private bool IsValid(Point point) =>
            (point.X >= 0 && point.X < width) &&
            (point.Y >= 0 && point.Y < height);

        private bool CanMoveOrPlace(List<List<Point>> figure,int currRotState, Point currRefPos, Point delta)
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

            var figPoints = figure[newRotState];
            var newRefPosX = currRefPos.X + delta.X;
            var newRefPosY = currRefPos.Y + delta.Y;

            //figure old position forming
            if (delta.X != 0 || delta.Y != 0 || rotation)
                for (int i = 0; i < figPoints.Count; i++)
                {
                    var xOld = currRefPos.X + figure[currRotState][i].X;
                    var yOld = currRefPos.Y + figure[currRotState][i].Y;

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
            currentFigurePosition = new Point(newRefPosX, newRefPosY);
            currentRotationState = newRotState;
            return true;
        }

        private void Fill(List<Point> data, bool value)
        {
            for (int i = 0; i < data.Count; i++)
                field[data[i].Y, data[i].X] = value;
        }

        private void Clear()
        {
            var upper0 = field.GetUpperBound(0)+1;
            var upper1 = field.GetUpperBound(1)+1;

            for (int i = 0; i < upper0; i++)
                for (int j = 0;j < upper1; j++)
                    field[i,j] = false;
        }

        public void MoveRight()
        {
            if (!gamePaused)
            {
                CanMoveOrPlace(currentFigurePositions, currentRotationState, currentFigurePosition, new Point(1, 0));
                OnRedraw?.Invoke(this, new GameEngineEventArgs(field));
            }           
        }

        public void MoveLeft()
        {
            if (!gamePaused)
            {
                CanMoveOrPlace(currentFigurePositions, currentRotationState, currentFigurePosition, new Point(-1, 0));
                OnRedraw?.Invoke(this, new GameEngineEventArgs(field));
            }
        }

        public void Rotate()
        {
            if (!gamePaused)
            {
                CanMoveOrPlace(currentFigurePositions, currentRotationState, currentFigurePosition, new Point(1, 1));
                OnRedraw?.Invoke(this, new GameEngineEventArgs(field));
            }
        }

        public void IncreaseFallSpeed() =>
            Speed = maxFallSpeed;

        public void RestoreSpeed() =>
            Speed = currentSpeed;

        public void PauseResume()
        {
            if (gamePaused)
                Resume();
            else
                Pause();            
        }

        public void Pause()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            gamePaused = true;
        }

        public void Resume()
        {
            _timer.Change(0, currentSpeed);
            gamePaused = false;
        }
    }
}
