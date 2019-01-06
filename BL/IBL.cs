using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
    {
        #region dish
        void addDish(Dish d); //הוספת מנה
        void deleteDish(int dishId); //מחיקת מנה
        void updateDish(Dish d); //עדכון פרטי מנה קיימת
        IEnumerable<Dish> getAllDish(Func<Dish, bool> predicat = null);
        #endregion
        #region branch
        void addBranch(Branch b);   //הוספת סניף
        void deleteBranch(int numberBr);   //מחיקת סניף
        void updateBranch(Branch b);  //עדכון סניף קיים
        Branch getBranch(int numB);
        IEnumerable<Branch> getAllBranch(Func<Branch, bool> predicate = null);
        #endregion
        #region order
        void addOrder(Order o);  //הוספת הזמנה
        void deleteOrder(Order o);   //מחיקת הזמנה
        void updateOrder(Order o);  //עדכון הזמנה
        void updateOrderDish(Ordered_Dish newOd, Ordered_Dish oldOd); // עדכון מנה מוזמנת
        IEnumerable<Order> getAllOrder(Func<Order, bool> predicat = null);
        IEnumerable<object> getDishesToOrder(int orderNumber);
        #endregion

        #region orderDish
        void addOrderedDish(Ordered_Dish od);  //הוספת מנה_מוזמנת
        void deleteOrderDish(Ordered_Dish od);  //מחיקת מנה_מוזמנת
        IEnumerable<Ordered_Dish> getAllOrdered_Dish(Func<Ordered_Dish, bool> predicat = null);        
        #endregion
List<Order> getListOrderes();  // קבלת רשימת כל ההזמנות
        List<Dish> getListDishes();  //  קבלת רשימת כל המנות
        List<Branch> getListBranches();  //קבלת רשימת כל הסניפים
        List<Ordered_Dish> getListOrderDishes();  //קבלת רשימת כל המנות מוזמנות
        /// calculte the final invoice 
        /// </summary>
        /// <param name="od"> list of the order-dish</param>
        /// <returns>sum of the invoic</returns>
        bool paymentLimit(float paymentOrder);
        float groupingByDate(DateTime date);
        float groupingByKosherLevel(kosherLevel k);
        IEnumerable<IGrouping<string, float>> groupingBydish();
        float profitBydish();
        float groupingByResidence(string cityName);
        string convertDishIdToDishName(int dishId);

        //#region XML-פונקציות הקשורות ל
        //public List<Dish> GetDishList();//XElement דרך DishXml את הקובץ DISH-טוען לתוך רשימת ה
        //public void SaveDishesListLinq(List<Dish> dishList);//XML בתוך קובץ DISH-שומר את רשימת ה
        //#endregion
    }
}
