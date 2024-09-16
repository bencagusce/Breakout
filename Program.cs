using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Breakout
{
    class Program
    {
        public const int WIDTH = 800;
        public const int HEIGHT = 600;
        
        static void Main()
        {
            Clock clock = new Clock();
            Ball ball = new Ball();
            
            RenderWindow window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Breakout");

            while (window.IsOpen)
            {
                float deltaTime = clock.Restart().AsSeconds();
                window.DispatchEvents();
                ball.Update(deltaTime);
                window.Clear(new Color(Color.Blue));
                ball.Draw(window);
                window.Display();
            }
        }
    }
}
