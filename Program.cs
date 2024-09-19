using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Breakout
{
    class Program
    {
        public const int ScreenW = 800;
        public const int ScreenH = 600;
        public static RenderWindow window = new RenderWindow(new VideoMode(ScreenW, ScreenH), "Breakout");
        public static bool gameOver = false;
        public static int score = 0;
        public static int health = 3;
        static void Main()
        {
            window.Closed += (o, e) => window.Close();
            Clock clock = new Clock();
            Ball ball = new Ball();
            Paddle paddle = new Paddle();
            Brick brick = new Brick();

            void lost()
            {
                if (gameOver == true)
                {
                    
                }
            }
            //movePaddle
            bool moveRight = false;
            bool moveLeft = false;
            window.KeyPressed += (s, e) =>
            {
                if (e.Code == Keyboard.Key.Right) moveRight = true;
                else if (e.Code == Keyboard.Key.Left) moveLeft = true;
            };
            window.KeyReleased += (s, e) =>
            {
                if (e.Code == Keyboard.Key.Right) moveRight = false;
                else if (e.Code == Keyboard.Key.Left) moveLeft = false;
            };
            
            while (window.IsOpen) 
            {
                float deltaTime = clock.Restart().AsSeconds();
                window.DispatchEvents();
                paddle.Update(deltaTime, moveRight, moveLeft); 
                ball.Update(deltaTime, paddle, brick);
                window.Clear(new Color(Color.Blue));
                ball.Draw(window);
                paddle.Draw(window);
                window.Display();
            }
        }
    }
}
