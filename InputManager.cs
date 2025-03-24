
using Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.System.Graphics.Fonts;

namespace IronOS
{
    public class InputManager
    {
        public int MouseX = 400;
        public int MouseY = 300;
        public bool Clicked = false;

        public void Update()
        {
            Clicked = false;

            if (KeyboardManager.TryReadKey(out var key))
            {
                switch (key.Key)
                {
                    case ConsoleKeyEx.UpArrow: MouseY = System.Math.Max(0, MouseY - 5); break;
                    case ConsoleKeyEx.DownArrow: MouseY = System.Math.Min(599, MouseY + 5); break;
                    case ConsoleKeyEx.LeftArrow: MouseX = System.Math.Max(0, MouseX - 5); break;
                    case ConsoleKeyEx.RightArrow: MouseX = System.Math.Min(799, MouseX + 5); break;
                    case ConsoleKeyEx.Spacebar: Clicked = true; break;
                }
            }
        }

        public void DrawCursor(Canvas canvas)
        {
            Pen cursorPen = new Pen(Color.Yellow);
            canvas.DrawPoint(cursorPen, MouseX, MouseY);
            canvas.DrawPoint(cursorPen, MouseX + 1, MouseY);
            canvas.DrawPoint(cursorPen, MouseX, MouseY + 1);
            canvas.DrawPoint(cursorPen, MouseX + 1, MouseY + 1);
        }
    }
}
