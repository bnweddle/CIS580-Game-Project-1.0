﻿/* Bethany Weddle
 * BoundingRectangle struct
 * Used to see if Rectangles collide
 * */
using Microsoft.Xna.Framework;

namespace GoalKeeper
{
    public struct BoundingRectangle
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        //Constructor
        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return (this.X > other.X + other.Width
                || this.X + this.Width < other.X
                || this.Y > other.Y + other.Height
                || this.Y + this.Height < other.Y);

        }

        public static implicit operator Rectangle(BoundingRectangle BR)
        {
            return new Rectangle((int)BR.X, (int)BR.Y, (int)BR.Width, (int)BR.Height);
        }

    }
}
