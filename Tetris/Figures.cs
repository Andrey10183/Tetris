using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal static class Figures
    {
        public static Dictionary<string, List<bool[,]>> figures = new Dictionary<string, List<bool[,]>>
        {
            { "Lshape", new List<bool[,]>()
                {
                    new bool[3,3]
                    {
                        { false, true, true },
                        { false, true, false },
                        { false, true, false }
                    },
                    new bool[3,3]
                    {
                        { false, false, false },
                        { true, true, true },
                        { false, false, true }
                    },
                    new bool[3,3]
                    {
                        { false, true, false },
                        { false, true, false },
                        { true, true, false }
                    },
                    new bool[3,3]
                    {
                        { true, false, false },
                        { true, true, true },
                        { false, false, false }
                    }
                }
            },
        };

        public static List<List<Point>> figs = new List<List<Point>>() 
        {             
                new List<Point>()
                { 
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(1,0),
                    new Point(1,1)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(0,1),
                    new Point(-1,1)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(-1,-1),
                    new Point(1,0)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(1,-1),
                    new Point(0,1)
                }

        };
    }
}
