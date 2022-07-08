using System.Collections.Generic;

namespace DataExtensions
{
    public class DataRenewer 
    {
        public static Dictionary<T1, T2> Renew<T1, T2>(Dictionary<T1, T2> items)
        {
            if (items != null)
                items.Clear();
            else
                items = new Dictionary<T1, T2>();
            return items;
        }
        public static List<T> Renew<T>(List<T> items)
        {
            if (items != null)
                items.Clear();
            else
                items = new List<T>();
            return items;
        }
        public static HashSet<T> Renew<T>(HashSet<T> items)
        {
            if (items != null)
                items.Clear();
            else
                items = new HashSet<T>();
            return items;
        }
    }
}