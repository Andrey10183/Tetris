namespace Tetris
{
    public class GameEngineEventArgs
    {
        public bool[,] _gamefield;
        public int _score;

        public GameEngineEventArgs(bool[,] gamefield, int score)
        {
            _gamefield = gamefield;
            _score = score;
        }
    }
}