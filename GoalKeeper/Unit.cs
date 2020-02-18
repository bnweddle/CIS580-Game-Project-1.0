using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Unit
    {
        private double X, Y;
        private Grid Grid;
        private Unit Next;
        private Unit Prev;

        public Unit(Grid grid, double x, double y)
        {
            this.Grid = grid;
            this.X = x;
            this.Y = y;
            Next = null;
            Prev = null;
        }      
    }
}
