using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;


namespace DAL
{

    public interface Idal
    {
         bool findDish(int dd);
         void addDish(Dish d); //הוספת מנה
         void deleteDish(int dishId); //מחיקת מנה
         void updateDish(Dish d); //עדכון פרטי מנה קיימת
         Dish getDish(int numD);//מציאת מנה ע"פ מס' זהות
         IEnumerable<Dish> getAllDish(Func<Dish, bool> predicat = null);


         bool findBranch(int numberBranch);
         void addBranch(Branch b);   //הוספת סניף
         void deleteBranch(int numberBr);   //מחיקת סניף
         void updateBranch(Branch b);  //עדכון סניף קיים
         Branch getBranch(int numB);
         IEnumerable<Branch> getAllBranch(Func<Branch, bool> predicat = null);


         bool findOrder(Order dd);
         void addOrder(Order o);  //הוספת הזמנה
         void deleteOrder(Order o);   //מחיקת הזמנה
         void updateOrder(Order o);  //עדכון הזמנה
         void updateOrderDish(Ordered_Dish newOd, Ordered_Dish oldOd); //עדכון מנה_מוזמנת
        IEnumerable<Order> getAllOrder(Func<Order, bool> predicat = null);


         bool findOrderDish(Ordered_Dish dd);
         void addOrdereDish(Ordered_Dish od);  //הוספת מנה_מוזמנת
         void deleteOrderDish(Ordered_Dish od);  //מחיקת מנה_מוזמנת
         IEnumerable<Ordered_Dish> getAllOrdered_Dish(Func<Ordered_Dish, bool> predicat = null);

        List<Order> getListOrderes();  // קבלת רשימת כל ההזמנות
        List<Dish> getListDishes();  //  קבלת רשימת כל המנות
        List<Ordered_Dish> getListOrderDishes();  //  קבלת רשימת כל המנות מוזמנות
        List<Branch> getListBranches();  //קבלת רשימת כל הסניפים

        string convertDishIdToDishName(int dishId);

       //#region XML-פונקציות הקשורות ל
       //  List<Dish> GetDishList();//XElement דרך DishXml את הקובץ DISH-טוען לתוך רשימת ה
       //  void SaveDishesListLinq(List<Dish> dishList);//XML בתוך קובץ DISH-שומר את רשימת ה
       // #endregion
    }

}
