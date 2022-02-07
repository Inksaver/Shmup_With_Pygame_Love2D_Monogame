using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Shmup
{
    internal class Explosion
    {
        // No public properties required
        #region Class variables
        private RectangleF rectangle;
        private Point2 centre;
        private float scale;
        private Texture2D image;
        private int frame = 0;
        private float timePassed = 0;
        private float frameRate = 0.1f;
        private bool active;
        private List<Texture2D> images = new List<Texture2D>();
        #endregion
        #region Constructor
        public Explosion(List<Texture2D> imageList, Point2 cntr, float scl)
        {
            images = imageList;
            centre = cntr;
            scale = scl;
            image = images[0];
            active = true;
            rectangle = new RectangleF( centre.X - image.Width / 2 * scale,
                                        centre.Y - image.Height / 2 * scale,
                                        image.Width * scale,
                                        image.Height * scale);

        }
        #endregion
        #region Methods
        public bool Update(float dt)
        {
            timePassed += dt;
            if (timePassed > frameRate)
            {
                timePassed = 0;
                frame += 1;
                if(frame >= images.Count)
                    active = false;
                else
                {
                    image = images[frame];
                    rectangle = new RectangleF( centre.X - image.Width / 2 * scale,
                                                centre.Y - image.Height / 2 * scale,
                                                image.Width * scale,
                                                image.Height * scale);
                }
            }
            return active;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
            (
                texture: image,
                position: rectangle.Position,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0.0f,
                origin: Vector2.Zero,
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }
        #endregion
    }
}
