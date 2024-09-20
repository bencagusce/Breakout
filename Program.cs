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
        public static int score = 0;
        public static int health = 3;
        public static bool gameOver = false;
        public static bool ridingPaddle = true;

        
        static void Main()
        {
            window.Closed += (o, e) => window.Close();
            Clock clock = new Clock();
            Ball ball = new Ball();
            Paddle paddle = new Paddle();
            Bricks bricks = new Bricks();

            Font font = new Font("./assets/future.ttf");
            
            Text textScore = new Text();
            Text textHealth = new Text();
            Text textRestart = new Text();
            Text textStart = new Text();
            textScore.Font = font;
            textHealth.Font = font;
            textRestart.Font = font;
            textStart.Font = font;
            textRestart.Position = new Vector2f(10, 550);
            textStart.Position = new Vector2f(10, 550);
            textScore.Position = new Vector2f(10, 0);
            textHealth.Position = new Vector2f(770, 0);
            
            
            //gameover screen
            Sprite sprite = new Sprite();
            sprite.Texture = new Texture("./assets/gameOver.png");
            
            //movePaddle
            bool moveRight = false;
            bool moveLeft = false;
            bool spacePressed = false;
            bool spaceBuffered = true;
            window.KeyPressed += (s, e) =>
            {
                if (e.Code == Keyboard.Key.Right) moveRight = true;
                else if (e.Code == Keyboard.Key.Left) moveLeft = true;
                else if (e.Code == Keyboard.Key.Space) spacePressed = true;
            };
            window.KeyReleased += (s, e) =>
            {
                if (e.Code == Keyboard.Key.Right) moveRight = false;
                else if (e.Code == Keyboard.Key.Left) moveLeft = false;
                else if (e.Code == Keyboard.Key.Space)
                {
                    spacePressed = false;
                    spaceBuffered = true;
                }
            };
            
            
            while (window.IsOpen) 
            {
                if(gameOver)
                {
                    window.DispatchEvents();
                    
                    // Update
                    if (spacePressed)
                    {
                        gameOver = false;
                        spaceBuffered = false;
                    }
                    textRestart.DisplayedString = ("Restart by pressing Space");
                    // Draw
                    window.Clear(new Color(Color.Blue));
                    window.Draw(sprite);
                    window.Draw(textRestart);
                    window.Display();

                }
                else
                {
                    float deltaTime = clock.Restart().AsSeconds();
                    window.DispatchEvents();
    
                    // Update
                    paddle.Update(deltaTime, moveRight, moveLeft);
                    gameOver = ball.Update(deltaTime, paddle, bricks);
                    textScore.DisplayedString = $"{Program.score}";
                    textHealth.DisplayedString = $"{Program.health}";
                    textStart.DisplayedString = "Press Space";
                    if (ridingPaddle)
                    {
                        if (spacePressed && spaceBuffered)
                        {
                            ridingPaddle = false;
                        }
                    }
    
                    // Draw
                    window.Clear(new Color(Color.Blue));
                    ball.Draw(window);
                    paddle.Draw(window);
                    bricks.Draw(window);
                    if (ridingPaddle) window.Draw(textStart);
                    window.Draw(textScore);
                    window.Draw(textHealth);
                    window.Display();
                }
            }
        }
    }
}
