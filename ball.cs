using System.Numerics;
using SFML.Graphics;
using SFML.System;

namespace Breakout
{
    public class Ball
    {
        public Sprite sprite;
        public const float diameter = 20.0f;
        public const float radius = diameter * 0.5f;
        public float speed = 100.0f;
        public Vector2f velocity = new Vector2f(1, 1) / MathF.Sqrt(2.0f); //direction. Uhm no :P
        public Ball()
        {
            sprite = new Sprite();
            sprite.Texture = new Texture("./assets/ball.png");
            sprite.Position = new Vector2f(250, 300);
            velocity *= speed;
            Vector2f ballTextureSize = (Vector2f)sprite.Texture.Size;
            sprite.Origin = 0.5f * ballTextureSize;
            sprite.Scale = new Vector2f(
                diameter / ballTextureSize.X,
                diameter / ballTextureSize.Y);
        }
        public void Update(float deltaTime)
        {
            var newPos = sprite.Position;
            newPos += velocity * deltaTime;
            sprite.Position = newPos;
            
            
        }

        public void Draw(RenderWindow target)
        {
            target.Draw(sprite);
        }

        private void Reflect(Vector2f normal)
        {
            velocity -= normal * (2 * (
                velocity.X * normal.X +
                velocity.Y * normal.Y
            ));
        }

        private bool WallBounce()
        {
            bool didBounce = false;
            if (sprite.Position.X + radius > Program.ScreenW)
            {
                sprite.Position -= new Vector2f(sprite.Position.X + radius - Program.ScreenW, 0);
                velocity.X = -velocity.X;
                didBounce = true;
            }
            else if (sprite.Position.X - radius <= 0)
            {
                sprite.Position -= new Vector2f(sprite.Position.X - radius, 0);
                velocity.X = -velocity.X;
                didBounce = true;
            }
            
            if (sprite.Position.Y - radius <= 0)
            {
                sprite.Position -= new Vector2f(0, sprite.Position.Y - radius);
                velocity.Y = -velocity.Y;
                didBounce = true;
            }

            return didBounce;
        }
        
    }
}