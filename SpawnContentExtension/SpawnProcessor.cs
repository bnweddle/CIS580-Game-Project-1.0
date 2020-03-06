/* Author: Bethany Weddle
 * CIS580 Project 5
 * Processes the file and converts the string lines into integers 
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.Collections.Generic.List<SpawnContentExtension.Spawn>; //List<Vector>
using TOutput = System.Collections.Generic.List<SpawnContentExtension.Spawn>; //List<Vector2>
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace SpawnContentExtension
{
    [ContentProcessor(DisplayName = "Spawn Processor")]

    public class SpawnProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            for(int i = 0; i < input.Count; i++)
            {
                Spawn s = input[i];
                ExternalReference<TextureContent> tc = new ExternalReference<TextureContent>(s.TextureFileName);
                s.Texture = context.BuildAsset<TextureContent, TextureContent>(tc, "TextureProcessor");
            }

            // The Spawnset has been processed
            return input;
        }
    }
}
