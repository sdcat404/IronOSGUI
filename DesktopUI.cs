
using System;
using System.Threading;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System.Drawing;

namespace IronOS
{
    public class DesktopUI
    {
        private Canvas canvas;
        private int mouseX = 320;
        private int mouseY = 240;
        private int prevMouseX = 320;
        private int prevMouseY = 240;
        private string lastTime = "";
        private bool terminalOpen = false;
        private ConsoleKeyEx? lastKey = null;
        private DateTime lastKeyTime = DateTime.MinValue;

        private readonly Pen bluePen = new Pen(Color.Blue);
        private readonly Pen whitePen = new Pen(Color.White);
        private readonly Pen yellowPen = new Pen(Color.Yellow);
        private readonly Pen taskbarPen = new Pen(Color.DarkGray);
        private readonly Pen clearClockPen = new Pen(Color.DarkGray);
        private readonly Pen terminalPen = new Pen(Color.Black);
        private readonly Pen terminalBorderPen = new Pen(Color.White);
        private readonly Pen terminalButtonPen = new Pen(Color.LightGray);

        private Rectangle terminalButton = new Rectangle(10, 462, 60, 16);
        private Rectangle terminalWindow = new Rectangle(100, 100, 440, 280);

        public void Start()
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas();
            canvas.Mode = new Mode(640, 480, ColorDepth.ColorDepth32);

            DrawBackground();
            DrawTaskbar();
            UpdateClock();
            DrawTerminalButton();
            DrawCursor();

            while (true)
            {
                bool moved = HandleKeyInput();

                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                if (currentTime != lastTime)
                {
                    UpdateClock();
                    lastTime = currentTime;
                }

                if (moved)
                {
                    EraseCursor(prevMouseX, prevMouseY);
                    DrawCursor();
                    prevMouseX = mouseX;
                    prevMouseY = mouseY;
                }

                if (KeyboardManager.TryReadKey(out var key))
                {
                    if (key.Key == ConsoleKeyEx.Spacebar)
                    {
                        if (IsCursorOver(terminalButton))
                        {
                            terminalOpen = true;
                            DrawTerminalWindow();
                        }
                        else if (terminalOpen && IsCursorOver(new Rectangle(terminalWindow.X + terminalWindow.Width - 20, terminalWindow.Y, 16, 16)))
                        {
                            terminalOpen = false;
                            ClearTerminalWindow();
                        }
                    }
                }

                Thread.Sleep(15);
            }
        }

        private void DrawBackground()
        {
            canvas.Clear(Color.Blue);
            canvas.DrawString("Iron OS v0.1.1", PCScreenFont.Default, whitePen, 10, 10);
        }

        private void DrawTaskbar()
        {
            canvas.DrawFilledRectangle(taskbarPen, 0, 460, 640, 20);
        }

        private void DrawTerminalButton()
        {
            canvas.DrawFilledRectangle(terminalButtonPen, terminalButton.X, terminalButton.Y, terminalButton.Width, terminalButton.Height);
            canvas.DrawString("Terminal", PCScreenFont.Default, whitePen, terminalButton.X + 2, terminalButton.Y + 2);
        }

        private void DrawTerminalWindow()
        {
            canvas.DrawFilledRectangle(terminalPen, terminalWindow.X, terminalWindow.Y, terminalWindow.Width, terminalWindow.Height);
            canvas.DrawRectangle(terminalBorderPen, terminalWindow.X, terminalWindow.Y, terminalWindow.Width, terminalWindow.Height);
            canvas.DrawString("IronOS Terminal", PCScreenFont.Default, whitePen, terminalWindow.X + 10, terminalWindow.Y + 10);
            canvas.DrawString("> Welcome to the Terminal!", PCScreenFont.Default, whitePen, terminalWindow.X + 10, terminalWindow.Y + 30);

            // Draw close [X] button
            canvas.DrawFilledRectangle(taskbarPen, terminalWindow.X + terminalWindow.Width - 20, terminalWindow.Y, 16, 16);
            canvas.DrawString("X", PCScreenFont.Default, whitePen, terminalWindow.X + terminalWindow.Width - 18, terminalWindow.Y + 2);
        }

        private void ClearTerminalWindow()
        {
            canvas.DrawFilledRectangle(bluePen, terminalWindow.X - 1, terminalWindow.Y - 1, terminalWindow.Width + 2, terminalWindow.Height + 2);
            DrawTaskbar();
            UpdateClock();
            DrawTerminalButton();
        }

        private bool HandleKeyInput()
        {
            bool moved = false;
            if (KeyboardManager.TryReadKey(out var key))
            {
                lastKey = key.Key;
                lastKeyTime = DateTime.Now;
            }

            if (lastKey != null && (DateTime.Now - lastKeyTime).TotalMilliseconds < 200)
            {
                switch (lastKey)
                {
                    case ConsoleKeyEx.UpArrow: mouseY = Math.Max(0, mouseY - 4); moved = true; break;
                    case ConsoleKeyEx.DownArrow: mouseY = Math.Min(479, mouseY + 4); moved = true; break;
                    case ConsoleKeyEx.LeftArrow: mouseX = Math.Max(0, mouseX - 4); moved = true; break;
                    case ConsoleKeyEx.RightArrow: mouseX = Math.Min(639, mouseX + 4); moved = true; break;
                }
            }

            return moved;
        }

        private void UpdateClock()
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            canvas.DrawFilledRectangle(clearClockPen, 530, 465, 100, 10);
            canvas.DrawString(time, PCScreenFont.Default, whitePen, 530, 465);
        }

        private void DrawCursor()
        {
            canvas.DrawPoint(yellowPen, mouseX, mouseY);
            canvas.DrawPoint(yellowPen, mouseX + 1, mouseY);
            canvas.DrawPoint(yellowPen, mouseX, mouseY + 1);
            canvas.DrawPoint(yellowPen, mouseX + 1, mouseY + 1);
        }

        private void EraseCursor(int x, int y)
        {
            canvas.DrawFilledRectangle(bluePen, x, y, 2, 2);
        }

        private bool IsCursorOver(Rectangle rect)
        {
            return mouseX >= rect.X && mouseX <= rect.X + rect.Width &&
                   mouseY >= rect.Y && mouseY <= rect.Y + rect.Height;
        }
    }

    public struct Rectangle
    {
        public int X, Y, Width, Height;
        public Rectangle(int x, int y, int w, int h)
        {
            X = x; Y = y; Width = w; Height = h;
        }
    }
}
