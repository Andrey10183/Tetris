using System.Configuration;

namespace Tetris
{
    internal class TetrisGameController
    {
        private TetrisEngine tetrisEngine;
        private Display display;
        public int score;
        public int bestScore;
        private int currScoreLimit;
        private bool gameOver = false;

        public TetrisGameController(TetrisEngine tetrisEngine, Display display)
        {
            this.tetrisEngine = tetrisEngine;
            this.display = display;

            this.tetrisEngine.OnRedraw += Draw;
            this.tetrisEngine.OnScoreEarned += ScoreEarned;
            this.tetrisEngine.OnGameOver += GameOver;

            bestScore = int.Parse(ReadConfigValue("BestScore"));
            display.SetBestScore(bestScore);
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

            if (bestScore < score)
            {
                bestScore = score;
                WriteConfigValue("BestScore", bestScore.ToString());
            }
                
            display.SetScore(score);
            display.SetBestScore(bestScore);
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

        private string ReadConfigValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"\"{nameof(key)}\" не может быть неопределенным или пустым.", nameof(key));

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings[key] != null)
                return settings[key].Value;
            else 
                return null;
        }

        private void WriteConfigValue(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"\"{nameof(key)}\" не может быть неопределенным или пустым.", nameof(key));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"\"{nameof(value)}\" не может быть неопределенным или пустым.", nameof(value));

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings[key] == null)
                return;
            
            settings[key].Value = value;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
