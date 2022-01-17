using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Shmup
{
    internal static class Player
    {
        /// <summary>
        /// Static class as there is only 1 player
        /// </summary>
        #region Private class variables
        private static float Speed;
        private static RectangleF Rectangle;
        private static Color Colour;
        #endregion
        #region Public Methods
        public static void CreatePlayer(int w, int h, float speed, Color colour)
        {
            /// This method is used to set the player rectangle width, height and colour
            /// It is similar to a constructor used on a non-static cless
            /// C# does have a proper static constructor, called when the first reference to the class is made
            /// but it is not used here, to replicate Lua and Python ways of doing things.
            Rectangle = new RectangleF(0, 0, w, h);
            Rectangle.Position = new Vector2((Shared.WIDTH - Rectangle.Width) / 2, Shared.HEIGHT - Rectangle.Height - 10); // starting position
            Speed = speed;
            Colour = colour;
        }
        public static void Update(float dt, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))  // check for left arrow or 'A'
            {
                Rectangle.X -= Speed * dt;
                if (Rectangle.X < 0)    // Is the rectangle off-screen to the left?
                    Rectangle.X = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))  // check for right arrow or 'D'
            {
                Rectangle.X += Speed * dt;
                if (Rectangle.X > Shared.WIDTH - Rectangle.Width) // Is the rectangle off-screen to the right?
                    Rectangle.X = Shared.WIDTH - Rectangle.Width;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            // FillRectangle is from MonoGame.Extended
            spriteBatch.FillRectangle(rectangle:Rectangle, color:Colour);
        }
        #endregion
    }
}
