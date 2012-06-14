using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace Geopeana
{
    public class RecentData
    {
        IsolatedStorageSettings storage;
        private List<string> data;

        private static readonly int HISTORY_SIZE = 5;

        private RecentData()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;

            if (storage.Contains("data"))
                data = (List<string>)storage["data"];
            else
                data = new List<string>();
        }

        public void AddToRecent(string guid)
        {
            if (data.Count < HISTORY_SIZE)
                data.Add(guid);
            else
            {
                data.Insert(0, guid);
                data.RemoveAt(data.Count - 1);
            }
        }

        public void Save()
        {
            if (storage.Contains("data"))
                storage.Remove("data");

            storage.Add("data", data);
            storage.Save();
        }

        #region Singleton instance
        private static RecentData instance;

        public static RecentData Instance
        {
            get
            {
                if (instance == null)
                    instance = new RecentData();

                return instance;
            }
        }
        #endregion
    }
}
