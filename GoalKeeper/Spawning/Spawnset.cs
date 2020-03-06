/* Author: Nathan Bean
 * Modified by: Bethany Weddle
 * CIS580 Project 5
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Spawnset
    {
        //array of spawns
        Spawn[] spawns;

        /// <summary>
        /// Constructs a new instance of Spanset
        /// </summary>
        /// <param name="tiles">The Spawns in this set</param>
        public Spawnset(Spawn[] spawnset)
        {
            this.spawns = spawnset;
        }

        /// <summary>
        /// An indexer for accessing individual spawns in the set
        /// </summary>
        /// <param name="index">The index of the spawn</param>
        /// <returns>The sprite</returns>
        public Spawn this[int index]
        {
            get => spawns[index];
        }

        /// <summary>
        /// The number of Spawns in the set
        /// </summary>
        public int Count => spawns.Length;

    }
}
