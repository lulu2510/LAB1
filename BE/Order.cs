using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order 
    {
        public int orderNumber { get; set; }   //the number of the order
        public DateTime orderDate = DateTime.Now;   //the date of the order
        public int branchNumber { get; set;}   // מס הסניף שממנו התבצעה ההזמנה
        public kosherLevel hechsher { get; set; } 
        public int countDeliver { get; set; } // מספר השליחים בסניף שהמנה הושמנה בה
        public string clientName { get; set; } //the name of the client
        public string clientCity { get; set; }  //the client address
        public string clientAddress { get; set; }  //the client address
        public int cardNumber { get; set; } //  מספר כרטיס האשראי שממנו נגבה הכסף של מחיר ההזמנה
        public long clientPhoneNomber { get; set; }//client phone nomber
        public override string ToString()
        {
            return "order number: " + orderNumber + "\n" +
                   "order date: " + orderDate + "\n" +
                   "branch number: " + branchNumber + "\n" +
                   "hechsher: " + hechsher + "\n" +
                   "clientName: " + clientName + "\n" +
                   "clientCity: " + clientCity + "\n" +
                   "clientPhoneNomber" + clientPhoneNomber + "\n" 
                   ;
        }
    }
}
