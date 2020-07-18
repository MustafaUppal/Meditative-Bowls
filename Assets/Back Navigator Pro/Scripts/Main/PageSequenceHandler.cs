using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BackNavigatorPro
{
    public class PageSequenceHandler
    {
        private static int currentID = -1;

        public static List<int> pagesInfo = new List<int>();

        public static int CurrentID
        {
            get
            {
                if (currentID == -1)
                    currentID = PersistantData.Load();

                return currentID;
            }
            set
            {
                currentID = value;
                PersistantData.Save(currentID);
            }
        }

        public static bool IsActivePage(int id)
        {
            if (pagesInfo.Count > 0)
                return pagesInfo[pagesInfo.Count - 1].Equals(id);

            return false;
        }

        public static void AddPageID(int id)
        {
            pagesInfo.Add(id);
        }

        public static void RemovePageID(int id)
        {
            if (pagesInfo.Count > 0 && pagesInfo[pagesInfo.Count - 1].Equals(id))
                pagesInfo.RemoveAt(pagesInfo.Count - 1);
        }
    }
}