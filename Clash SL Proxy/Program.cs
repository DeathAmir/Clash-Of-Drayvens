using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CSP
{
    internal class Proxy
    {
        // Legacy Android clients connect to TCP/9339. The proxy keeps that
        // compatibility port and forwards traffic to the Drayvens server.
        public const int listenPort = 9339;
        public const string upstreamHost = "irautox.ir";
        public const int upstreamPort = 7676;

        public static Stopwatch _Stopwatch = new Stopwatch();

        private static void Main()
        {
            const int GWL_EXSTYLE = -20;
            const int WS_EX_LAYERED = 0x80000;
            const uint LWA_ALPHA = 0x2;
            IntPtr handle = GetConsoleWindow();
            SetWindowLong(handle, GWL_EXSTYLE, (int)GetWindowLong(handle, GWL_EXSTYLE) ^ WS_EX_LAYERED);
            SetLayeredWindowAttributes(handle, 0, 227, LWA_ALPHA);

            Console.Title = $"Clash Of Drayvens Gateway v{Assembly.GetExecutingAssembly().GetName().Version}";

            if (!Directory.Exists("Packets"))
                Directory.CreateDirectory("Packets");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("============================================================");
            Console.WriteLine("                 CLASH OF DRAYVENS GATEWAY");
            Console.WriteLine("============================================================");
            Console.ResetColor();
            Console.WriteLine($"Legacy listen endpoint : 0.0.0.0:{listenPort}");
            Console.WriteLine($"Drayvens upstream      : {upstreamHost}:{upstreamPort}");
            Console.WriteLine("Based on the open-source Clash Of SL proxy.");
            Console.WriteLine();

            _Stopwatch.Start();

            try
            {
                Server server = new Server(listenPort);
                server.StartServer();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }
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
