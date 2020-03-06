/* Author: Nathan Bean
 * Modified by: Bethany Weddle
 * CIS580 Project 5
 * Writes the output of the rows and columns by getting their values
 */
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = System.Collections.Generic.List<SpawnContentExtension.Spawn>;

namespace SpawnContentExtension {

    [ContentTypeWriter]
    public class SpawnWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            // Write the number of spawns
            output.Write(value.Count);

            // Write the individual spawn data
            for (int i = 0; i < value.Count; i++)
            {
                var spawn = value[i];
                output.Write(spawn.Position.X);
                output.Write(spawn.Position.Y);
                output.WriteExternalReference(spawn.Texture);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "GoalKeeper.SpawnReader, GoalKeeper";
        }
    }
}
