using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using DS;
using System.IO;
using System.ComponentModel;

namespace DAL
{
    public class Dal_XML_imp : Idal
    {
        XElement dishRoot;
        string dishPath = @"DishXml.xml";

        XElement branchRoot;
        string branchPath = @"branchXml.xml";

        XElement orderRoot;
        const string orderPath = @"Order.XML"; //orderPath
        XElement orderDishRoot;
        const string orderDishPath = @"orderDish.XML"; //orderDish

        public Dal_XML_imp()//בנאי
        {
            try
            {
                if (!File.Exists(dishPath))//dish
                {
                    dishRoot = new XElement("dishes");
                    dishRoot.Save(dishPath);
                }
                else dishRoot = XElement.Load(dishPath);
                if (!File.Exists(branchPath))//branch
                {
                    branchRoot = new XElement("branches");
                    branchRoot.Save(branchPath);
                }
                else branchRoot = XElement.Load(branchPath);

                if (!File.Exists(orderPath))//order
                {
                    orderRoot = new XElement("order");
                    orderRoot.Save(orderPath);
                }
                else orderRoot = XElement.Load(orderPath);
                if (!File.Exists(orderDishPath))//order dish
                {
                    orderDishRoot = new XElement("order");
                    orderDishRoot.Save(orderDishPath);
                }
                else orderDishRoot = XElement.Load(orderDishPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
            //orderRoot.RemoveAll();
            //branchRoot.Save(orderPath);
        }

        #region order
        public bool findOrder(Order dd)
        {
            XElement orderElement;
            orderElement = (from p in orderRoot.Elements()
                            where Convert.ToInt32(p.Element("orderNumber").Value) == dd.orderNumber
                            select p).FirstOrDefault();
            if (orderElement == null)
                return false;
            return true;
        }
        public void addOrder(Order o)
        {
            XElement orderNumber = new XElement("orderNumber", o.orderNumber);
            XElement branchNumber = new XElement("branchNumber", o.branchNumber);
            XElement orderDate = new XElement("orderDate", o.orderDate);
            XElement hechsher = new XElement("hechsher", o.hechsher);
            XElement countDeliver = new XElement("countDeliver", o.countDeliver);
            XElement clientName = new XElement("clientName", o.clientName);
            XElement clientCity = new XElement("clientCity", o.clientCity);
            XElement clientAddress = new XElement("clientAddress", o.clientAddress);
            XElement cardNumber = new XElement("cardNumber", o.cardNumber);
            XElement clientPhoneNomber = new XElement("clientPhoneNomber", o.clientPhoneNomber);

            orderRoot.Add(new XElement("Order", orderNumber, branchNumber, orderDate, hechsher, countDeliver, clientName, clientCity, cardNumber, clientAddress, clientPhoneNomber));
            orderRoot.Save(orderPath);

        }
        public void deleteOrder(Order o)
        {
            try
            {
                IEnumerable<XElement> orderDishElements;
                XElement orderElement;
                orderElement = (from p in orderRoot.Elements()
                                where p.Element("orderNumber").Value == o.orderNumber.ToString()
                                select p).FirstOrDefault();
                orderDishElements = from p in orderDishRoot.Elements()
                                    where Convert.ToInt32(p.Element("orderNumber").Value) == o.orderNumber
                                    select p;
                foreach (var item in orderDishElements)
                    item.Remove();
                orderDishRoot.Save(orderDishPath);
                orderElement.Remove();
                orderRoot.Save(orderPath);
            }
            catch
            {
                throw new Exception("The order dos'nt exsist in the system");
            }
        }
        public void updateOrder(Order o)
        {
            orderRoot = XElement.Load(orderPath);
            XElement order = (from item in orderRoot.Elements()
                              where item.Element("orderNumber").Value == o.orderNumber.ToString()
                              select item).FirstOrDefault();
            order.Element("branchNumber").Value = o.branchNumber.ToString();
            order.Element("hechsher").Value = o.hechsher.ToString();
            order.Element("orderDate").Value = o.orderDate.ToString();
            order.Element("countDeliver").Value = o.countDeliver.ToString();
            order.Element("clientName").Value = o.clientName.ToString();
            order.Element("clientCity").Value = o.clientCity.ToString();
            order.Element("clientAddress").Value = o.clientAddress.ToString();
            order.Element("cardNumber").Value = o.cardNumber.ToString();
            order.Element("clientPhoneNomber").Value = o.clientPhoneNomber.ToString();
            orderRoot.Save(orderPath);
        }

        public List<Order> getListOrderes()
        {
            orderRoot = XElement.Load(orderPath);
            List<Order> orders = new List<Order>();
            try
            {
                orders = (from item in orderRoot.Elements()
                          select new Order()
                          {
                              orderNumber = Convert.ToInt32(item.Element("orderNumber").Value),
                              orderDate = Convert.ToDateTime(item.Element("dateOrder").Value),
                              branchNumber = Convert.ToInt32(item.Element("branchNumber").Value),
                              hechsher = (kosherLevel)Enum.Parse(typeof(kosherLevel), item.Element("hechsher").Value),
                              countDeliver = Convert.ToInt32(item.Element("countDeliver").Value),
                              clientName = item.Element("clientName").Value,
                              clientCity = item.Element("clientCity").Value,
                              clientAddress = item.Element("clientAddress").Value,
                              cardNumber = Convert.ToInt32(item.Element("cardNumber").Value),
                              clientPhoneNomber = Convert.ToInt64(item.Element("clientPhoneNomber").Value),
                          }).ToList();
            }
            catch
            {
                orders = null;
            }

            return orders;
        }

        public IEnumerable<Order> getAllOrder(Func<Order, bool> predicat = null)
        {
            IEnumerable<Order> order;
            try
            {

                order = (from item in orderRoot.Elements()
                         select new Order()
                         {
                             orderNumber = Convert.ToInt32(item.Element("orderNumber").Value),
                             orderDate = Convert.ToDateTime(item.Element("orderDate").Value),
                             branchNumber = Convert.ToInt32(item.Element("branchNumber").Value),
                             hechsher = (kosherLevel)Enum.Parse(typeof(kosherLevel), item.Element("hechsher").Value),
                             countDeliver = Convert.ToInt32(item.Element("countDeliver").Value),
                             clientName = item.Element("clientName").Value,
                             clientCity = item.Element("clientCity").Value,
                             clientAddress = item.Element("clientAddress").Value,
                             cardNumber = Convert.ToInt32(item.Element("cardNumber").Value),
                             clientPhoneNomber = Convert.ToInt64(item.Element("clientPhoneNomber").Value),
                         }).ToList();
                if (predicat != null)
                {
                    order = order.Where(predicat);

                }
            }
            catch
            {
                order = null;
            }
            return order;
        }
        #endregion

        #region dish

        public bool findDish(int id)//מחזיר "אמת" כאשר כאשר המנה נמצאה בקובץ
        {
            XElement dishElement;
            dishElement = (from p in dishRoot.Elements()
                           where Convert.ToInt32(p.Element("dishId").Value) == id
                           select p).FirstOrDefault();
            if (dishElement == null)
                return false;
            return true;
        }

        public void addDish(Dish dish)//הוספת מנה
        {
            if (findDish(dish.dishId))
                throw new Exception("The dish is already exsist in the system");
            XElement dishId = new XElement("dishId", dish.dishId);
            XElement dishName = new XElement("dishName", dish.dishName);
            XElement dishPrice = new XElement("dishPrice", dish.dishPrice);
            XElement dishSize = new XElement("dishSize", dish.dishSize);
            XElement hechsher = new XElement("hechsher", dish.hechsher);
            dishRoot.Add(new XElement("dish", dishId, dishName, dishPrice, dishSize, hechsher));
            dishRoot.Save(dishPath);
        }
        public void deleteDish(int dishId) //מחיקת מנה
        {
            XElement dishElement;
            dishElement = (from p in dishRoot.Elements()
                           where Convert.ToInt32(p.Element("dishId").Value) == dishId
                           select p).FirstOrDefault();
            if (dishElement == null)
                throw new Exception("The dish dos'nt exsist in the system");
            dishElement.Remove();
            dishRoot.Save(dishPath);

        }
        public IEnumerable<Dish> getAllDish(Func<Dish, bool> predicat = null)
        {
            IEnumerable<Dish> dishes;
            try
            {

                dishes = from p in dishRoot.Elements()
                         select new Dish()
                         {
                             dishId = Convert.ToInt32(p.Element("dishId").Value),
                             dishName = (p.Element("dishName").Value).ToString(),
                             dishPrice = float.Parse(p.Element("dishPrice").Value),
                             dishSize = (BE.Size)Enum.Parse(typeof(BE.Size), p.Element("dishSize").Value),
                             hechsher = (BE.kosherLevel)Enum.Parse(typeof(BE.kosherLevel), p.Element("hechsher").Value)
                         };
                if (predicat != null)
                {
                    dishes = dishes.Where(predicat);

                }
            }
            catch
            {
                dishes = null;
            }
            return dishes;
        }


        public void updateDish(Dish d)
        {
            XElement dishElement = (from p in dishRoot.Elements()
                                    where Convert.ToInt32(p.Element("dishId").Value) == d.dishId
                                    select p).FirstOrDefault();
            dishElement.Element("dishName").Value = d.dishName;
            dishElement.Element("dishPrice").Value = (d.dishPrice).ToString();
            dishElement.Element("dishSize").Value = (d.dishSize).ToString();
            dishElement.Element("hechsher").Value = (d.hechsher).ToString();
            dishRoot.Save(dishPath);
        }
        public string convertDishIdToDishName(int dishId)
        {
            string dishName;
            try
            {
                dishName = (from p in dishRoot.Elements()
                            where Convert.ToInt32(p.Element("dishId").Value) == dishId
                            select p.Element("dishName").Value).FirstOrDefault();
            }
            catch
            {
                dishName = null;
                throw new Exception("The dish dos'nt exsist in the system");
            }
            return dishName;
        }

        public Dish getDish(int numD)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region branch
        public bool findBranch(int numberBranch)
        {
            XElement brancElement;
            brancElement = (from p in branchRoot.Elements()
                            where Convert.ToInt32(p.Element("branchNumber").Value) == numberBranch
                            select p).FirstOrDefault();
            if (brancElement == null)
                return false;
            return true;
        }

        public void addBranch(Branch b)
        {
            if (findBranch(b.branchNumber))
                throw new Exception("The branch is already exsist in the system");
            XElement branchNumber = new XElement("branchNumber", b.branchNumber);
            XElement branchName = new XElement("branchName", b.branchName);
            XElement branchAddress = new XElement("branchAddress", b.branchAddress);
            XElement branchPhone = new XElement("branchPhone", b.branchPhone);
            XElement branchResponsName = new XElement("branchResponsName", b.branchResponsName);
            XElement branchNumWorkers = new XElement("branchNumWorkers", b.branchNumWorkers);
            XElement freeDeliverNum = new XElement("freeDeliverNum", b.freeDeliverNum);
            XElement hechsher = new XElement("hechsher", b.hechsher);
            branchRoot.Add(new XElement("branch", branchNumber, branchName, branchAddress, branchPhone, branchResponsName, branchNumWorkers, freeDeliverNum, hechsher));
            branchRoot.Save(branchPath);
        }

        public void deleteBranch(int numberBr)
        {
            XElement branchElement;
            branchElement = (from p in branchRoot.Elements()
                             where Convert.ToInt32(p.Element("branchNumber").Value) == numberBr
                             select p).FirstOrDefault();
            if (branchElement == null)
                throw new Exception("The branch doesn't exsist in the system");
            branchElement.Remove();
            branchRoot.Save(branchPath);
        }

        public void updateBranch(Branch b)
        {

            XElement branchElement = (from p in branchRoot.Elements()
                                      where Convert.ToInt32(p.Element("branchNumber").Value) == b.branchNumber
                                      select p).FirstOrDefault();
            branchElement.Element("branchName").Value = b.branchName;
            branchElement.Element("branchAddress").Value = b.branchAddress;
            branchElement.Element("branchPhone").Value = (b.branchPhone).ToString();
            branchElement.Element("branchResponsName").Value = b.branchResponsName;
            branchElement.Element("branchNumWorkers").Value = (b.branchNumWorkers).ToString();
            branchElement.Element("freeDeliverNum").Value = (b.freeDeliverNum).ToString();
            branchElement.Element("hechsher").Value = (b.hechsher).ToString();
            branchRoot.Save(branchPath);
        }

        public Branch getBranch(int numB)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Branch> getAllBranch(Func<Branch, bool> predicat = null)
        {
            IEnumerable<Branch> branches;
            try
            {
                branches = from p in branchRoot.Elements()
                           // where predicat
                           select new Branch()
                           {
                               branchNumber = Convert.ToInt32(p.Element("branchNumber").Value),
                               branchName = p.Element("branchName").Value,
                               branchAddress = p.Element("branchAddress").Value,
                               branchPhone = Convert.ToInt64(p.Element("branchPhone").Value),
                               branchResponsName = p.Element("branchResponsName").Value,
                               branchNumWorkers = Convert.ToInt32(p.Element("branchNumWorkers").Value),
                               freeDeliverNum = Convert.ToInt32(p.Element("freeDeliverNum").Value),
                               hechsher = (BE.kosherLevel)Enum.Parse(typeof(BE.kosherLevel), p.Element("hechsher").Value)
                           };
                if (predicat != null)
                {
                    branches = branches.Where(predicat);

                }
            }
            catch
            {
                branches = null;
            }
            return branches;
        }
        #endregion

        #region order dish
        public void updateOrderDish(Ordered_Dish newOd, Ordered_Dish oldOd)
        {
            XElement orderDishElement = (from p in orderDishRoot.Elements()
                                         where Convert.ToInt32(p.Element("orderNumber").Value) == oldOd.orderNumber && Convert.ToInt32(p.Element("dishNumber")) == oldOd.dishNumber
                                         select p).FirstOrDefault();
            orderDishElement.Element("orderNumber").Value = newOd.orderNumber.ToString();
            orderDishElement.Element("dishNumber").Value = (newOd.dishNumber).ToString();
            orderDishElement.Element("amountDish").Value = (newOd.amountDish).ToString();
            orderDishRoot.Save(orderDishPath);
           // dishRoot.Save(dishPath);
        }


        public bool findOrderDish(Ordered_Dish dd)
        {
            XElement orderDishElement;
            orderDishElement = (from p in orderDishRoot.Elements()
                                where (Convert.ToInt32(p.Element("orderNumber").Value) == dd.orderNumber && Convert.ToInt32(p.Element("dishNumber").Value) == dd.dishNumber)
                                select p).FirstOrDefault();
            if (orderDishElement == null)
                return false;
            return true;
        }

        public void addOrdereDish(Ordered_Dish od)
        {
            if (findOrderDish(od))
                throw new Exception("The  dish for this order is already exsist in the system");
            XElement orderNumber = new XElement("orderNumber", od.orderNumber);
            XElement dishNumber = new XElement("dishNumber", od.dishNumber);
            XElement amountDish = new XElement("amountDish", od.amountDish);
            orderDishRoot.Add(new XElement("dish", orderNumber, dishNumber, amountDish));
            orderDishRoot.Save(orderDishPath);
        }

        public void deleteOrderDish(Ordered_Dish od)
        {
            XElement orderDishElement;
            orderDishElement = (from p in orderDishRoot.Elements()
                                where Convert.ToInt32(p.Element("dishNumber").Value) == od.dishNumber && Convert.ToInt32(p.Element("orderNumber").Value) == od.orderNumber
                                select p).FirstOrDefault();
            if (orderDishElement == null)
                throw new Exception("The order dish dos'nt exsist in the system");
            orderDishElement.Remove();
            orderDishRoot.Save(orderDishPath);
        }

        public IEnumerable<Ordered_Dish> getAllOrdered_Dish(Func<Ordered_Dish, bool> predicat = null)
        {
            IEnumerable<Ordered_Dish> orderDish;
            try
            {
                orderDish = (from item in orderDishRoot.Elements()
                             select new Ordered_Dish()
                             {
                                 orderNumber = Convert.ToInt32(item.Element("orderNumber").Value),
                                 dishNumber = Convert.ToInt32(item.Element("dishNumber").Value),
                                 amountDish = Convert.ToInt32(item.Element("amountDish").Value),
                             }).ToList();
                if (predicat != null)
                {
                    orderDish = orderDish.Where(predicat);

                }
            }
            catch
            {
                orderDish = null;
            }
            return orderDish;
        }
        #endregion
        public List<Dish> getListDishes()
        {
            throw new NotImplementedException();
        }

        public List<Ordered_Dish> getListOrderDishes()
        {
            throw new NotImplementedException();
        }

        public List<Branch> getListBranches()
        {
            throw new NotImplementedException();
        }


    }
}
