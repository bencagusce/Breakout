using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Breakout
{
    public class Brick
    {
        private Sprite sprite;
        public List<Vector2f> positions;
        public Vector2f size = new Vector2f(50.0f, 10.0f);
        public Brick()
        {
            sprite = new Sprite();
            sprite.Texture = new Texture("./assets/tileBlue.png");
            size.Y = size.X * ((float)sprite.Texture.Size.Y / (float)sprite.Texture.Size.X);
            positions = new List<Vector2f>();
            for (int i = -7; i <= 7; i++)
            {
                for (int j = -6; j <= 3; j++)
                {
                    var pos = new Vector2f(
                        Program.ScreenW * 0.5f + i * 53.1f,
                        Program.ScreenH * 0.29f + j * 26.5f);
                    positions.Add(pos);
                }
            }
            sprite.Origin = 0.5f * (Vector2f)sprite.Texture.Size;
            sprite.Scale = new Vector2f(
                size.X / sprite.Texture.Size.X,
                size.Y / sprite.Texture.Size.Y
            );
        }
        public void Draw(RenderTarget target)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                sprite.Position = positions[i];
                target.Draw(sprite);
            }
        }
    }
}

