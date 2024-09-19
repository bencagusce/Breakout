using SFML.Graphics;
using SFML.System;
using SFML.Window;
namespace Breakout
{
    public class Paddle
    {
        public Sprite sprite;
        private float speed = 300.0f;
        public float width = 80.0f;
        public float height = 10.0f;
        public Paddle()
        {
            sprite = new Sprite();
            sprite.Texture = new Texture("./assets/paddle.png");
            height = width * ((float)sprite.Texture.Size.Y / (float)sprite.Texture.Size.X);
            sprite.Position = new Vector2f(Program.ScreenW * 0.5f, Program.ScreenH - 2.0f * height);
            sprite.Origin = 0.5f * (Vector2f)sprite.Texture.Size;
            sprite.Scale = new Vector2f(
                width / sprite.Texture.Size.X,
                height / sprite.Texture.Size.Y
            );
            
            
        }
        public void Update(float deltaTime, bool moveRight, bool moveLeft)
        {
            if (moveRight) sprite.Position += new Vector2f(deltaTime * speed, 0);
            if (moveLeft) sprite.Position -= new Vector2f(deltaTime * speed, 0);

            if (sprite.Position.X - width * 0.5f <= 0)
            {
                sprite.Position = new Vector2f(width * 0.5f, sprite.Position.Y);
            }
            else if (sprite.Position.X + width * 0.5f >= Program.ScreenW)
            {
                sprite.Position = new Vector2f(Program.ScreenW - width * 0.5f, sprite.Position.Y);
            }
        }

        public void Draw(RenderWindow target)
        {
            // Font font = new Font("./assets/Monocraft.ttf"); // Ensure you have a font file at this path
            // Text text = new Text($"{height}", font);
            // target.Draw(text);
            target.Draw(sprite);
        }
    }
}       