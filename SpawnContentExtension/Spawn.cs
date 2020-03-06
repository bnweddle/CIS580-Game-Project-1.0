/* Author: Bethany Weddle
 * CIS580 Project 5
 * Creating the balls to spawn
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace SpawnContentExtension
{
    public struct Spawn
    {
        public Vector2 Position { get; set; }

        public ExternalReference<TextureContent> Texture { get; set; }

        public string TextureFileName { get; set; } 
    }
}
