// See https://aka.ms/new-console-template for more information
using System.Text;
using Tetris;

var tetris = new TetrisEngine(10, 20, 200);
var Visualisation = new TetrisVisualisation(tetris);
Visualisation.StartVisualization();

    
