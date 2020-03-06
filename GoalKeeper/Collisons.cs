/* Author: Nathan Bean
 * Modified By: Bethany Weddle
 * Date: 2-7-20
 * Done in Class mostly
 * */
using Microsoft.Xna.Framework;
using System;

namespace GoalKeeper
{
    public static class Collisons
    {
        public static bool CollidesWith(this BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.X > b.X + b.Width
                || a.X + a.Width < b.X
                || a.Y > b.Y + b.Height
                || a.Y + a.Height < b.Y);
        }
        public static bool CollidesWith(this BoundingRectangle r, BoundingCircle c)
        {
            Vector2 collisionPoint;
            return CollidesWith(r, c, out collisionPoint);

        }
        public static bool CollidesWith(this BoundingRectangle r, BoundingCircle c, out Vector2 collisionPoint)
        {
            var closestX = Math.Max(r.X, Math.Min(c.X, r.X + r.Width));
            var closestY = Math.Max(r.Y, Math.Min(c.Y, r.Y + r.Height));
            collisionPoint.X = closestX;
            collisionPoint.Y = closestY;
            return (Math.Pow(c.Radius, 2) >= Math.Pow(closestX - c.X, 2) + Math.Pow(closestY - c.Y, 2));
        }

        public static bool CollidesWith(this BoundingCircle c, BoundingRectangle r, out Vector2 cp)
        {
            return r.CollidesWith(c, out cp);
        }

        public static bool CollidesWith(this BoundingCircle c, BoundingRectangle r)
        {
            return r.CollidesWith(c);
        }

        public static bool CollidesWith(this Vector2 v, Vector2 other)
        {
            //point colliding with another point
            return v == other;
        }

        public static bool CollidesWith(this Vector2 v, BoundingRectangle r)
        {
            return (r.X <= v.X && v.X <= r.X + r.Width)
                && (r.Y <= v.Y && v.Y <= r.Y + r.Height);
        }

        public static bool CollidesWith(this BoundingRectangle r, Vector2 v)
        {
            return v.CollidesWith(r);
        }

        public static bool CollidesWith(this Vector2 v, BoundingCircle c)
        {
            return Math.Pow(c.Radius, 2) >= Math.Pow(v.X - c.X, 2) + Math.Pow(v.Y - c.Y, 2);
        }

        public static bool CollidesWith(this BoundingCircle c, Vector2 v)
        {
            return v.CollidesWith(c);
        }
    }
}
