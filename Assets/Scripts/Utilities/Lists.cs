using System;
using System.Collections.Generic;

namespace DataExtensions
{
    public class Lists
    {
        public static int FindIndex<T>(List<T> allDataInList, T data)
        {
            int index = -1;
            for (int i = 0; i < allDataInList.Count; i++)
            {
                if (allDataInList[i].Equals(data))
                {
                    index = i;
                    break;
                }
            }
            return IndexException(index);
        }
        public static int FindIndex(List<IMazeObjects> allDataInList, IMazeObjects data)
        {
            int index = allDataInList.FindIndex(x => x.id == data.id);
            //for (int i = 0; i < allDataInList.Count; i++)
            //{
            //    UnityEngine.Debug.Log("<color=red>   id:  " + allDataInList[i].id + "  </color>     !!!  "+ data.id);

            //}

            return IndexException(index);
        }

        private static int IndexException(int index)
        {
#if UNITY_EDITOR
            if (index < 0)
                throw (new Exception("No Data Has Been Found"));
            else
                return index;
#else
        return index;
#endif
        }

        public static List<T> Clone<T>(List<T> items)//shallow copy
        {
            List<T> newItem = new List<T>(items.ToArray());
            return newItem;
        }
    }
}