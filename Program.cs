using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Breakout
{
    class Program
    {
        public static Sprite sprite { private set; get; }
        public const int ScreenW = 800;
        public const int ScreenH = 600;
        public static RenderWindow window = new RenderWindow(new VideoMode(ScreenW, ScreenH), "Breakout");
        public static int score = 0;
        public static int health = 3;
        public static bool gameOver = false;

        
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
            textScore.Font = font;
            textHealth.Font = font;
            textRestart.Font = font;
            textRestart.Position = new Vector2f(10, 550);
            textScore.Position = new Vector2f(10, 0);
            textHealth.Position = new Vector2f(770, 0);
            
            
            //gameover screen
            sprite = new Sprite();
            sprite.Texture = new Texture("./assets/gameOver.png");
            
            //Vector2f restartTextureSize = textRestart.Position;
            //textRestart.Origin = 0.5f * restartTextureSize;
            
            
            //movePaddle
            bool moveRight = false;
            bool moveLeft = false;
            bool spacePressed = false;
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
                else if (e.Code == Keyboard.Key.Space) spacePressed = false;
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
    
                    // Draw
                    window.Clear(new Color(Color.Blue));
                    ball.Draw(window);
                    paddle.Draw(window);
                    bricks.Draw(window);
                    window.Draw(textScore);
                    window.Draw(textHealth);
                    window.Display();
                }
            }
        }
    }
}
