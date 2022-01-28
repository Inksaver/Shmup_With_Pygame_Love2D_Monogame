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
        private float speedY;
        #endregion   
        #region Constructor
        public Bullet(GraphicsDevice graphicsDevice)
        {
            Rectangle = new RectangleF(0, 0, 10, 20);
            speedY = 1000;
            if(Shared.Debug)
                speedY = 500;
            Active = true;
            Rectangle.Position = new Vector2(Player.Rectangle.X + Player.Rectangle.Width / 2, Player.Rectangle.Y);
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
            spriteBatch.FillRectangle(rectangle: Rectangle, color: Color.Yellow);
        }
        #endregion
    }
}  
