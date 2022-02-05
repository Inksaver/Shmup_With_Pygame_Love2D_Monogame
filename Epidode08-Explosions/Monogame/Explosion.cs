using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Shmup
{
    internal class Explosion
    {
        private List<Texture2D> Images = new List<Texture2D>();
        public Point2 Centre { get; set; }
        public float Scale { get; set; }
        public bool Active { get; set; }
        public RectangleF Rectangle;
        private Texture2D image;
        private int frame = 0;
        private float timePassed = 0;
        private float frameRate = 0.1f;
       
        public Explosion(List<Texture2D> imageList, Point2 centre, float scale)
        {
            Images = imageList;
            Centre = centre;
            Scale = scale;
            image = Images[0];
            Active = true;
            Rectangle = new RectangleF( Centre.X - image.Width / 2 * Scale,
                                        Centre.Y - image.Height / 2 * Scale,
                                        image.Width * Scale,
                                        image.Height * Scale);

        }
        public bool Update(float dt)
        {
            timePassed += dt;
            if (timePassed > frameRate)
            {
                timePassed = 0;
                frame += 1;
                if(frame >= Images.Count)
                {
                    frame = 0;
                    Active = false;
                }
                else
                {
                    image = Images[frame];
                    Rectangle = new RectangleF( Centre.X - image.Width / 2 * Scale,
                                                Centre.Y - image.Height / 2 * Scale,
                                                image.Width * Scale,
                                                image.Height * Scale);
                }
            }
            return Active;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw
                (
                    texture: image,
                    position: Rectangle.Position,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0.0f,
                    origin: Vector2.Zero,
                    scale: Scale,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
                if (Shared.Debug)
                {
                    spriteBatch.DrawRectangle(Rectangle, Color.Blue);
                }
            }
        }
    }
}
