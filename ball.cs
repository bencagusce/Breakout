using System.Numerics;
using SFML.Graphics;
using SFML.System;

namespace Breakout
{
    public class Ball
    {
        public Sprite sprite { private set; get; } 
        private const float diameter = 20.0f;
        private const float radius = diameter * 0.5f;
        private float speed = 200.0f;
        private Vector2f velocity = new Vector2f(1, 1) / MathF.Sqrt(2.0f); //direction. Uhm no :P
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
        public bool Update(float deltaTime, Paddle paddle, Bricks bricks)
        {
            sprite.Position += velocity * deltaTime;
            WallBounce();
            PaddleBounce(paddle, deltaTime);
            // BrickBounce(brick);
            return LoosingHealth();
        }

        public void Draw(RenderWindow target)
        {
            target.Draw(sprite);
        }

        
        private bool LoosingHealth()
        {
            if (sprite.Position.Y > 600)
            {
                Program.health --;
                if (Program.health == 0)
                {
                    
                    // Program.gameOver = true;
                    return true;
                }
            }

            return false;
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
                // sprite.Position -= new Vector2f(sprite.Position.X - radius, 0);
                sprite.Position = new Vector2f(radius, sprite.Position.Y);
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
        private bool PaddleBounce(Paddle paddle, float deltaTime)
        {
            // If the ball is way higher than the paddle return false
            if (paddle.sprite.Position.Y - sprite.Position.Y > radius + (paddle.height * 0.5) + (speed * deltaTime)) return false;

            // Paddle is divided into 1 rectangle and 2 semicircles
            float rectangleWidth = paddle.width - paddle.height;
            Vector2f leftSemicircleCenter = paddle.sprite.Position - new Vector2f(rectangleWidth * 0.5f, 0);
            Vector2f rightSemicircleCenter = paddle.sprite.Position + new Vector2f(rectangleWidth * 0.5f, 0);
            
            // Rectangle collision
            if (
                MathF.Abs(paddle.sprite.Position.X - sprite.Position.X) <= rectangleWidth * 0.5f &&
                MathF.Abs(paddle.sprite.Position.Y - sprite.Position.Y) <= paddle.height * 0.5f + radius
            )
            {
                sprite.Position = new Vector2f(sprite.Position.X, paddle.sprite.Position.Y - (radius + paddle.height * 0.5f));
                velocity.Y = -velocity.Y;
            }
            // left semicircle collision
            else if ((sprite.Position - leftSemicircleCenter).Length() <= radius + (paddle.height * 0.5f))
            {
                // Undo last movement
                sprite.Position -= velocity * deltaTime;
                
                // Ray marching to find the most physically correct collision point
                sprite.Position = Helpers.RayMarchFindCollisionPoint(
                    sprite.Position, radius,
                    leftSemicircleCenter, paddle.height * 0.5f,
                    velocity.Normalized());
                
                // Reflect ball
                velocity = Helpers.Reflect(velocity, (sprite.Position - leftSemicircleCenter).Normalized());
            }
            // right semicircle collision
            else if ((sprite.Position - rightSemicircleCenter).Length() <= radius + (paddle.height * 0.5f))
            {
                // Undo last movement
                sprite.Position -= velocity * deltaTime;
                
                // Ray marching to find the most physically correct collision point
                sprite.Position = Helpers.RayMarchFindCollisionPoint(
                    sprite.Position, radius,
                    rightSemicircleCenter, paddle.height * 0.5f,
                    velocity.Normalized());
                
                // Reflect ball
                velocity = Helpers.Reflect(velocity, (sprite.Position - rightSemicircleCenter).Normalized());
            }
            return false;
        }
        // private bool BrickBounce(Brick brick)
        // {
        //     bool brickHit = false;
        //     for (int i = 0; i < brick.positions.Count; i++) 
        //     {
        //         var pos = brick.positions[i];
        //         if (Helpers.CircleRectangleCollision(
        //             sprite, brick
        //             pos, brick.size, out Vector2f hit))
        //         {
        //             sprite.Position += hit;
        //             brick.positions.RemoveAt(i);
        //             i = 0; // Check all again since ball was moved
        //             brickHit = true;
        //             if (brickHit == true)
        //             {
        //                 Program.score += 100;
        //             }
        //         }
        //     }
        //     return brickHit;
        // }
    }
}