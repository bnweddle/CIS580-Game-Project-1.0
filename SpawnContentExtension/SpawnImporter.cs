/* Author: Nathan Bean
 * Modified by: Bethany Weddle
 * CIS580 Project 5
 * Imports the file by the content and converts the values to Ints
 */
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;

using TInput = System.Collections.Generic.List<SpawnContentExtension.Spawn>;
using System.Collections.Generic;

namespace SpawnContentExtension
{
    [ContentImporter(".spawn", DisplayName = "Spawn Importer", DefaultProcessor = "SpawnProcessor")]

    public class SpawnImporter : ContentImporter<TInput>
    {
        //NOT SURE ABOUT THIS
        public override TInput Import(string filename, ContentImporterContext context)
        {
            List<Spawn> spawned = new List<Spawn>();

            // Read in the texture path, set for all spawn
            
            // Open and read the file, create a spwn for each line and add to list

            return spawned;
        }
    }
}
