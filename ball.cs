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
        private Vector2f velocity = new Vector2f(1, 1) / MathF.Sqrt(2.0f);
        
        public Ball()
        {            

            sprite = new Sprite();
            sprite.Texture = new Texture("./assets/ball.png");
            sprite.Position = new Vector2f(250, 400);
            velocity *= speed;
            Vector2f ballTextureSize = (Vector2f)sprite.Texture.Size;
            sprite.Origin = 0.5f * ballTextureSize;
            sprite.Scale = new Vector2f(
                diameter / ballTextureSize.X,
                diameter / ballTextureSize.Y);
        }
        public bool Update(float deltaTime, Paddle paddle, Bricks bricks, out bool resetBricks)
        {
            resetBricks = false;
            if (Program.ridingPaddle)
            {
                RidingPaddle(paddle);
                return false;
            }
            
            sprite.Position += velocity * deltaTime;
            WallBounce();
            resetBricks = PaddleBounce(paddle, bricks, deltaTime);
            BrickBounce(bricks, deltaTime);
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
                Program.health--;
                Program.ridingPaddle = true;
                if (Program.health == 0)
                {
                    Program.score = 0;
                    Program.health = 3;
                    return true;
                }
            }

            return false;
        }

        private void RidingPaddle(Paddle paddle)
        {
            sprite.Position = new Vector2f(paddle.sprite.Position.X, (Program.ScreenH - 2.0f * paddle.height) + -20.0f);
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
        private bool PaddleBounce(Paddle paddle, Bricks bricks, float deltaTime)
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
                
                // If we're out of bricks spawn new
                if (bricks.sprites.Count() == 0) return true;
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
                
                // If we're out of bricks spawn new
                if (bricks.sprites.Count() == 0) return true;
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
                
                // If we're out of bricks spawn new
                if (bricks.sprites.Count() == 0) return true;
            }
            return false;
        }
        private void BrickBounce(Bricks bricks, float deltaTime)
        {
            int colidingBrick = -1;
            
            for (int i = 0; i < bricks.sprites.Count(); i++)
            {
                // Collision by Emil Forslund
                if (Collision.CircleRectangle(
                        sprite.Position, radius,
                        bricks.sprites[i].Position, bricks.size, out Vector2f v))
                {
                    // Hit top or bottom
                    if (MathF.Abs(sprite.Position.X - bricks.sprites[i].Position.X) < bricks.size.X * 0.5f)
                    {
                        velocity.Y = -velocity.Y;
                        bool down = sprite.Position.Y - bricks.sprites[i].Position.Y > 0;
                        sprite.Position = new Vector2f(sprite.Position.X,bricks.sprites[i].Position.Y + (radius + bricks.size.Y * 0.5f) * (down ? 1f : -1f));
                        Program.score += 100;
                        bricks.sprites.RemoveAt(i);
                        return;
                    }
                    // Hit from left or right
                    if (MathF.Abs(sprite.Position.Y - bricks.sprites[i].Position.Y) < bricks.size.Y * 0.5f)
                    {
                        velocity.X = -velocity.X;
                        bool right = sprite.Position.X - bricks.sprites[i].Position.X > 0;
                        sprite.Position = new Vector2f(bricks.sprites[i].Position.X + (radius + bricks.size.X * 0.5f) * (right ? 1f : -1f), sprite.Position.Y);
                        Program.score += 100;
                        bricks.sprites.RemoveAt(i);
                        return;
                    }
                    // Hit a corner
                    // Lower priority than flat sides so done after the loop
                    colidingBrick = i;
                }
            }

            // Hit a corner
            if (colidingBrick != -1)
            {
                // Undo last movement
                sprite.Position -= velocity * deltaTime;

                // Find closest corner
                Vector2f corner = bricks.sprites[colidingBrick].Position +
                                  new Vector2f(bricks.size.X * 0.5f, bricks.size.Y * 0.5f);
                // cornerCoefficients
                Vector2f[] cC =
                    { new Vector2f(-0.5f, 0.5f), new Vector2f(0.5f, -0.5f), new Vector2f(-0.5f, -0.5f) };
                for (int j = 0; j < 3; j++)
                {
                    Vector2f newCorner = bricks.sprites[colidingBrick].Position +
                                         new Vector2f(bricks.size.X * cC[j].X, bricks.size.Y * cC[j].Y);
                    if ((sprite.Position - newCorner).Length() < (sprite.Position - corner).Length())
                        corner = newCorner;
                }

                // Ray marching to find the most physically correct collision point
                sprite.Position = Helpers.RayMarchFindCollisionPoint(
                    sprite.Position, radius,
                    corner, 0f,
                    velocity.Normalized());

                // Reflect ball
                velocity = Helpers.Reflect(velocity, (sprite.Position - corner).Normalized());
                
                Program.score += 100;
                bricks.sprites.RemoveAt(colidingBrick);
            }
        }
    }
}