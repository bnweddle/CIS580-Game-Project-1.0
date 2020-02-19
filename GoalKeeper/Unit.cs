using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Unit
    {
        public double x_;
        public double y_;

        public Unit prev_;
        public Unit next_;
        Grid grid_;

        public Unit(Grid grid, double x, double y)
        {
            this.grid_ = grid;
            this.x_ = x;
            this.y_ = y;
            prev_ = null;
            next_ = null;
            grid_.add(this);

        }

        public void move(double x, double y)
        {
            grid_.move(this, x, y);
        }
    }
}
