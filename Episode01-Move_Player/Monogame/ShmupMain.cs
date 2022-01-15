using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Shmup
{
    public class ShmupMain : Game
    {
        /// <summary>
        /// C# / Monogame equivalent of Python / Pygame Shmup:
        /// https://www.youtube.com/watch?v=nGufy7weyGY&t=91s
        /// </summary>
        #region Private Class variables
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        #endregion
        #region Constructor
        public ShmupMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        #endregion
        #region Inherited Methods
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Shared.WIDTH;
            graphics.PreferredBackBufferHeight = Shared.HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player.CreatePlayer();
            Shared.gameState = Shared.gameStates["play"];
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = ProcessEvents(out bool quit); // gets bool value for quit as well 
            if (quit || Shared.gameState == Shared.gameStates["quit"])
                Exit();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Player.Update(dt, keyboardState);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Player.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
        #region Custom methods
        private KeyboardState ProcessEvents(out bool quit)
        {
            /// This function returns the keyboard state directly
            /// The calling code has access to the quit variable in any following code
            /// Similar to Lua,Python: keyboardState, quit = ProcessEvents()
            quit = false;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                quit = true;
            return Keyboard.GetState();
        }
        #endregion
    }
}
