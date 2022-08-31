using System.Drawing;

namespace Tetris
{
    internal class Figures
    {
        public static List<List<List<Point>>> figs = new List<List<List<Point>>>() 
        {
            new List<List<Point>>()//L-shape - 0
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
            },
            new List<List<Point>>()//L-shape Reverse - 1
            {
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(1,0),
                    new Point(-1,1)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(0,1),
                    new Point(-1,-1)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(1,0),
                    new Point(1,-1)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(0,1),
                    new Point(1,1)
                }
            },
            new List<List<Point>>()//Qube - 2
            {
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,1),
                    new Point(1,0),
                    new Point(1,1)
                }
            },
            new List<List<Point>>()//Flash -3 
            {
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(-1,-1),
                    new Point(1,0)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(1,0),
                    new Point(1,-1),
                    new Point(0,1)
                }
            },
            new List<List<Point>>()//Flash reverse
            {
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(1,-1),
                    new Point(-1,0)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(-1,-1),
                    new Point(0,1)
                }
            },
            new List<List<Point>>()//T-shape
            {
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(1,0),
                    new Point(0,-1)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(0,1),
                    new Point(1,0)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(1,0),
                    new Point(0,1)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(0,-1),
                    new Point(0,1)
                }
            },
            new List<List<Point>>()//line
            {
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(-1,0),
                    new Point(-2,0),
                    new Point(1,0)
                },
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(0,-1),
                    new Point(0,1),
                    new Point(0,2)
                }
            }
        };
    }
}
