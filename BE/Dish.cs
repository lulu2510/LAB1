using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Dish
    {
        public int dishId { get; set; }  //the id of the dish
        public string dishName { get; set; }  //the name of the dish
        public Size dishSize { get; set; }  // the size of the dish
        public float dishPrice { get; set; } // the price of 1 dish
        public kosherLevel hechsher { get; set; } 
        public override string ToString()
        {
            return dishId + " " + dishName + " " + dishSize+" "+dishPrice+" "+hechsher;
        }
     
    }
}
