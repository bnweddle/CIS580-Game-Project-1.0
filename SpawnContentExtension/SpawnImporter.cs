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
using System;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace SpawnContentExtension
{
    [ContentImporter(".spawn", DisplayName = "Spawn Importer", DefaultProcessor = "SpawnProcessor")]

    public class SpawnImporter : ContentImporter<TInput>
    {

        public override TInput Import(string filename, ContentImporterContext context)
        {

             List<Spawn> spawned = new List<Spawn>();
            string path = "C:\\Users\\betha\\source\\repos\\GoalKeeper\\GoalKeeper\\Content\\ball.png";

            //https://stackoverflow.com/questions/18886945/reading-a-text-file-and-inserting-information-into-a-new-object
            //Used this format to actually get it working
            string[] allLines = File.ReadAllLines(filename);

            foreach (var line in allLines)
            {
                var splittedLines = line.Split(new char[] { ' ' });
                if (splittedLines != null)
                {
                    spawned.Add(new Spawn
                    {
                        Position = new Vector2((float)Convert.ToDouble(splittedLines[0]),
                                               (float)Convert.ToDouble(splittedLines[1])),
                        TextureFileName = splittedLines[2],
                        Texture = new ExternalReference<TextureContent>(path), //How to make the image show???
                    });
                }

            }

            return spawned;
        }
    }
}
