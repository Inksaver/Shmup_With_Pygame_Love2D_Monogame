using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Shmup
{
    internal static class Shield
    {
        public static float Percent = 100f;
        private static int barLength = 100;
        private static int barHeight = 10;
        private static Color lineColor = Color.White;
        private static Color fillColor = new Color(0, 0, 12);
        private static RectangleF outlineRect = new RectangleF(5, 5, barLength, barHeight);
        private static RectangleF fillRect = new RectangleF(5, 5, barLength, barHeight);
        private static RectangleF barRect = new RectangleF(5, 5, barLength, barHeight);

        public static void Update(float value)
        {
            if(value < 0) value = 0;
            Percent = value;
            fillRect.Width = (Percent / 100) * barLength;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(barRect, fillColor);
            spriteBatch.FillRectangle(fillRect, Shared.GREEN);
            spriteBatch.DrawRectangle(outlineRect, lineColor);
        }
    }
}
