// See https://aka.ms/new-console-template for more information
using Tetris;

const int width = 10;
const int height = 20;

var tetris = new TetrisEngine(width, height, 500);


var display = new Display(new GameInfo() {gameField = tetris.getField()}, width, height);
var gameController = new GameController(tetris, display);

var text = new Dictionary<string, GameInfo>() 
{ 
    { "SCORE", gameController.GetGameInfo() }
};

display.SetupText(text);
gameController.Start();

