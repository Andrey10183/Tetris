using System.Drawing;

namespace Tetris
{
    public class PointData
    {        
        public Point Position { get; set; }
        public Color PointColor { get; set; }

        public PointData(Point position, Color pointColor)
        {
            Position = position;
            PointColor = pointColor;
        }
    }   
}