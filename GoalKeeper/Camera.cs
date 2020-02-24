/* Camera.cs
 * Author: Bethany Weddle
 * 2-24-20
 * */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
            //Zooms to the position of the mouse
            Position.X = mouse.Position.X;
            Position.Y = mouse.Position.Y;

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;

            //Fix the mouse so it can't go out of bounds so the screeen doesn't move
            if (mouse.Position.X > view.Width)
                Mouse.SetPosition(view.Width, mouse.Position.Y);
            if(mouse.Position.X < view.X)
                Mouse.SetPosition(view.X, mouse.Position.Y);
            if (mouse.Position.Y > view.Height)
                Mouse.SetPosition(mouse.Position.X, view.Height);

               
            //So the user can't zoom out past the screen dimensions
            if(scale < 1.0)
            {
                scale = 1.0f;
            }


            if (mouse.LeftButton == ButtonState.Pressed)
                scale += 0.01f;
            else if(mouse.RightButton == ButtonState.Pressed)
                scale -= 0.01f;

            //Used for debugging
            //Debug.WriteLine($"Scale : {scale}");
            matrix = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(new Vector3(Position, 0));
        }

    }
}
