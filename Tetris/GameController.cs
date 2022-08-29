namespace Tetris
{
    internal class GameController
    {
        private TetrisEngine tetrisEngine;
        private Display display;
        private static GameInfo gameInfo;
        private bool[,] field;
        public int score;

        public GameController(TetrisEngine tetrisEngine, Display display)
        {
            this.tetrisEngine = tetrisEngine;
            this.display = display;
            gameInfo = new GameInfo() { gameField = tetrisEngine.getField(), score = 0 };

            this.tetrisEngine.OnRedraw += Draw;
            this.tetrisEngine.OnScoreEarned += ScoreEarned;            
        }

        public GameInfo GetGameInfo() =>
            gameInfo;

        private void ScoreEarned(TetrisEngine sender, GameEngineEventArgs e)
        {
            gameInfo.score += 100 * e._rowsCompleeted * e._rowsCompleeted;
        }
            
        public void Start()
        {
            tetrisEngine.StartGame();
            
            while (true)
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
                    default:
                        tetrisEngine.Speed = tetrisEngine._setupSpeed;
                        break;
                }
            }
        }

        private void Draw(TetrisEngine sender, GameEngineEventArgs e)
        {
            gameInfo.gameField = e._gamefield;
            display.Draw();          
        }
    }
}
