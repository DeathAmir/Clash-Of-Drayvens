using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using CSS.Core;
using CSS.Core.Settings;
using CSS.Helpers;
using CSS.Core.Web;
using static CSS.Core.Logger;

namespace CSS
{
    internal class Program
    {
        internal static int OP = 0;
        internal static string Title = $"Clash Of Drayvens Server v{Constants.Version} Build {Constants.Build} | Online Players: ";
        public static Stopwatch _Stopwatch = new Stopwatch();
        public static string Version { get; set; }

        internal static void Main()
        {
            const int GWL_EXSTYLE = -20;
            const int WS_EX_LAYERED = 0x80000;
            const uint LWA_ALPHA = 0x2;

            IntPtr handle = GetConsoleWindow();
            SetWindowLong(handle, GWL_EXSTYLE, (int)GetWindowLong(handle, GWL_EXSTYLE) ^ WS_EX_LAYERED);

            try
            {
                Console.SetWindowSize(92, 32);
            }
            catch
            {
                // Some hosts do not allow resizing the console. Startup should continue.
            }

            if (Utils.ParseConfigBoolean("Animation"))
            {
                new Thread(() =>
                {
                    for (int i = 20; i < 227; i++)
                    {
                        SetLayeredWindowAttributes(handle, 0, (byte)i, LWA_ALPHA);
                        Thread.Sleep(i < 100 ? 5 : 15);
                    }
                }).Start();
            }
            else
            {
                SetLayeredWindowAttributes(handle, 0, 227, LWA_ALPHA);
            }

            UpdateTitle();
            Say();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Logger.WriteCenter("============================================================");
            Logger.WriteCenter("CLASH OF DRAYVENS");
            Logger.WriteCenter("Private Server Core");
            Logger.WriteCenter("Endpoint: irautox.ir:7676");
            Logger.WriteCenter("============================================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Logger.WriteCenter("Based on the open-source Clash Of SL server project.");
            Logger.WriteCenter("Not affiliated with or endorsed by Supercell Oy.");
            Console.ResetColor();

            Say();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[DRAYVENS] ");

            Version = VersionChecker.GetVersionString();
            _Stopwatch.Start();

            if (Version == Constants.Version)
            {
                Console.WriteLine($"> Core ready: {Constants.Version}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Say("Preparing Clash Of Drayvens server...\n");
                Resources.Initialize();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("> Version validation failed. Aborting startup.");
                Thread.Sleep(3000);
                Environment.Exit(1);
            }
        }

        public static void UpdateTitle()
        {
            if (Constants.LicensePlanID == 2)
                Console.Title = Title + OP + "/700";
            else if (Constants.LicensePlanID == 1)
                Console.Title = Title + OP + "/350";
            else
                Console.Title = Title + OP;
        }

        public static void TitleU()
        {
            ++OP;
            UpdateTitle();
        }

        public static void TitleD()
        {
            if (OP > 0)
                --OP;
            UpdateTitle();
        }

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();
    }
}
