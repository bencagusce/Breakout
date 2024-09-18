using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Breakout
{
    class Program
    {
        public const int ScreenW = 800;
        public const int ScreenH = 600;
        
        static void Main()
        {
            using (var window = new RenderWindow(new VideoMode(ScreenW, ScreenH), "Breakout"))
            {
                window.Closed += (o, e) => window.Close();
                Clock clock = new Clock();
                Ball ball = new Ball();
                
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
}
