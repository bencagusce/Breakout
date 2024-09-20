using SFML.System;

namespace Breakout
{
    public class Helpers
    {
        // public static bool CircleRectangleCollision(
        //     Vector2f circlePos, float circleR,
        //     Vector2f rectPos, Vector2f rectDim,
        //     out Vector2f newCirclePos
        // )
        // {
        //
        //     
        //     
        //     // Rect to rect collision
        //     
        //     newCirclePos = new Vector2f(0,0);
        //     return false;
        // }

        // public static bool RaymarchCollisionCircleCircle(
        //     Vector2f position1, float radius1,
        //     Vector2f position2, float radius2,
        //     Vector2f direction1, float maxDistance,
        //     out Vector2f newPosition1
        // )
        // {
        //     float radiuses = radius1 + radius2;
        //     float currentDistance = 0f;
        //     float ray1 = (position1 - position2).Length() - radiuses;
        //     float ray2;
        //     float approximate0 = 0.01f;
        //     while (ray1 > approximate0)
        //     {
        //         position1 += direction1 * ray1;
        //         ray2 = (position1 - position2).Length() - radiuses;
        //         if (ray2 > ray1) return false;
        //         ray1 = ray2;
        //     }
        // }
        
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