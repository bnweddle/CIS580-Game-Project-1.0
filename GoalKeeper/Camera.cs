using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    /// <summary>
    /// Using the camera for creating Zoom capability to satisfy the 
    /// Sprite.Begin and Matrices requirement
    /// </summary>
    public class Camera
    {
        // I followed this tutorial to implement Zooming
        // https://www.youtube.com/watch?v=lS2Vu8SD2uY

        Vector2 Position;
        Matrix matrix;
        float scale = 1.0f;

        public Matrix Matrix
        {
            get { return matrix; }
        }

        public void Update(MouseState mouse, Viewport view)
        {
            Position.X = ((mouse.Position.X + 16) - (view.Width / 2) / scale);
            Position.Y = ((mouse.Position.Y + 24) - (view.Height / 2) / scale);

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;


            if (mouse.LeftButton == ButtonState.Pressed)
                scale += 0.01f;
            else if (mouse.RightButton == ButtonState.Pressed)
                scale -= 0.01f;

            matrix = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                Matrix.CreateScale(scale);
        }

    }
}
