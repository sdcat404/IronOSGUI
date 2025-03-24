
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.System.Graphics.Fonts;

namespace IronOS
{
    public class Icon
    {
        private string label;
        private int x, y;
        private int size = 40;
        private Pen iconPen = new Pen(Color.LightGray);
        private Pen textPen = new Pen(Color.White);

        public Icon(string label, int x, int y)
        {
            this.label = label;
            this.x = x;
            this.y = y;
        }

        public void Draw(Canvas canvas)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    canvas.DrawPoint(iconPen, x + i, y + j);
                }
            }
            canvas.DrawString(label, PCScreenFont.Default, textPen, x + 5, y + size + 2);
        }

        public bool IsHovered(int mouseX, int mouseY)
        {
            return mouseX >= x && mouseX <= x + size &&
                   mouseY >= y && mouseY <= y + size;
        }

        public void CheckClick(int mouseX, int mouseY, bool clicked)
        {
            if (clicked && IsHovered(mouseX, mouseY))
            {
                // Future: launch app
            }
        }
    }
}
