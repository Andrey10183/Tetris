// See https://aka.ms/new-console-template for more information
using Tetris;

const int width = 10;
const int height = 20;

var tetris = new TetrisEngine(width, height, 500);
var display = new Display(tetris.getField(), width, height);
var gameController = new GameController(tetris, display);

gameController.Start();

