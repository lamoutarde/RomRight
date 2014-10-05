using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomRight.ConsoleHelper
{
    public class Utils
    {
        


        public static ConsoleColor SAVED_TEXT_COLOR;
        public static int SAVED_CURSOR_POSITION;

        public static void SaveConsoleConfig()
        {
            SAVED_TEXT_COLOR = Console.ForegroundColor;
            SAVED_CURSOR_POSITION = Console.CursorTop;
        }

        public static void RestoreConsoleConfig()
        {
            Console.ForegroundColor = SAVED_TEXT_COLOR;
            Console.CursorTop = SAVED_CURSOR_POSITION;
        }
    }
}
