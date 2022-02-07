using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Shmup
{
    internal class Bullet
    {
        /// <summary>
        /// Mob class for generating meteors from top of window
        /// </summary>
        #region Properties
        private RectangleF rectangle;
        private CircleF circle;
        public RectangleF Rectangle  { get { return rectangle; } } 
        public CircleF Circle { get { return circle; } }
        #endregion
        #region Class variables
        private bool active;
        private Texture2D bulletImg;
        private float speedY;
        private float radius;
        #endregion
        #region Constructor
        public Bullet(Texture2D bulletimg, string align = "centre")
        {
            bulletImg = bulletimg;
            /// Create a rectangle based on Player position, assuming centre firing point
            rectangle = new RectangleF
            (
                Player.Rectangle.X + (Player.Rectangle.Width - bulletImg.Width)  / 2,
                Player.Rectangle.Y - bulletImg.Height,
                bulletImg.Width,
                bulletImg.Height
            );
            /// Adjust rectangle for left or right firing point
            if (align == "left")
                rectangle.X = Player.Rectangle.X;
            else if (align == "right")
                rectangle.X = Player.Rectangle.Right - bulletImg.Width;
            radius = MathHelper.Min(Rectangle.Width, Rectangle.Height) / 2;
            circle = new CircleF(Rectangle.Center, radius);
            speedY = 1000;
            if (Shared.Debug) speedY = speedY / 2;
            active = true; // Not Active allows removal of this object in main Update
        }
        #endregion
        #region Class Methods
        public bool Update(float dt)
        {
            rectangle.Y -= speedY * dt;
            circle.Center.Y = Rectangle.Y;
            if (Rectangle.Y < 0 - Rectangle.Height)
                active = false;
            return active;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Shared.Debug)
            {
                spriteBatch.DrawCircle(circle: Circle, sides: 20, color: Color.Red, thickness: 1);
                spriteBatch.DrawRectangle(Rectangle, Color.Yellow);
            }
            else
                spriteBatch.Draw(bulletImg, Rectangle.Position, Color.White);
        }
        #endregion
    }
}
