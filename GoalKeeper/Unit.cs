using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Unit
    {
        private float x;
        private float y;
        private int id;
        private Unit prev;
        private Unit next;
        private Grid grid;

        //constructor
        public Unit(float x, float y, int id, Grid grid)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            this.id = id;
            prev = null;
            next = null;
        }

        public void Update(float x, float y)
        {
            grid.Update(this, x, y);
        }

        //getters and setters
        public float X
        {
            get; set;
        }

        public float Y
        {
            get; set;
        }

        public int Id
        {
            get; set;
        }

        public Unit Prev
        {
            get; set;
        }

        public Unit Next
        {
            get; set;
        }
    }
}
