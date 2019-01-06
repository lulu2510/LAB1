using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public class BL_imp : IBL
    {
        Idal dal = FactoryDalXml.getdal();
        static int maxPrice = 500;
        #region dish
        public void addDish(Dish d)
        {
            dal.addDish(d);
        }
        public void deleteDish(int dishId)
        {
            dal.deleteDish(dishId);
        }
        public void updateDish(Dish d)
        {
            dal.updateDish(d);
        }
        public IEnumerable<Dish> getAllDish(Func<Dish, bool> predicat = null)
        {
            return dal.getAllDish(predicat);
        }
        #endregion
        #region branch
        public void addBranch(Branch b)
        {
            if ((b.branchPhone < 100000000) || (b.branchPhone > 9999999999))
                throw new Exception("Wrong phone number");
            if (b.branchNumWorkers == 0)
                throw new Exception("there is no workers at this branch");
            if(b.branchNumWorkers<b.freeDeliverNum)
                throw new Exception("free Delivers can't be more than the number of workers ");
            dal.addBranch(b);
        }
        public void deleteBranch(int numberBr)
        {
            dal.deleteBranch(numberBr);
        }
        public void updateBranch(Branch b)
        {
            dal.updateBranch(b);
        }

        public Branch getBranch(int numB)
        {
            return dal.getBranch(numB);
        }
        public IEnumerable<Branch> getAllBranch(Func<Branch, bool> predicate = null)
        {
            return dal.getAllBranch(predicate);
        }
        #endregion
        #region order
        public void addOrder(Order o)
        {
            if (o.orderNumber < 10000000 || o.orderNumber > 99999999)
                throw new Exception("the order number have to be 8 digits");
            Branch b = dal.getAllBranch(br => o.branchNumber == br.branchNumber).FirstOrDefault();
            if (b == null)
                throw new Exception("this branch doesn't exist");
            if (o.hechsher != b.hechsher)
                throw new Exception("the choosed branch dosn't match the choosed hechsher");
            if ((o.clientPhoneNomber < 10000000) || (o.clientPhoneNomber > 999999999))
                throw new Exception("Wrong phone number");
            if (!(o.orderDate.Year == DateTime.Now.Year && o.orderDate.Month == DateTime.Now.Month && o.orderDate.Day == DateTime.Now.Day))
                throw new Exception("The date is not today");
            dal.addOrder(o);
        }
        public void deleteOrder(Order o)
        {
            dal.deleteOrder(o);
        }
        public void updateOrder(Order o)
        {
            Branch b = dal.getAllBranch(br => o.branchNumber == br.branchNumber).FirstOrDefault();
            if (b == null)
                throw new Exception("this branch doesn't exist");
            if (o.hechsher != b.hechsher)
                throw new Exception("the choosed branch dosn't match the choosed hechsher");
            if ((o.clientPhoneNomber < 10000000) || (o.clientPhoneNomber > 999999999))
                throw new Exception("Wrong phone number");
            if (!(o.orderDate.Year == DateTime.Now.Year && o.orderDate.Month == DateTime.Now.Month && o.orderDate.Day == DateTime.Now.Day))
                throw new Exception("The date is not today");
            dal.updateOrder(o);
        }
        public IEnumerable<Order> getAllOrder(Func<Order, bool> predicat = null)
        {
            return dal.getAllOrder(predicat);
        }
        public IEnumerable<object> getDishesToOrder(int orderNumber)
        {
            //return from item1 in dal.getAllOrdered_Dish()
            //       where item1.orderNumber == orderNumber
            //       from item2 in dal.getAllDish()
            //       where item1.dishNumber == item2.dishId
            //       select new { name = item2.dishName, amount = item1.amountDish, price = item1.amountDish * item2.dishPrice };
            return from item1 in dal.getAllOrdered_Dish()
                   where item1.orderNumber == orderNumber
                   from item2 in dal.getAllDish()
                   where item1.dishNumber == item2.dishId
                   select new { name = item2.dishName, amount = item1.amountDish, price = item1.amountDish * item2.dishPrice };
        }
        #endregion
        #region order-dish
        public void addOrderedDish(Ordered_Dish od)
        {
            Dish tmpD = dal.getAllDish(d => od.dishNumber == d.dishId).FirstOrDefault();
            if (tmpD == null)
                throw new Exception("this dish is not exist");
            Order o = dal.getAllOrder(or => or.orderNumber == od.orderNumber).FirstOrDefault();
            if (o.hechsher > tmpD.hechsher)
                throw new Exception("this hechsher isn't good");
            dal.addOrdereDish(od);
        }
        public void updateOrderDish(Ordered_Dish newOd, Ordered_Dish oldOd)
        {
            dal.updateOrderDish(newOd,oldOd);
        }
        public void deleteOrderDish(Ordered_Dish od)
        {
            dal.deleteOrderDish(od);
        }
        public IEnumerable<Ordered_Dish> getAllOrdered_Dish(Func<Ordered_Dish, bool> predicat = null)
        {
            return dal.getAllOrdered_Dish(predicat);
        }
        #endregion
        #region get lists
        public List<Order> getListOrderes()
        {
            return dal.getListOrderes();
        }
        public List<Dish> getListDishes()
        {
            return dal.getListDishes();
        }
        public List<Branch> getListBranches()
        {
            return dal.getListBranches();
        }
        public List<Ordered_Dish> getListOrderDishes()
        {
            return dal.getListOrderDishes();
        }

        #endregion
        public float paymentOrder(int numberInvitation)
        {
            float sum = 0, dishPrice;
            var od1 = dal.getAllOrdered_Dish(od => od.orderNumber == numberInvitation);
            foreach (Ordered_Dish item in dal.getAllOrdered_Dish())
            {
                foreach (Dish item2 in dal.getAllDish())
                {
                    if (item.dishNumber == item2.dishId)
                    {
                        dishPrice = item2.dishPrice;
                        sum += item.amountDish * item2.dishPrice;
                        break;
                    }
                }
            }

            return sum;
        }
        public bool paymentLimit(float paymentOrder)
        {
            return paymentOrder > maxPrice;
        }
        #region grouping
        public float groupingByKosherLevel(kosherLevel k)
        {
            float sum = 0;
            var v = from item in dal.getListOrderes()
                    group paymentOrder(item.orderNumber) by item.hechsher;
            foreach (var hecherGroup in v)
            {
                if (hecherGroup.Key == k)
                {
                    //foreach (var ord in hecherGroup)
                    //    sum += ord;
                    //break;
                    hecherGroup.Sum();
                    break;
                }
            }
            return sum;
        }
        public IEnumerable<IGrouping<string, float>> groupingBydish()
        {
           // float sum = 0;
            return  from item in dal.getListOrderDishes()
                    let d=dal.getDish(item.dishNumber)
                    group item.amountDish*d.dishPrice by d.dishName ;
        }
        public float profitBydish()
        {
            // float sum = 0;
            IEnumerable<IGrouping<string, float>> v= from item in dal.getListOrderDishes()
                   let d = dal.getDish(item.dishNumber)
                   group item.amountDish * d.dishPrice by d.dishName;
            return (float)3.4;
        }
        public float groupingByDate(DateTime date)
        {
            float sum = 0;
            var v = from item in dal.getListOrderes()
                    group paymentOrder(item.orderNumber) by item.orderDate.Month;
            foreach (var monthGroup in v)
            {
                if (monthGroup.Key == date.Month)
                {
                    foreach (var ord in monthGroup)
                        sum += ord;
                    break;
                }
            }
            return sum;
        }
        public float groupingByResidence(string cityName)
        {
            float sum = 0;
            var v = from o in dal.getListOrderes()
                    group paymentOrder(o.orderNumber) by o.clientCity;
            foreach (var cityGroup in v)
            {
                if (cityGroup.Key == cityName)
                    foreach (var ord in cityGroup)
                        sum += ord;
                break;
            }
            return sum;

        }
        #endregion

        public string convertDishIdToDishName(int dishId)
        {
            return dal.convertDishIdToDishName(dishId);
        }
    }

       
 
}