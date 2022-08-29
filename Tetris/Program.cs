// See https://aka.ms/new-console-template for more information
using System.Text;
using Tetris;

var tetris = new TetrisEngine(10, 20, 500);
var Visualisation = new TetrisVisualisation(tetris);
Visualisation.StartVisualization();
//Console.WriteLine("  ");
//Console.BackgroundColor=ConsoleColor.White;
//Console.WriteLine("  ");
//Console.BackgroundColor = ConsoleColor.Black;

