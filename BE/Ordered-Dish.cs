using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Ordered_Dish
    {
        public int orderNumber{get;set;} // the number of the branch
        public int dishNumber{get;set;} // the dish number
        public int amountDish{get;set;} // how much dish was invited        
        public override string ToString()
        {
            return orderNumber + " " + dishNumber + " " + amountDish;
        }
    }
}
