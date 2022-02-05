using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Shmup
{
    internal class Powerup
    {
        public Point2 Centre { get; set; }
        public bool Active { get; set; }
        public RectangleF Rectangle;
        public string Type = "";
        public CircleF Circle;
        private Texture2D image;
        private Random random = new Random();
        private float SpeedY = 400f;
        private Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>(); // list of 2 images

        public Powerup(Dictionary<string, Texture2D> imageDict, Point2 centre)
        {
            Images = imageDict;
            Centre = centre;
            List<string> indexes = new List<string>();
            foreach (KeyValuePair<string, Texture2D> kvp in Images)
            {
                indexes.Add(kvp.Key);
            }
            int index = random.Next(indexes.Count);
            Type = indexes[index];
            image = Images[Type];
            Active = true;
            Rectangle = new RectangleF(Centre.X - image.Width / 2 , Centre.Y - image.Height / 2, image.Width, image.Height);
            Circle = new CircleF(Rectangle.Center, Math.Min(Rectangle.Width, Rectangle.Height) / 2);
            if(Shared.Debug)
                SpeedY = 200f;
        }
        public bool Update(float dt)
        {
            Rectangle.Y += SpeedY * dt;
            if (Rectangle.Y > Shared.HEIGHT || Rectangle.X < 0 - Rectangle.Width || Rectangle.X > Shared.WIDTH)
                Active = false;
            Circle.Center = Rectangle.Center;

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
                    color: Color.White
                );
                if (Shared.Debug)
                {
                    spriteBatch.DrawRectangle(Rectangle, Color.Blue);
                }
            }
        }
    }
}
