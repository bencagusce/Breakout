using SFML.Graphics;
using SFML.System;

namespace Breakout
{
    public class Ball
    {
        public Sprite sprite;

        public Ball()
        {
            sprite = new Sprite();
            sprite.Texture = new Texture("./assets/ball.png");
            sprite.Position = new Vector2f(250, 300);
        }
        public void Update(float deltaTime)
        {
            
        }

        public void Draw(RenderWindow target)
        {
            target.Draw(sprite);
        }
        
    }
}