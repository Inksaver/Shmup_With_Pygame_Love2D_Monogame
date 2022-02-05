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
        public static bool Alive = true;
        public static RectangleF Rectangle;
        public static CircleF Circle;
        public static float Shield = 100f;
        public static int Lives = 3;
        public static bool Hidden = false;
        private static float radius = 0f;
        private static float speed = 10;
        private static Texture2D playerImg;
        private static float scale = 0f;
        private static float hideTimer = 0f;
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
            Circle = new CircleF(Rectangle.Position, radius);
        }
        public static void Hide()
        {
            Hidden = true;
            hideTimer = 0f;
            Rectangle.X = Shared.WIDTH / 2;
            Rectangle.Y = Shared.HEIGHT + 200;
        }
        public static void Update(KeyboardState keyboardState, float dt)
        {
            // unhide if hidden
            if (Hidden)
            {
                hideTimer += dt;
                if (hideTimer > 2) //  restore to centre after 2 second
                {
                    Hidden = false;
                    Rectangle.X = Shared.WIDTH / 2 - Rectangle.Width / 2;
                    Rectangle.Y = Shared.HEIGHT - Rectangle.Height - 10;
                }
            }
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
            if (Shared.Debug)
            {
                spriteBatch.DrawRectangle(Rectangle, Color.Blue);
                spriteBatch.DrawCircle(circle: Circle, sides: 20, color: Color.Red, thickness: 1);
            }
        }
        #endregion
    }
}
