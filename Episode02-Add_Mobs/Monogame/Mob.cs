using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Shmup
{
    internal class Mob
    {
        /// <summary>
        /// Mob class for generating meteors from top of window
        /// </summary>
        #region Private Class variables
        private RectangleF Rectangle;
        private float SpeedY;
        private float SpeedX;
        private Color Colour;
        private Random random;
        #endregion
        #region Constructor
        public Mob(int width, int height, Color colour)
        {
            random = new Random();
            Rectangle = new RectangleF(0, 0, width, height);
            Colour = colour;
            SetProperties();
        }
        #endregion
        #region Class Methods
        public void Update(float dt)
        {
            Rectangle.X += SpeedX * dt;
            Rectangle.Y += SpeedY * dt;
            if (Rectangle.Y > Shared.HEIGHT || Rectangle.X < 0 - Rectangle.Width || Rectangle.X > Shared.WIDTH)
                SetProperties();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle( rectangle: Rectangle, color: Colour);
        }
        private void SetProperties()
        {
            Rectangle.X = random.Next(0, Shared.WIDTH - (int)Rectangle.Width);
            Rectangle.Y = random.Next(-150, -50);
            Rectangle.Y = 0;
            SpeedY = random.Next(50, 300);
            SpeedX = random.Next(-50, 50);
        }
        #endregion
    }
}
