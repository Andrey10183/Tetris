namespace Tetris
{
    internal class GameController
    {
        private TetrisEngine tetrisEngine;
        private Display display;
        private bool[,] field;
        public int score;
        private int currScoreLimit;
        private bool gameOver = false;

        public GameController(TetrisEngine tetrisEngine, Display display)
        {
            this.tetrisEngine = tetrisEngine;
            this.display = display;

            this.tetrisEngine.OnRedraw += Draw;
            this.tetrisEngine.OnScoreEarned += ScoreEarned;
            this.tetrisEngine.OnGameOver += GameOver;
        }

        private void GameOver(TetrisEngine sender, GameEngineEventArgs e)
        {
            
            if (display.ShowDialogForm("GAME OVER!", "RESTART?"))
            {
                currScoreLimit = 1000;
                score = 0;
                gameOver = false;
                tetrisEngine.StartGame();
            }
            else
                gameOver = true;
        }

        private void ScoreEarned(TetrisEngine sender, GameEngineEventArgs e)
        {
            score += 100 * e._rowsCompleeted * e._rowsCompleeted;
            display.SetScore(score);
        }
            
        public void Start()
        {
            currScoreLimit = 1000;
            tetrisEngine.StartGame();
            
            while (!gameOver)
            {
                ConsoleKey keyPressed;
                if (Console.KeyAvailable)
                    keyPressed = Console.ReadKey(true).Key;
                else
                    keyPressed = default;

                switch (keyPressed)
                {
                    case ConsoleKey.RightArrow: tetrisEngine.MoveRight(); break;
                    case ConsoleKey.LeftArrow: tetrisEngine.MoveLeft(); break;
                    case ConsoleKey.Spacebar: tetrisEngine.Rotate(); break;
                    case ConsoleKey.DownArrow: tetrisEngine.Speed = 50; break;
                    case ConsoleKey.Escape: tetrisEngine.PauseResume(); break;
                    default:
                        tetrisEngine.Speed = tetrisEngine._setupSpeed;
                        break;
                }
            }
        }

        private void Draw(TetrisEngine sender, GameEngineEventArgs e)
        {
            if (!gameOver) display.Draw();          
        }

        private void GameSpeedControl(int currScore)
        {
            if (currScore >= currScoreLimit)
            {
                tetrisEngine._setupSpeed -= 100;
                currScoreLimit += 1000;
            }
        }
    }
}
