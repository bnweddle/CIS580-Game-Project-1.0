/* Author: Bethany Weddle
 * CIS580 Project 5
 * Read in the balls created from the file
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using TRead = GoalKeeper.Spawnset;

namespace GoalKeeper
{
    public class SpawnReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            int numToSpawn = input.ReadInt32();

            var balls = new Spawn[numToSpawn];

            for (int i = 0; i < numToSpawn; i++)
            {
                var source = new Ball(new Vector2(
                    input.ReadInt32(),
                    input.ReadInt32()));

                balls[i] = new Spawn(source);
            }    
            // Construct and return the Spawnset
            return new Spawnset(balls);
        }
    }
}
