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
        #region Class variables
        public static RectangleF Rectangle;
        public static CircleF Circle;
        private static float radius = 0f;
        private static float speed = 10;
        private static Texture2D playerImg;
        private static float scale = 0f;
        #endregion
        #region Public Methods
        public static void CreatePlayer(Texture2D playerimg, float scl, float spd)
        {
            playerImg = playerimg;
            scale = scl;
            speed = spd;
            Rectangle = new RectangleF
            (
                Shared.WIDTH / 2 - (playerImg.Width / 2), 
                Shared.HEIGHT - playerImg.Height * scale - 10,
                playerImg.Width * scale,
                playerImg.Height * scale
            );
            radius = MathHelper.Min(playerImg.Width, playerImg.Height) / 2 * scale;
            Circle = new CircleF(Rectangle.Center, radius);
        }
        public static void Update(KeyboardState keyboardState, float dt)
        {
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Rectangle.X -= speed * dt;
                if (Rectangle.X < 0)
                    Rectangle.X = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Rectangle.X += speed * dt;
                if (Rectangle.X > Shared.WIDTH - Rectangle.Width)
                    Rectangle.X = Shared.WIDTH - Rectangle.Width;
            }
            Circle.Position = Rectangle.Center;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
            (
                texture: playerImg,
                position: Rectangle.Position,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0.0f,
                origin: Vector2.Zero,
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
            spriteBatch.DrawRectangle(Rectangle, Color.Blue);
            spriteBatch.DrawCircle(circle: Circle, sides: 20, color: Color.Red, thickness: 1);
        }
        #endregion
    }
}
