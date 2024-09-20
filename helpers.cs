using SFML.Graphics;
using SFML.System;

namespace Breakout
{
    public class Helpers
    {
        /// <summary>
        /// Uses ray marching to find the closest point to the physically correct collision point
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="radius1"></param>
        /// <param name="position2"></param>
        /// <param name="radius2"></param>
        /// <param name="direction1"></param>
        /// <param name="maxSteps"></param>
        /// <returns></returns>
        public static Vector2f RayMarchFindCollisionPoint(
            Vector2f position1, float radius1,
            Vector2f position2, float radius2,
            Vector2f direction1, int maxSteps = 10, float approximate0 = 0.01f
        )
        {
            float radiuses = radius1 + radius2;
            int steps = 0;
            float ray = (position1 - position2).Length() - radiuses;
            while (ray > approximate0 && steps < maxSteps)
            {
                position1 += direction1 * ray;
                ray = (position1 - position2).Length() - radiuses;
                steps++;
            }
            position1 = position2 + ((position1 - position2).Normalized() * radiuses);
            return position1;
        }
        
        // Taken from tutorial
        public static Vector2f Reflect(Vector2f velocity, Vector2f normal)
        {
            return velocity -= normal * (2 * (
                velocity.X * normal.X +
                velocity.Y * normal.Y
            ));
        }
    }
}