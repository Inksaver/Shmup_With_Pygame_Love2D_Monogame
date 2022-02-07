using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Shmup
{
    internal class Mob
    {
        /// <summary>
        /// Mob class for generating meteors from top of window
        /// </summary>
        //public RectangleCollider Rectangle;
        #region Properties
        private RectangleF rectangle;
        private CircleF circle;
        public RectangleF Rectangle { get { return rectangle; } } // getter
        public CircleF Circle { get { return circle; } } // getter
        #endregion
        #region Class variables
        private Texture2D meteorImg;
        private Random random;
        private float speedY;
        private float speedX;
        private float rotation;
        private float rotationSpeed;
        private float radius;
        private Vector2 offset;
        #endregion
        #region Constructor
        public Mob(Texture2D meteorimg)
        {
            random = new Random();
            meteorImg = meteorimg;
            rectangle = new Rectangle(0, 0, meteorImg.Width, meteorImg.Height);
            radius = (float)(Math.Max(meteorImg.Width, meteorImg.Height) * 0.85 / 2);
            circle = new CircleF(new Point2(0, 0), radius);
            offset = new Vector2(meteorImg.Width / 2, meteorImg.Height / 2);
            Reset();
        }
        #endregion
        #region Custom Methods
        
        public void Reset()
        {
            rectangle.X = random.Next(0, Shared.WIDTH - (int)Rectangle.Width);
            rectangle.Y = random.Next(-150, -50);
            speedY = random.Next(50, 300);
            speedX = random.Next(-50, 50);
            if (Shared.Debug)
            {
                speedY = random.Next(30, 100);
                speedX = random.Next(-20, 20);
            }
            circle.Center = Rectangle.Center;
            rotation = 0f;
            rotationSpeed = (float)(random.Next(-5, 5) * 0.5) ;
        }
        private void Rotate(float dt)
        {
            rotation += rotationSpeed * dt;
            if (rotation > Math.PI * 2) rotation = 0;
        }
        private List<Vector2> GetRotatedVertices()
        {
            double r = Math.Sqrt(Rectangle.Width * Rectangle.Width / 4 + Rectangle.Height * Rectangle.Height / 4);
            List<double> thetas = new List<double> { Math.Atan((Rectangle.Height / 2) / (Rectangle.Width / 2)) };
            thetas.AddRange(new double[] {-thetas[0] + rotation,
                                           thetas[0] - Math.PI + rotation,
                                           Math.PI - thetas[0] + rotation});
            thetas[0] += rotation;
            List<Vector2> vertices = new List<Vector2>();
            foreach (double theta in thetas)
            {
                vertices.Add(new Vector2((float)(Math.Cos(theta) * r), (float)(Math.Sin(theta) * r)));
            }
            return vertices;
        }
        public void Update(float dt)
        {
            Rotate(dt);
            rectangle.X += speedX * dt;
            rectangle.Y += speedY * dt;
            circle.Center = Rectangle.Center;
            if (Rectangle.Y > Shared.HEIGHT || Rectangle.X < 0 - Rectangle.Width || Rectangle.X > Shared.WIDTH)
                Reset();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
            (
                texture: meteorImg,
                position: Rectangle.Center,
                sourceRectangle: null,
                color: Color.White,
                rotation: rotation,
                origin: offset,
                scale: 1.0f,
                effects: SpriteEffects.None,
                layerDepth: 1.0f
            );
            if (Shared.Debug)
            {
                spriteBatch.DrawCircle(circle: Circle, sides: 20, color: Color.Red, thickness: 1);
                spriteBatch.DrawRectangle(Rectangle, Color.Blue);
                spriteBatch.DrawPolygon(Rectangle.Center, GetRotatedVertices(), Shared.GREEN);
            }
        }
        #endregion
    }
}
