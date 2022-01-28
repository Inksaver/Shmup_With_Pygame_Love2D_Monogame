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
        #region Class variables
        private GraphicsDevice graphicsDevice;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<Mob> Mobs;
        private List<Bullet> Bullets;
        private float newBulletTimerInterval = 0.2f;    // allow a new bullet every 0.2 seconds
        private float newBulletTimer = 0.0f;	        // start timer at 0, update(dt) increases its value
        private bool allowNewBullet = true;             // change to false as soon as a new bullet is added to the bulletList
        #endregion
        #region Constructor
        public ShmupMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        #endregion
        #region Custom Methods
        private KeyboardState ProcessEvents(out bool quit)
        {
            quit = false;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                quit = true;
            return Keyboard.GetState();
        }
        private void CheckMobBulletCollisions(float dt)
        {
            /// Check if any bullets are colliding with any Mobs
            /// If so, re-deploy Mob and delete the bullet
            for (int i = Mobs.Count - 1; i >= 0; i--)           // outer loop checks mobs
            {
                bool destroy = false;                           // local boolean set to false
                for (int j = Bullets.Count - 1; j >= 0; j--)    // inner loop checks Bullets
                {
                    destroy = Bullets[j].Rectangle.Intersects(Mobs[i].Rectangle);
                    if (destroy)
                    {
                        Mobs[i].SetProperties();                // re-deploy mob first
                        Bullets.RemoveAt(j);
                    }
                }
            }
            // Update remaining bullets
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                if (!Bullets[i].Update(dt))
                    Bullets.RemoveAt(i);
            }
        }
        private void CheckMobPlayerCollisions(KeyboardState keyboardState, float dt)
        {
            /// Update player and all mobs.
            Player.Update(keyboardState, dt);
            /// check if any mobs are colliding with the player ///
            for (int i = Mobs.Count - 1; i >= 0; i--)
            {
                Mobs[i].Update(dt);
                if (Mobs[i].Rectangle.Intersects(Player.Rectangle))
                    if(!Shared.Debug)
                        Shared.GameState = Shared.GameStates["quit"];
            }
        }
        public void Shoot()
        {
            /// fire bullet if enough time has passed
            if (allowNewBullet)
            {
                Bullets.Add(new Bullet(graphicsDevice));
                allowNewBullet = false; // prevent new bullets being made
                newBulletTimer = 0;     // reset newBulletTimer to 0
            }
        }
        #endregion
        #region Inherited Procedures
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Shared.WIDTH;
            graphics.PreferredBackBufferHeight = Shared.HEIGHT;
            graphics.ApplyChanges();
            graphicsDevice = GraphicsDevice;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player.CreatePlayer(w: 40, h: 50, spd: 500f, clr: Shared.GREEN);
            Bullets = new List<Bullet>();
            Mobs = new List<Mob>();
            for (int i = 0; i < 8; i++)
            {
                Mobs.Add(new Mob(30, 40, Color.Red));
            }
            Shared.Debug = true;
            Shared.GameState = Shared.GameStates["play"];
        }
        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            newBulletTimer = newBulletTimer + dt;
            if (newBulletTimer >= newBulletTimerInterval)
            {
                allowNewBullet = true;      // new bullet can be created
                newBulletTimer = 0;         // reset newBulletTimer to 0
            }
            KeyboardState keyboardState = ProcessEvents(out bool quit); // gets bool value for quit as well 
            if (quit || Shared.GameState == Shared.GameStates["quit"])
                Exit();
            else if (Shared.GameState == Shared.GameStates["play"])
            {
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space))
                    Shoot();
                CheckMobPlayerCollisions(keyboardState, dt);
                CheckMobBulletCollisions(dt); 
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (Shared.GameState == Shared.GameStates["play"])
            {
                Player.Draw(spriteBatch);
                foreach (Bullet bullet in Bullets)
                {
                    bullet.Draw(spriteBatch);
                }
                foreach (Mob mob in Mobs)
                {
                    mob.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}
