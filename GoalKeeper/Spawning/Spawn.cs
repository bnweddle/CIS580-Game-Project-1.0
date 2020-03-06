using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
/* Author: Bethany Weddle
 * CIS580 Project 5
 * Creating the balls to spawn
 */
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public struct Spawn
    {
        public Ball Source;

        Texture2D texture => Source.texture;


        public Spawn(Ball ball)
        {
            Source = ball;
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            Source.Draw(SpriteBatch);
        }
    }
}
