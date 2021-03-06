using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Shmup
{
    public class ShmupMain : Game
    {
        /// <summary>
        /// C# / Monogame equivalent of Python / Pygame Shmup Episodes 5/6:
        /// https://www.youtube.com/watch?v=_y5U8tB36Vk
        /// https://www.youtube.com/watch?v=3Bk-Ny7WLzE
        /// Add remaining images into img folder
        /// Meteors will be rotated
        /// Multiple meteor images are randomly chosen
        /// Introduction of circle collider
        /// Draw circles around images
        /// 
        /// IMPORTANT Add Monogame.Extended via Nuget UI
        /// 
        /// </summary>
        #region Private Class variables
        private GraphicsDevice graphicsDevice;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<Mob> Mobs;
        private List<Bullet> Bullets;
        private List<Texture2D> MeteorImages;
        private Texture2D bgImg;
        private Texture2D playerImg;
        private Texture2D bulletImg;
        private Random random;
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
                    //destroy = Bullets[j].Rectangle.Intersects(Mobs[i].Rectangle);
                    destroy = Bullets[j].Circle.Intersects(Mobs[i].Circle);
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
                if(!Bullets[i].Update(dt))
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
                //if (Mobs[i].Rectangle.Intersects(Player.Rectangle))
                if (Mobs[i].Circle.Intersects(Player.Circle) && !Shared.Debug)
                    Shared.GameState = Shared.GameStates["quit"];
            }
        }
        public void Shoot()
        {
            /// fire bullet if enough time has passed
            if (allowNewBullet)
            {
                Bullets.Add(new Bullet(bulletImg, 1000));
                allowNewBullet = false; // prevent new bullets being made
                newBulletTimer = 0;     // reset newBulletTimer to 0
            }
        }
        private void LoadImages()
        {
            bgImg = Content.Load<Texture2D>("img/starfield");
            bulletImg = Content.Load<Texture2D>("img/laserRed16");
            playerImg = Content.Load<Texture2D>("img/playerShip1_orange");
            MeteorImages = new List<Texture2D>
            {
                Content.Load<Texture2D>("img/meteorBrown_med1"),
                Content.Load<Texture2D>("img/meteorBrown_med3"),
                Content.Load<Texture2D>("img/meteorBrown_small1"),
                Content.Load<Texture2D>("img/meteorBrown_small2"),
                Content.Load<Texture2D>("img/meteorBrown_tiny1"),
                Content.Load<Texture2D>("img/meteorBrown_big1"),
                Content.Load<Texture2D>("img/meteorBrown_big2")
            };
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
            Mobs = new List<Mob>();
            Bullets = new List<Bullet>();
            LoadImages();
            Player.CreatePlayer(playerimg: playerImg, scl: 0.5f, spd: 300f);
            random = new Random();
            // make 8 meteor Mobs from random images
            
            for (int i = 0; i < 8; i++)
            {
                if (i < MeteorImages.Count) // at least 1 of each type
                    Mobs.Add(new Mob(MeteorImages[i]));
                else // add 2 random choices
                    Mobs.Add(new Mob(MeteorImages[random.Next(0, MeteorImages.Count)]));
            }
            Shared.GameState = Shared.GameStates["play"];
            Shared.Debug = true;
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
            else if(Shared.GameState == Shared.GameStates["play"])
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
            spriteBatch.Begin();
            spriteBatch.Draw(bgImg, new Vector2(0, 0), Color.White);
			if(Shared.GameState == Shared.GameStates["play"])
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
