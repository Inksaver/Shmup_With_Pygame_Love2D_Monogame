using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using System.Collections.Generic;
using System;

namespace Shmup
{
    public class ShmupMain : Game
    {
        /// <summary>
        /// Episode 12+13
        /// https://www.youtube.com/watch?v=z6h6l1yJ5-w
        /// https://www.youtube.com/watch?v=y2w-116htIQ
        /// Powerups
        /// 
        /// Background music:
        /// Frozen Jam by tgfcoder <https://twitter.com/tgfcoder> licensed under CC-BY-3 
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
        private Texture2D bulletImg;
        private Texture2D playerImg;
        private Random random;
        private float newBulletTimerInterval = 0.2f;    // allow a new bullet every 0.2 seconds
        private float newBulletTimer = 0.0f;	        // start timer at 0, update(dt) increases its value
        private bool allowNewBullet = true;             // change to false as soon as a new bullet is added to the bulletList
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, SoundEffectInstance> soundEffectInstances;
        private Song backgroundMusic;
        private List<Texture2D> explosionsMob;
        private List<Texture2D> explosionsPlayer;
        private List<Explosion> explosionList;
        private Dictionary<string,Texture2D> powerupImages;
        private List<Powerup> powerupList;
        private Explosion deathExplosion;

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
        private void DrawLives(float x, float y, int lives, float scale)
        {
            // draw up to 3 mini ships to represent lives left
	        for(int i = 0; i < lives; i++)
                spriteBatch.Draw
                (
                    texture: playerImg,
                    position: new Vector2(x + 30 * i, y),
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0.0f,
                    origin: Vector2.Zero,
                    scale: scale,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
        }
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
                        Shared.Score += (int)(Mobs[i].Circle.Radius);
                        soundEffectInstances["shoot"].Stop();
                        double rnd = random.NextDouble();
                        if(rnd > 0.9 || (Shared.Debug && rnd >0.1))
                        {
                            powerupList.Add(new Powerup(powerupImages, Mobs[i].Rectangle.Center));
                        }
                        if (Mobs[i].Circle.Radius < 15)
                            soundEffectInstances["expl1"].Play();
                        else
                            soundEffectInstances["expl2"].Play();
                        explosionList.Add(new Explosion(explosionsMob, Mobs[i].Rectangle.Center, 0.5f));
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
            if (Player.Alive)
                Player.Update(keyboardState, dt);
            else //  0 lives so allow for finish of explosion sound and animation
            {
                if (deathExplosion == null) //Player dead, explosion animation finished
                {
                    if (soundEffectInstances["die"].State == SoundState.Stopped)
                        Shared.GameState = Shared.GameStates["quit"];
                }
            }
            /// check if any mobs are colliding with the player ///
            for (int i = Mobs.Count - 1; i >= 0; i--)
            {
                Mobs[i].Update(dt);
                //if (Mobs[i].Rectangle.Intersects(Player.Rectangle))
                if (Mobs[i].Circle.Intersects(Player.Circle))
                {
                    Mobs[i].SetProperties();
                    Player.Shield -= Mobs[i].Circle.Radius;
                    soundEffectInstances["shoot"].Stop();
                    if (Player.Shield <= 0) // lose a life
                    {
                        Player.Lives -= 1;
                        Player.Shield = 100;
                        soundEffectInstances["die"].Play();
                        if (Player.Lives <= 0)
                        {
                            // Alive set to false only when all 3 lives used. Play full size death explosion
                            deathExplosion = new Explosion(explosionsPlayer, new Point2(Player.Rectangle.Center.X, Player.Rectangle.Top), 1.0f);
                            Player.Alive = false;
                        }
                        else
                        {
                            // Alive still true, so play half size death explosion and hide for 3 seconds
                            deathExplosion = new Explosion(explosionsPlayer, new Point2(Player.Rectangle.X - Player.Rectangle.Width, Player.Rectangle.Top), 0.5f);
                            Player.Hide();
                        }
                    }
                }   
            }
        }
        private void CheckPowerupPlayerCollisions(float dt)
        {
            /// Update and check any powerups colliding with Player	
            for (int i = powerupList.Count - 1; i >= 0; i--)
            {
                //if (powerupList[i].Rectangle.Intersects(Player.Rectangle))
                if (powerupList[i].Circle.Intersects(Player.Circle))
                {
                    soundEffectInstances["shoot"].Stop();
                    if (powerupList[i].Type == "shield")
                    {
                        soundEffectInstances["shield"].Play();
                        Player.Shield += random.Next(10, 30);
                        if (Player.Shield > 100)
                        {
                            Player.Shield = 100;
                        }
                    }
                    if (powerupList[i].Type == "gun")
                    {
                        soundEffectInstances["power"].Play();
                        Player.Powerup();
                    }
                    powerupList.RemoveAt(i);
                }
                else
                {
                    if(!powerupList[i].Update(dt))
                    {
                        powerupList.RemoveAt(i);
                    }
                }
            }
        }
        public void Shoot()
        {
            /// fire bullet if enough time has passed
            if (allowNewBullet && !Player.Hidden)
            {
                soundEffectInstances["shoot"].Stop();
                if(Player.Power == 1)
                    Bullets.Add(new Bullet(bulletImg, 1000, "centre"));
                else
                {
                    Bullets.Add(new Bullet(bulletImg, 1000, "left")); // left
                    Bullets.Add(new Bullet(bulletImg, 1000, "right")); // right
                }
                allowNewBullet = false; // prevent new bullets being made
                newBulletTimer = 0;     // reset newBulletTimer to 0
                soundEffectInstances["shoot"].Play();
            }
        }
        #endregion
        #region Inherited Procedures
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Shared.WIDTH;
            graphics.PreferredBackBufferHeight = Shared.HEIGHT;
            graphics.ApplyChanges();
            /// Getting the window onto a specific monitor is a real problem.
            /// This code is specific for displaying in the centre of a 1920x1080 monitor
            /// described as screen 2 and situated to the left of the main (screen 1) 3840x2160 monitor
            /// No success so far finding a generic method!!
            this.Window.Position = new Point( - (1920 + Shared.WIDTH) / 2 , (1080 - Shared.HEIGHT) / 2);
            graphicsDevice = GraphicsDevice;
            base.Initialize();
        }
        private void LoadAudio()
        {
            /// Populate soundEffects Dictionary.
            /// These sounds can be played directly:
            /// soundEffects["shoot"].Play()
            soundEffects = new Dictionary<string, SoundEffect>
            {
                { "shoot", Content.Load<SoundEffect>("snd/Laser_Shoot6")},
                { "shield", Content.Load<SoundEffect>("snd/pow4")},
                { "power", Content.Load<SoundEffect>("snd/pow5")},
                { "die", Content.Load<SoundEffect>("snd/rumble1")},
                { "expl1", Content.Load<SoundEffect>("snd/expl3")},
                { "expl2", Content.Load<SoundEffect>("snd/expl6")},
                { "expl3", Content.Load<SoundEffect>("snd/explosion5")}
            };
            /// Populate soundEffectsInstances Dictionary.
            /// These instances give greater conrtol
            /// soundEffectInstances["shoot"].Play();
            /// soundEffectInstances["shoot"].Stop();
            soundEffectInstances = new Dictionary<string, SoundEffectInstance>();
            foreach(KeyValuePair<string, SoundEffect> item in soundEffects)
            {
                soundEffectInstances.Add(item.Key, item.Value.CreateInstance());
                soundEffectInstances[item.Key].Volume = 0.2f;
            }
            //SoundEffectInstance instance = soundEffects["shoot"].CreateInstance();
            backgroundMusic = Content.Load<Song>("snd/FrozenJam");
            
            MediaPlayer.Volume = 0.2f; // 0 to 1 volume
            MediaPlayer.IsRepeating = true;
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
            powerupImages = new Dictionary<string, Texture2D>
            {
                { "shield", Content.Load<Texture2D>("img/shield_gold") },
                { "gun", Content.Load<Texture2D>("img/bolt_gold") }
            };
            powerupList = new List<Powerup>();
        }
        private void LoadFonts()
        {
            Shared.Fonts = new Dictionary<string, SpriteFont>
            {
                {"size14", Content.Load<SpriteFont>("font/arial14") },
                {"size16", Content.Load<SpriteFont>("font/arial16") },
                {"size18", Content.Load<SpriteFont>("font/arial18") },
                {"size20", Content.Load<SpriteFont>("font/arial20") },
                {"size22", Content.Load<SpriteFont>("font/arial22") },
                {"size24", Content.Load<SpriteFont>("font/arial24") },
                {"size30", Content.Load<SpriteFont>("font/arial30") },
                {"size50", Content.Load<SpriteFont>("font/arial50") },
                {"size64", Content.Load<SpriteFont>("font/arial64") },
            };
        }
        private void LoadExplosions()
        {
            explosionsMob = new List<Texture2D>();      // images for Mob explosion
            explosionsPlayer = new List<Texture2D>();   // images for Player explosion
            explosionList = new List<Explosion>();      // new explosions added as required
            for (int i = 0; i < 9; i++)
            {
                explosionsMob.Add(Content.Load<Texture2D>($"img/regularExplosion0{i}"));
                explosionsPlayer.Add(Content.Load<Texture2D>($"img/sonicExplosion0{i}"));
            }
        }
        private void UpdateExplosions(float dt)
        {
            for (int i = explosionList.Count - 1; i >= 0 ; i--)
            {
                if(!explosionList[i].Update(dt))
                {
                    explosionList.RemoveAt(i);
                }
            }
            if(deathExplosion != null)
            {
                if (!deathExplosion.Update(dt))
                    deathExplosion = null;
            }
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Mobs = new List<Mob>();
            Bullets = new List<Bullet>();
            LoadImages();
            LoadAudio();
            LoadFonts();
            LoadExplosions();
            Player.CreatePlayer(playerimg: playerImg, scl: 0.5f, spd: 300f);
            random = new Random();
            // make 8 meteor Mobs from random images
            for(int i = 0; i < 8; i++)
            {
                if (i < MeteorImages.Count) // at least 1 of each type
                    Mobs.Add(new Mob(MeteorImages[i]));
                else // add 2 random choices
                    Mobs.Add(new Mob(MeteorImages[random.Next(0, MeteorImages.Count)]));
            }
            Shared.Debug = true;
            if (!Shared.Debug)
                MediaPlayer.Play(backgroundMusic);
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
            else if(Shared.GameState == Shared.GameStates["play"])
            {
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space))
                    Shoot();
                UpdateExplosions(dt);
                CheckMobPlayerCollisions(keyboardState, dt);
                CheckMobBulletCollisions(dt);
                CheckPowerupPlayerCollisions(dt);
                Shield.Update(Player.Shield);
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
                foreach (Explosion explosion in explosionList)
                {
                    explosion.Draw(spriteBatch);
                }
                foreach (Powerup powerup in powerupList)
                {
                    powerup.Draw(spriteBatch);
                }
                if (deathExplosion != null)
                {
                    deathExplosion.Draw(spriteBatch);
                }
                DrawLives(Shared.WIDTH - 100, 5, Player.Lives, 0.2f);
                Shared.DrawString(spriteBatch, text: Shared.Score.ToString(), size: "size24", posX: 0, posY: 10, color: Color.White);
                Shield.Draw(spriteBatch);
            }
            if (Shared.Debug)
            {
                Shared.DrawString(spriteBatch: spriteBatch, text:"Debug mode", size:"size18", posX: 10, posY: Shared.HEIGHT - 24, color:Color.Yellow, align:"left" );
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion 
    }
}
