/* Author: Bethany Weddle
 * CIS580 Project 5
 * Creating the balls to spawn
 */

using Microsoft.Xna.Framework.Graphics;

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
