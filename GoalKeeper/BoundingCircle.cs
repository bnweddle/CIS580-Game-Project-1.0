/* Bethany Weddle
 * BoundingCircle struct
 * Used to see if Circles collide
 * */
using System;
using Microsoft.Xna.Framework;

namespace GoalKeeper
{
    public struct BoundingCircle
    {
        public float X;

        public float Y;

        public float Radius => 25;

        public Vector2 Center
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        //Constructor
        public BoundingCircle(float x, float y)
        {
            this.X = x;
            this.Y = y;
            //this.Radius = radius;
        }

        public bool CollidesWith(BoundingCircle other)
        {
            // (A.Radius)^2 + (B.Radius)^2 <= (A.Center.X - B.Center.X)^2 + (A.Center.Y - B.Center.Y)^2
            return !(Math.Sqrt((double)this.Radius) + Math.Sqrt((double)other.Radius) <=
                Math.Sqrt((double)this.Center.X - (double)other.Center.X) + Math.Sqrt((double)this.Center.Y - (double)other.Center.Y));
        }

        public static implicit operator Rectangle(BoundingCircle c)
        {
            return new Rectangle(
                (int)(c.X - c.Radius),
                (int)(c.Y - c.Radius),
                (int)(2 * c.Radius),
                (int)(2 * c.Radius));
        }
    }
}
