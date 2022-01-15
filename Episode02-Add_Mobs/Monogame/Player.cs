using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Shmup
{
    internal static class Player
    {
        /// <summary>
        /// Static class for Player as there is only 1
        /// </summary>
        #region Private class variables
        private static float Speed = 0;
        private static RectangleF Rectangle;
        #endregion
        #region Public Methods
        public static void CreatePlayer(float speed)
        {
            Speed = speed;
            Rectangle = new RectangleF(0, 0, 50, 40);
            Rectangle.Position = new Vector2((Shared.WIDTH - Rectangle.Width) / 2, Shared.HEIGHT - Rectangle.Height - 10); // starting position
        }
        public static void Update(float dt, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Rectangle.X -= Speed * dt;
                if (Rectangle.X < 0)
                    Rectangle.X = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Rectangle.X += Speed * dt;
                if (Rectangle.X > Shared.WIDTH - Rectangle.Width)
                    Rectangle.X = Shared.WIDTH - Rectangle.Width;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(rectangle: Rectangle, color: Color.Green);
        }
        #endregion
    }
}
