using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Grid
    {
        
        const int NUM_CELLS = 10;
        const int CELL_SIZE = 20;
        //private Unit cells[NUM_CELLS][NUM_CELLS];

        public Grid()
        {
            // Clear the grid.
            for (int x = 0; x < NUM_CELLS; x++)
            {
                for (int y = 0; y < NUM_CELLS; y++)
                {
                  //  cells[x][y] = null;
                }
            }
        }    
    }
}
