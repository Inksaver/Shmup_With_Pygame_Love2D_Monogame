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
        #region Class variables
        public RectangleF Rectangle;
        private float speedY;
        private float speedX;
        private Color colour;
        private Random random;
        #endregion
        #region Constructor
        public Mob(int width, int height, Color clr)
        {
            random = new Random();
            Rectangle = new RectangleF(0, 0, width, height);
            colour = clr;
            SetProperties();
        }
        #endregion
        #region Class Methods
        public void SetProperties()
        {
            Rectangle.X = random.Next(0, Shared.WIDTH - (int)Rectangle.Width);
            Rectangle.Y = random.Next(-150, -50);
            Rectangle.Y = 0;
            speedY = random.Next(150, 600);
            speedX = random.Next(-50, 50);
        }
        public void Update(float dt)
        {
            Rectangle.X += speedX * dt;
            Rectangle.Y += speedY * dt;
            if (Rectangle.Y > Shared.HEIGHT || Rectangle.X < 0 - Rectangle.Width || Rectangle.X > Shared.WIDTH)
                SetProperties();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle( rectangle: Rectangle, color: colour);
        }
        #endregion
    }
}
