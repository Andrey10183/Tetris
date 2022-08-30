namespace Tetris
{
    internal class GameController
    {
        private TetrisEngine tetrisEngine;
        private Display display;
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
                    
        public void Start()
        {
            ResetAndStartGame();

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
                    case ConsoleKey.DownArrow: tetrisEngine.IncreaseFallSpeed(); break;
                    case ConsoleKey.Escape: tetrisEngine.PauseResume(); break;
                    default:
                        tetrisEngine.RestoreSpeed();
                        break;
                }
            }
        }

        private void GameOver(TetrisEngine sender, GameEngineEventArgs e)
        {
            if (display.ShowDialogForm("GAME OVER!", "RESTART?"))
                ResetAndStartGame();
            else
                gameOver = true;
        }

        private void ScoreEarned(TetrisEngine sender, GameEngineEventArgs e)
        {
            score += 100 * e._rowsCompleeted * e._rowsCompleeted;
            GameSpeedControl(score);
            display.SetScore(score);
        }

        private void Draw(TetrisEngine sender, GameEngineEventArgs e)
        {
            if (!gameOver) display.Draw();          
        }

        private void GameSpeedControl(int currScore)
        {
            if (currScore >= currScoreLimit)
            {
                tetrisEngine.currentSpeed -= 50;
                currScoreLimit += 2000;
            }
        }

        private void ResetAndStartGame()
        {
            currScoreLimit = 1000;
            score = 0;
            gameOver = false;
            display.SetScore(0);
            tetrisEngine.StartGame();
        }
    }
}
