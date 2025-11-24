using System;
using System.Collections.Generic;
using System.Threading;

namespace CLI.UI
{
    public class SnowEngine
    {
        private readonly int maxFlakes;
        private readonly List<Flake> flakes = new();
        private readonly Random rnd = new();
        private Thread loopThread;
        private bool running;
        private readonly object drawLock = new();

        private class Flake { public int X; public int Y; public char C; }

        public SnowEngine(int maxFlakes = 80)
        {
            this.maxFlakes = maxFlakes;
        }

        public void Start()
        {
            if (running) return;
            running = true;
            InitFlakes();

            loopThread = new Thread(Loop)
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };
            loopThread.Start();
        }

        public void Stop()
        {
            running = false;
            loopThread?.Join(200);
        }

        private void InitFlakes()
        {
            lock (drawLock)
            {
                flakes.Clear();
                int top = Console.WindowHeight - Layout.BackgroundHeight;
                for (int i = 0; i < maxFlakes; i++)
                {
                    flakes.Add(new Flake
                    {
                        X = rnd.Next(Math.Max(1, Console.WindowWidth)),
                        Y = rnd.Next(top, Console.WindowHeight),
                        C = RandomChar()
                    });
                }
            }
        }

        private char RandomChar()
        {
            char[] chars = { '.', '*', '❄', '·' };
            return chars[rnd.Next(chars.Length)];
        }

        // Call BEFORE you clear/refresh UI so existing background artifacts get removed.
        public void ClearBackgroundRegion()
        {
            lock (drawLock)
            {
                int top = Console.WindowHeight - Layout.BackgroundHeight;
                string blank = new string(' ', Console.WindowWidth);
                for (int y = top; y < Console.WindowHeight; y++)
                {
                    Console.SetCursorPosition(0, y);
                    Console.Write(blank);
                }
            }
        }

        private void Loop()
        {
            while (running)
            {
                lock (drawLock)
                {
                    DrawFrame();
                }
                Thread.Sleep(90);
            }
        }

        private void DrawFrame()
        {
            int top = Console.WindowHeight - Layout.BackgroundHeight;

            // Radera gamla positioner (vi skriver över senare)
            foreach (var f in flakes)
            {
                if (f.Y >= top && f.Y < Console.WindowHeight)
                {
                    Console.SetCursorPosition(Math.Min(Console.WindowWidth - 1, Math.Max(0, f.X)), Math.Min(Console.WindowHeight - 1, Math.Max(0, f.Y)));
                    Console.Write(' ');
                }
            }

            // Flytta och återställ de som nått botten
            foreach (var f in flakes)
            {
                f.Y++;
                if (f.Y >= Console.WindowHeight)
                {
                    f.Y = top;
                    f.X = rnd.Next(Math.Max(1, Console.WindowWidth));
                    f.C = RandomChar();
                }
                // liten sidoförskjutning
                if (rnd.NextDouble() < 0.12)
                    f.X = Math.Clamp(f.X + rnd.Next(-1, 2), 0, Console.WindowWidth - 1);
            }

            // Rita nya
            foreach (var f in flakes)
            {
                if (f.Y >= top && f.Y < Console.WindowHeight)
                {
                    Console.SetCursorPosition(Math.Min(Console.WindowWidth - 1, Math.Max(0, f.X)), Math.Min(Console.WindowHeight - 1, Math.Max(0, f.Y)));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(f.C);
                }
            }

            DrawBackgroundDecorations();
        }

        private void DrawBackgroundDecorations()
        {
            int top = Console.WindowHeight - Layout.BackgroundHeight;

            // Snowman (placera inuti bakgrunden)
            int sx = 2;
            int sy = top + Math.Max(1, Layout.BackgroundHeight / 2 - 2);
            SafeWrite(sx + 0, sy + 0, "   _===_  ");
            SafeWrite(sx + 0, sy + 1, "  (o_o)   ");
            SafeWrite(sx + 0, sy + 2, " /( : )\\ ");
            SafeWrite(sx + 0, sy + 3, "  ( : )  ");

            // Tree (höger)
            int tx = Math.Max(10, Console.WindowWidth - 18);
            int ty = top + Math.Max(0, Layout.BackgroundHeight / 2 - 4);
            SafeWrite(tx, ty + 0, "    *");
            SafeWrite(tx - 1, ty + 1, "   ***");
            SafeWrite(tx - 2, ty + 2, "  *****");
            SafeWrite(tx - 3, ty + 3, " *******");
            SafeWrite(tx - 1, ty + 4, "  |||");
        }

        private void SafeWrite(int x, int y, string text)
        {
            if (y < 0 || y >= Console.WindowHeight) return;
            int safeX = Math.Clamp(x, 0, Console.WindowWidth - 1);
            Console.SetCursorPosition(safeX, y);
            Console.Write(text.Length > Console.WindowWidth - safeX ? text.Substring(0, Console.WindowWidth - safeX) : text);
        }
    }
}
