using System.Runtime.InteropServices;
using System.Text;

namespace Tetris
{
    internal class Display
    {
        private bool[,] gameField;
        private int width;
        private int height;

        private int windowWidth;
        private int windowHeight;

        private int offsetWidth;
        private int offsetHeight;
        private bool drawingInProcess;
        private int score;
        private int heightControl;

        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

        //Libraries importing
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public Display(bool[,] gameField,int width, int height, int offsetWidth = 4, int offsetHeight = 4)
        {
            this.width = width;
            this.height = height;
            this.offsetWidth = offsetWidth;
            this.offsetHeight = offsetHeight;
            this.gameField = gameField;

            SequencesEnable();
            DisplayInitialization();
        }
        
        public void SetScore(int score) =>
            this.score = score;

        public void Draw()
        {
            if (!drawingInProcess)
            {
                heightControl = 0;
                drawingInProcess = true;
                Console.SetCursorPosition(0, 0);
                var screenImage = new StringBuilder();
                screenImage = screenImage.Append(Colors.REDUCE_BRIGHT);

                DrawHeightOffset(screenImage);
                
                DrawWidthOffset(screenImage);
                screenImage.Append(Colors.FG_ORANGE+ "SCORE: " + score + '\n');
                                                           
                DrawWidthOffset(screenImage);                
                DrawHorizontalFieldFrame(screenImage);
               
                for (int y = 0; y < height; y++)
                {
                    DrawWidthOffset(screenImage);
                    DrawVerticalFieldFrame(screenImage);

                    for (int x = 0; x < width; x++)
                        screenImage.Append(gameField[y, x] ? (GetColorFigure(x,heightControl) + "  " + Colors.RESET) : "  ");

                    DrawVerticalFieldFrame(screenImage);
                    screenImage.Append('\n');
                }
                DrawWidthOffset(screenImage);
                DrawHorizontalFieldFrame(screenImage);

                Console.Write(screenImage);
                drawingInProcess = false;
            }
        }

        public bool ShowDialogForm(string message, string query)
        {
            var offsetX = 6;
            var offsetY = 2;
            var xPos = (windowWidth - message.Length) / 2;
            var yPos = (windowHeight - 1) / 2;
            var wWidth = message.Length + offsetX * 2;
            var wHeight = 2 + offsetY * 2;
            var screen = new StringBuilder();
            query += " (Y/N)";

            for (int i = 0; i < wHeight; i++)
            {
                for (int j = 0; j < wWidth; j++)
                {
                    screen.Append(Colors.BG_ORANGE + " ");
                }
                Console.SetCursorPosition(xPos - offsetX, yPos - offsetY + i);
                Console.Write(screen);
                screen.Clear();
            }
            Console.Write(screen);
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(message);
            Console.SetCursorPosition(GetCenteredPosition(query), yPos + 1);
            Console.CursorVisible = true;
            Console.Write(query);
            var input = Console.ReadLine();
            Console.CursorVisible = false;
            Console.WriteLine(Colors.RESET);

            return (input == "y" || input == "Y");
        }

        // Set output mode to handle virtual terminal sequences
        private void SequencesEnable()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            uint mode;
            GetConsoleMode(handle, out mode);
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(handle, mode);
        }

        private void DisplayInitialization()
        {
            windowWidth = width * 2 + offsetWidth * 4;
            windowHeight = height + offsetHeight * 2;

            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(windowWidth + 1, windowHeight + 1);
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.CursorVisible = false;
        }

        private void DrawHeightOffset(StringBuilder input)
        {
            for (int i = 0; i<offsetHeight; i++)
                input.Append('\n');
        }

        private void DrawWidthOffset(StringBuilder input)
        {
            var limit = offsetWidth * 2;
            for (int i = 0; i < limit; i++)
                input.Append(" ");
        }

        private void DrawHorizontalFieldFrame(StringBuilder input)
        {
            for (int i = 0; i < width * 2 + 4; i++)
                input.Append(GetColorFrame(heightControl) + " " + Colors.RESET);
            input.Append('\n');
            heightControl++;
        }

        private void DrawVerticalFieldFrame(StringBuilder input)
        {
            input.Append(GetColorFrame(heightControl++) + "  " + Colors.RESET);
        }

        private string GetColorFrame(int y)
        {
            return "\u001b[48;2;210;140;" + ((y*255)/height) + "m";
        }

        private string GetColorFigure(int x, int y)
        {
            return "\u001b[48;2;60;"+((x * 255) / height) +";"+ ((y * 255) / height) + "m";
        }

        private int GetCenteredPosition(string message)=>
            (windowWidth - message.Length) / 2;
    }
}
