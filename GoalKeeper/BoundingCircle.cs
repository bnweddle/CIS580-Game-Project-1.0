/* Bethany Weddle
 * BoundingCircle struct
 * Used to see if Circles collide
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GoalKeeper
{
    public struct BoundingCircle
    {
        public float X;

        public float Y;

        public float Radius;

        //This is a getter
        public Vector2 Center => new Vector2(X, Y);

        //Constructor
        public BoundingCircle(float x, float y, float radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
        }

        public bool CollidesWith(BoundingCircle other)
        {
            // (A.Radius)^2 + (B.Radius)^2 <= (A.Center.X - B.Center.X)^2 + (A.Center.Y - B.Center.Y)^2
            return !(Math.Sqrt((double)this.Radius) + Math.Sqrt((double)other.Radius) <=
                Math.Sqrt((double)this.Center.X - (double)other.Center.X) + Math.Sqrt((double)this.Center.Y - (double)other.Center.Y));
        }
    }
}
