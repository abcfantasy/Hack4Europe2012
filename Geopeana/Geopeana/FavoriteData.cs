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
    public class FavoriteData
    {
        IsolatedStorageSettings storage;
        private List<EUPItem> data;

        private static readonly int HISTORY_SIZE = 5;

        private FavoriteData()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;

            if (storage.Contains("data"))
                data = (List<EUPItem>)storage["data"];
            else
                data = new List<EUPItem>();
        }

        public void AddToRecent(EUPItem item)
        {
            if (data.Count == 1)
            {
                if (data[0].Title == "No recent entries")
                    data.Clear();
            }

            if (data.Count < HISTORY_SIZE)
                data.Add(item);
            else
            {
                data.Insert(0, item);
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

        public List<EUPItem> Retrieve()
        {
            return data;
        }

        #region Singleton instance
        private static FavoriteData instance;

        public static FavoriteData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FavoriteData();
                    EUPItem item = new EUPItem();
                    item.Link = "";
                    item.Thumbnail = "";
                    item.Title = "No recent entries";
                    instance.AddToRecent(item);
                }

                return instance;
            }
        }
        #endregion
    }
    }
}
