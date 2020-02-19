using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Grid
    {
        //create 4 quadrants
        const int NUM_CELLS = 10;
        const int CELL_SIZE = 150;
        Unit[,] Cells = new Unit[NUM_CELLS, NUM_CELLS];
        
        public Grid()
        {
            // Clear the grid.
            for (int x = 0; x < NUM_CELLS; x++)
            {
                for (int y = 0; y < NUM_CELLS; y++)
                {
                    Cells[x,y] = null;
                }
            }
        }

        public void Add(Unit unit)
        {
            int cellX = (int)(unit.X / Grid.CELL_SIZE);
            int cellY = (int)(unit.Y / Grid.CELL_SIZE);

            unit.Prev = null;

            Cells[cellX, cellY] = unit.Next;
            Cells[cellX, cellY] = unit;

            if (unit.Next != null)
            {
                unit.Next.Prev = unit;
            }

        }

        public void Update(Unit old, float x, float y)
        {
            // See which cell it was in.
            int oldCellX = (int)(old.X / Grid.CELL_SIZE);
            int oldCellY = (int)(old.Y / Grid.CELL_SIZE);

            // See which cell it's moving to.
            int cellX = (int)(x / Grid.CELL_SIZE);
            int cellY = (int)(y / Grid.CELL_SIZE);

            old.X = x;
            old.Y = y;

            // Unlink it from the list of its old cell.
            if (old.Prev != null)
            {
                old.Prev.Next = old.Next;
            }

            if (old.Next != null)
            {
                old.Next.Prev = old.Prev;
            }

            // If it's the head of a list, remove it.
            if (Cells[oldCellX, oldCellY] == old)
            {
                Cells[oldCellX, oldCellY] = old.Next;
            }

            // Add it back to the grid at its new cell.
            Add(old);

        }

        public void CheckCollisions()
        {
            for (int x = 0; x < NUM_CELLS; x++)
            {
                for (int y = 0; y < NUM_CELLS; y++)
                {
                    handleCell(Cells[x,y]);
                }
            }
        }

        public void handleCell(Unit unit)
        {
            while (unit != null)
            {
                Unit other = unit.Next;
                while (other != null)
                {
                    if (unit.X == other.X &&
                        unit.Y == other.Y)
                    {
                        unit.X -= 1;
                        unit.Y -= 1;
                    }
                    other = other.Next;
                }

                unit = unit.Next;
            }
        }

    }
}
