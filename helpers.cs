using SFML.System;

namespace Breakout
{
    public class Helpers
    {
        public static bool CircleRectangleCollision(
            Vector2f circlePos, float circleR,
            Vector2f rectPos, Vector2f rectDim,
            out Vector2f newCirclePos
        )
        {

            
            
            // Rect to rect collision
            
            newCirclePos = new Vector2f(0,0);
            return false;
        }
    }
}