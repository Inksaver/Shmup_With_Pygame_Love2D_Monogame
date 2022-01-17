using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Shmup
{
    public class ShmupMain : Game
    {
        /// <summary>
        /// C# / Monogame equivalent of Python / Pygame Shmup Episode 2:
        /// https://www.youtube.com/watch?v=-5GNbL33hz0
        /// </summary>
        #region Private Class variables
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<Mob> Mobs;
        #endregion
        #region Constructor
        public ShmupMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
        #region Inherited Procedures
        protected override void Initialize()
        {
            /// set the window width and height to values hard-coded in Shared.cs
			graphics.PreferredBackBufferWidth = Shared.WIDTH;
            graphics.PreferredBackBufferHeight = Shared.HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player.CreatePlayer(w:40, h:50, speed:500f, colour:Shared.GREEN);
            Mobs = new List<Mob>();
            for(int i = 0; i < 8; i++)
            {
                Mobs.Add(new Mob(30, 40, Color.Red));
            }
            Shared.gameState = Shared.gameStates["play"];
        }
        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = ProcessEvents(out bool quit); // gets bool value for quit as well 
            if (quit || Shared.gameState == Shared.gameStates["quit"])
                Exit();
            else if (Shared.gameState == Shared.gameStates["play"])
            {
                Player.Update(dt, Keyboard.GetState());
                for (int i = Mobs.Count - 1; i >= 0; i--)
                {
                    Mobs[i].Update(dt);
                }
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Player.Draw(spriteBatch);
            for (int i = Mobs.Count - 1; i >= 0; i--)
            {
                Mobs[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}
