using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{

    public class Dal_imp : Idal
    {
        #region dish
        /// <summary>
        ///  check if the dish already exists
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public bool findDish(int dd)
        {
            foreach (Dish item in DataSource.dishes)
                if (dd == item.dishId)
                    return true;
            return false;
        }
        /// <summary>
        /// add a new dish 
        /// </summary>
        /// <param name="d"></param>
        public void addDish(Dish d)
        {
            if (!findDish(d.dishId))
                DataSource.dishes.Add(d);
            else
                throw new Exception("the dish is already exsits");
        }
        /// <summary>
        /// delete from the data base
        /// </summary>
        /// <param name="tmpId"></param>
        public void deleteDish(int dishId)
        {
            for (int i = 0; i < DataSource.dishes.Count; i++)
            {
                if (dishId == DataSource.dishes[i].dishId)
                {
                    DataSource.dishes.RemoveAt(i);
                    DataSource.ordered_dishes.RemoveAll(od => dishId == od.orderNumber);
                }
            }
        }
        /// <summary>
        /// update from the data base
        /// </summary>
        /// <param name="d"></param>
        public void updateDish(Dish d)
        {
            for (int i = 0; i < DataSource.dishes.Count; i++)
                if (DataSource.dishes[i].dishId == d.dishId)
                    DataSource.dishes[i] = d;
            throw new Exception("this Dish is not exist");
        }
        public Dish getDish(int numD)
        {
            return DataSource.dishes.FirstOrDefault(d => d.dishId == numD);
        }//***********************
        public IEnumerable<Dish> getAllDish(Func<Dish, bool> predicat = null)
        {
            if (predicat == null)
                return DataSource.dishes.AsEnumerable();
            return DataSource.dishes.Where(predicat);
        }

        #endregion
        #region branch
        /// <summary>
        /// check if the branch already exists
        /// </summary>
        /// <param name="b"></param>
        public bool findBranch(int numberBranch)
        {
            if (DataSource.branches.Count == 0)
                return false;
            foreach (Branch item in DataSource.branches)
                if (numberBranch == item.branchNumber)
                    return true;
            return false;
        }
        public void addBranch(Branch b)
        {
            if (!findBranch(b.branchNumber))
                DataSource.branches.Add(b);
            else
                throw new Exception("this Branch is already exist");
        }
        public void deleteBranch(int numberBr) //recieve branch number
        {
            if (!findBranch(numberBr))
                throw new Exception("this Branch is not exist");
            DataSource.orders.RemoveAll(od => od.branchNumber == numberBr);
            DataSource.ordered_dishes.RemoveAll(br => br.orderNumber == numberBr);
            DataSource.branches.RemoveAll(br => br.branchNumber == numberBr);
        }
        public void updateBranch(Branch b)
        {
            bool flag = false;
            for (int i = 0; i < DataSource.branches.Count; i++)
                if (DataSource.branches[i].branchNumber == b.branchNumber)
                {
                    DataSource.branches[i] = b;
                    flag = true;
                }
            if (!flag)
                throw new Exception("this Branch is not exist");
        }
        public Branch getBranch(int numB)
        {
            return DataSource.branches.FirstOrDefault(b => b.branchNumber == numB);
        }
        public IEnumerable<Branch> getAllBranch(Func<Branch, bool> predicat = null)
        {
            if (predicat == null)
                return DataSource.branches.AsEnumerable();
            return DataSource.branches.Where(predicat);
        }


        #endregion
        #region order
        /// <summary>
        /// check if the dish already exists
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public bool findOrder(Order dd)
        {
            if (DataSource.orders.Count == 0)
                return false;
            foreach (Order item in DataSource.orders)
                if (dd.orderNumber == item.orderNumber)
                    return true;
            return false;
        }
        public void addOrder(Order o)
        {
            foreach (Order item in DataSource.orders)
            {
                Console.WriteLine(item.ToString());
            }
            if (!findOrder(o))
                DataSource.orders.Add(o);
            else
                throw new Exception("this order is already exsits");
        }
        public void deleteOrder(Order o)
        {
            if (findOrder(o))
                DataSource.orders.RemoveAll(d => d.orderNumber == o.orderNumber);
            else
                throw new Exception("this order is not exist");
        }
        public void updateOrder(Order o)
        {
            for (int i = 0; i < DataSource.orders.Count; i++)
                if (DataSource.orders[i].orderNumber == o.orderNumber)
                    DataSource.orders[i] = o;
            throw new Exception("this order is not exist");
        }

        #endregion

        #region orderDish
        public void updateOrderDish(Ordered_Dish newOd, Ordered_Dish oldOd)
        {
            for (int i = 0; i < DataSource.ordered_dishes.Count; i++)
            {
                if (DataSource.ordered_dishes[i].orderNumber == oldOd.orderNumber)
                {
                    DataSource.ordered_dishes[i] = oldOd;

                }
            }
        }
        public IEnumerable<Order> getAllOrder(Func<Order, bool> predicat = null)
        {
            if (predicat == null)
                return DataSource.orders.AsEnumerable();
            return DataSource.orders.Where(predicat);
        }



        /// <summary>
        /// check if the Ordered_Dish already exists
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public bool findOrderDish(Ordered_Dish dd)
        {
            foreach (Ordered_Dish item in DataSource.ordered_dishes)
                if (dd.orderNumber != item.orderNumber && dd.dishNumber!=item.dishNumber)
                    return false;
            return true;
        }
        public void addOrdereDish(Ordered_Dish od)
        {
            if (DataSource.ordered_dishes.Exists(a => (a.orderNumber == od.orderNumber && a.dishNumber == od.dishNumber)))
                throw new Exception("the dish is already exsits at this order , you can try update Order-Dish");
            DataSource.ordered_dishes.Add(od);

        }
        public void deleteOrderDish(Ordered_Dish od)
        {
            if (DataSource.ordered_dishes.Exists(a => (a.orderNumber == od.orderNumber && a.dishNumber == od.dishNumber)))
                throw new Exception("the dish is not exsits at this order ");
            DataSource.ordered_dishes.Remove(od);
        }
        public IEnumerable<Ordered_Dish> getAllOrdered_Dish(Func<Ordered_Dish, bool> predicat = null)
        {
            if (predicat == null)
                return DataSource.ordered_dishes.AsEnumerable();
            return DataSource.ordered_dishes.Where(predicat);
        }
        #endregion


        public List<Order> getListOrderes()
        {
            return DataSource.orders;
        }
        public List<Dish> getListDishes()
        {
            return DataSource.dishes;
        }
        public List<Ordered_Dish> getListOrderDishes()
        {
            return DataSource.ordered_dishes;
        }
        public List<Branch> getListBranches()
        {
            return DataSource.branches;
        }

        public string convertDishIdToDishName(int dishId)
        {
            if (DataSource.dishes.Count == 0)
                throw new Exception(" there is no dishes at the list ");
            foreach (Dish d in getAllDish())
                if (d.dishId == dishId)
                    return d.dishName;
            throw new Exception(" there is no such dish ");
        
        }


    }
}
