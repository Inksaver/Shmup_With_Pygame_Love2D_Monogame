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
        private Texture2D meteorImg;
        private float speedY;
        private float speedX;
        private Random random;
        #endregion
        #region Constructor
        public Mob(GraphicsDevice graphicsDevice, Texture2D img)
        {
            random = new Random();
            meteorImg = img;
            Rectangle = new RectangleF(0, 0, img.Width, img.Height);
            SetProperties();
        }
        #endregion
        #region Class Methods
        public void SetProperties()
        {
            Rectangle.X = random.Next(0, Shared.WIDTH - (int)Rectangle.Width);
            Rectangle.Y = random.Next(-150, -50);
            speedY = random.Next(50, 300);
            speedX = random.Next(-50, 50);
            if (Shared.Debug)
            {
                speedY = random.Next(10, 100);
                speedX = random.Next(-20, 20);
            }
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
            spriteBatch.Draw(meteorImg, Rectangle.Position, Color.White);
            if(Shared.Debug)
                spriteBatch.DrawRectangle(Rectangle, Color.Blue); //debug check image position
        }
        #endregion
    }
}
