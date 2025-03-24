using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;

namespace IronOS
{
    public class Taskbar
    {
        public void Draw(Canvas canvas)
        {
            Pen barPen = new Pen(Color.DarkGray);
            Pen textPen = new Pen(Color.White);

            // Draw taskbar background (bottom 20px of screen)
            for (int y = 580; y < 600; y++)
            {
                for (int x = 0; x < 800; x++)
                {
                    canvas.DrawPoint(barPen, x, y);
                }
            }

            // Draw live clock in bottom-right corner
            string timeStr = DateTime.Now.ToString("HH:mm:ss");
            canvas.DrawString(timeStr, PCScreenFont.Default, textPen, 700, 585);

            // Optional debug text (bottom-left)
            canvas.DrawString("Taskbar", PCScreenFont.Default, textPen, 10, 585);
        }
    }
}
