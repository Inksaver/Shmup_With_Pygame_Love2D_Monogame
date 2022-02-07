using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Shmup
{
    internal class Powerup
    {
        #region Public Properties
        private RectangleF rectangle;
        public RectangleF Rectangle { get { return rectangle; } }
        private CircleF circle;
        public CircleF Circle { get { return circle; } }
        public string Key { get; }
        #endregion
        #region Class Variables
        private Point2 centre;
        private bool active;
        private Texture2D image;
        private Random random = new Random();
        private float SpeedY = 500f;
        private Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>(); // list of 2 images
        #endregion
        public Powerup(Dictionary<string, Texture2D> imageDict, Point2 cntr)
        {
            images = imageDict;
            centre = cntr;
            List<string> indexes = new List<string>();
            foreach (KeyValuePair<string, Texture2D> kvp in images)
            {
                indexes.Add(kvp.Key); // ["shield", "gun"]
            }
            int index = random.Next(indexes.Count);
            Key = indexes[index];  // "shield" or "gun"
            image = images[Key];   // image with index "gun" or "shield"
            rectangle = new RectangleF(centre.X - image.Width / 2 , centre.Y - image.Height / 2, image.Width, image.Height);
            circle = new CircleF(rectangle.Center, Math.Min(rectangle.Width, rectangle.Height) / 2);
            if(Shared.Debug)
                SpeedY = 250f;
            active = true;
        }
        public bool Update(float dt)
        {
            rectangle.Y += SpeedY * dt;
            circle.Center = rectangle.Center;
            if (rectangle.Y > Shared.HEIGHT || rectangle.X < 0 - rectangle.Width || rectangle.X > Shared.WIDTH)
                active = false;

            return active;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
            (
                texture: image,
                position: rectangle.Position,
                sourceRectangle: null,
                color: Color.White
            );
            if (Shared.Debug)
            {
                spriteBatch.DrawRectangle(rectangle, Color.Blue);
            }
        }
    }
}
