using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Grid
    {
        public const int NUM_CELLS = 10;
        public const int CELL_SIZE = 20;
        private Unit[,] cells_ = new Unit[NUM_CELLS, NUM_CELLS];

        public Grid()
        {
            // Clear the grid.
            for (int x = 0; x < NUM_CELLS; x++)
            {
                for (int y = 0; y < NUM_CELLS; y++)
                {
                    cells_[x,y] = null;
                }
            }
        }

        public void add(Unit unit)
        {
            // Determine which grid cell it's in.
            int cellX = (int)(unit.x_ / Grid.CELL_SIZE);
            int cellY = (int)(unit.y_ / Grid.CELL_SIZE);

            // Add to the front of list for the cell it's in.
            unit.prev_ = null;
            unit.next_ = cells_[cellX,cellY];
            cells_[cellX,cellY] = unit;

            if (unit.next_ != null)
            {
                unit.next_.prev_ = unit;
            }
        }

        public void move(Unit unit, double x, double y)
        {
            // See which cell it was in.
            int oldCellX = (int)(unit.x_ / Grid.CELL_SIZE);
            int oldCellY = (int)(unit.y_ / Grid.CELL_SIZE);

            // See which cell it's moving to.
            int cellX = (int)(x / Grid.CELL_SIZE);
            int cellY = (int)(y / Grid.CELL_SIZE);

            unit.x_ = x;
            unit.y_ = y;

            // If it didn't change cells, we're done.
            if (oldCellX == cellX && oldCellY == cellY)
            {
                return;
            }

            // Unlink it from the list of its old cell.
            if (unit.prev_ != null)
            {
                unit.prev_.next_ = unit.next_;
            }

            if (unit.next_ != null)
            {
                unit.next_.prev_ = unit.prev_;
            }

            // If it's the head of a list, remove it.
            if (cells_[oldCellX, oldCellY] == unit)
            {
                cells_[oldCellX, oldCellY] = unit.next_;
            }

            // Add it back to the grid at its new cell.
            add(unit);
        }

    }
}
