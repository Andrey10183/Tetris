using System.Runtime.InteropServices;
using System.Text;

namespace Tetris
{
    internal class Display
    {
        private bool[,] gameField;
        private int width;
        private int height;
        private int offsetWidth;
        private int offsetHeight;
        private bool drawingInProcess;
        private string fieldFrameColor = Colors.BG_ORANGE;
        private string gameObjectColor = Colors.BG_GREEN;
        private Dictionary<string, GameInfo> textOutput;  

        private int color;
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

        public Display(GameInfo gameInfo,int width, int height, int offsetWidth = 4, int offsetHeight = 4)
        {
            this.width = width;
            this.height = height;
            this.offsetWidth = offsetWidth;
            this.offsetHeight = offsetHeight;
            this.gameField = gameInfo.gameField;

            SequencesEnable();
            DisplayInitialization();
        }

        public void SetupText(Dictionary<string, GameInfo> input)
        {
            textOutput = input;
            DisplayInitialization();
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
            //if (textOutput != null)
            var coorectionHeight = textOutput != null ? textOutput.Count : 0;
            var windowWidth = width * 2 + offsetWidth * 4;
            var windowHeight = height + offsetHeight*2 + coorectionHeight;

            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(windowWidth + 1, windowHeight + 1);
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.CursorVisible = false;
        }

        public void Draw()
        {
            if (!drawingInProcess)
            {
                heightControl = 0;
                drawingInProcess = true;
                Console.SetCursorPosition(0, 0);
                var s1 = new StringBuilder();
                s1 = s1.Append(Colors.REDUCE_BRIGHT);

                DrawHeightOffset(s1);

                if (textOutput.Count > 0)
                    foreach (var item in textOutput)
                    {
                        DrawWidthOffset(s1);
                        s1.Append(item.Key + ": " + item.Value.score + '\n');
                    }
                                       
                DrawWidthOffset(s1);
                
                DrawHorizontalFieldFrame(s1);
               
                for (int y = 0; y < height; y++)
                {
                    DrawWidthOffset(s1);
                    DrawVerticalFieldFrame(s1);
                    for (int x = 0; x < width; x++)
                    {
                        s1.Append(gameField[y, x] ? (GetColorFigs(x,heightControl) + "  " + Colors.RESET) : "  ");
                    }

                    DrawVerticalFieldFrame(s1);
                    s1.Append('\n');
                }
                DrawWidthOffset(s1);
                DrawHorizontalFieldFrame(s1);

                Console.Write(s1);
                drawingInProcess = false;
            }
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
                input.Append(GetColor(heightControl) + " " + Colors.RESET);
            input.Append('\n');
            heightControl++;
        }

        private void DrawVerticalFieldFrame(StringBuilder input)
        {
            input.Append(GetColor(heightControl++) + "  " + Colors.RESET);
        }

        private string GetColor(int y)
        {
            return "\u001b[48;2;210;140;" + ((y*255)/height) + "m";
        }

        private string GetColorFigs(int x, int y)
        {
            return "\u001b[48;2;60;"+((x * 255) / height) +";"+ ((y * 255) / height) + "m";
        }
    }
}
