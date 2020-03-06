/* Author: Bethany Weddle
 * CIS580 Project 5
 * Read in the balls created from the file
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


using TRead = System.Collections.Generic.List<GoalKeeper.Ball>;

namespace GoalKeeper
{
    public class SpawnReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            int numToSpawn = input.ReadInt32();

            var list = new List<Ball>();

            for (int i = 0; i < numToSpawn; i++)
            {
                var vector = new Vector2(
                    input.ReadInt32(),
                    input.ReadInt32());

                var texture = input.ReadExternalReference<Texture2D>();

                var ball = new Ball(vector, texture);

                list.Add(ball);
            }

            return list;
        }
    }
}
