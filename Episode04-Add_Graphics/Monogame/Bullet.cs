using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Shmup
{
    internal class Bullet
    {
        /// <summary>
        /// Mob class for generating meteors from top of window
        /// </summary>

		#region Class variables
        public bool Active { get; set; }
        public RectangleF Rectangle;
        private Texture2D bulletImg;
        private float speedY;
        #endregion
        #region Constructor
        public Bullet(Texture2D bulletimg, float speed)
        {
            bulletImg = bulletimg;
            Rectangle = new RectangleF
            (
                Player.Rectangle.X + (Player.Rectangle.Width - bulletImg.Width) / 2,
                Player.Rectangle.Y - bulletImg.Height,
                bulletImg.Width,
                bulletImg.Height
            );
            speedY = speed;
            if (Shared.Debug) speedY = speed / 2;
            Active = true;
        }
        #endregion
        #region Class Methods
        public bool Update(float dt)
        {
            Rectangle.Y -= speedY * dt;
            if (Rectangle.Y < 0 - Rectangle.Height)
                Active = false;
            return Active;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Shared.Debug)
                spriteBatch.DrawRectangle(Rectangle, Color.Yellow); //debug check image position
            else
                spriteBatch.Draw(bulletImg, Rectangle.Position, Color.White);
        }
        #endregion
    }
}
