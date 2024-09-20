using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Breakout
{
    public class Bricks
    {
        public List<Sprite> sprites;
        public Vector2f size = new Vector2f(50.0f, 10.0f);
        public Bricks()
        {
            Texture[] textures =
            {
                new Texture("./assets/tileBlue.png"),
                new Texture("./assets/tileGreen.png"),
                new Texture("./assets/tilePink.png")
            };
            
            size.Y = size.X * ((float)textures[0].Size.Y / (float)textures[0].Size.X);
            sprites = new List<Sprite>();
            for (int i = -6; i <= 6; i++)
            {
                for (int j = -6; j <= 5; j++)
                {
                    Sprite sprite = new Sprite();
                    sprite.Texture = textures[(j + 7) % 3];
                    sprite.Position = new Vector2f(
                        Program.ScreenW * 0.5f + i * 53.1f,
                        Program.ScreenH * 0.3f + j * 24.0f);
                    sprite.Origin = 0.5f * (Vector2f)sprite.Texture.Size;
                    sprite.Scale = new Vector2f(
                        size.X / sprite.Texture.Size.X,
                        size.Y / sprite.Texture.Size.Y
                    );
                    
                    sprites.Add(sprite);
                }
            }
        }
        public void Draw(RenderTarget target)
        {
            foreach (Sprite sprite in sprites)
            {
                target.Draw(sprite);
            }
        }
    }
}

