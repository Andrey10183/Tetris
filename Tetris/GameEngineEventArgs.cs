namespace Tetris
{
    public class GameEngineEventArgs
    {
        public bool[,] _gamefield;
        public int _rowsCompleeted;

        public GameEngineEventArgs(bool[,] gamefield, int rowsCompleeted = 0)
        {
            _gamefield = gamefield;
            _rowsCompleeted = rowsCompleeted;
        }
    }
}