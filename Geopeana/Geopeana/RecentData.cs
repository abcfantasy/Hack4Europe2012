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
        public delegate void recentImageFound(EUPItem item);
        public event recentImageFound recentImageFoundEvent;

        private static readonly int HISTORY_SIZE = 5;

        private RecentData()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;

            if (storage.Contains("recent"))
                data = (List<string>)storage["recent"];
            else
                data = new List<string>();
        }

        public void AddToRecent(string guid)
        {
            if (data.Count == 1)
            {
                if (data[0] == "No recent entries")
                    data.Clear();
            }

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
            if (storage.Contains("recent"))
                storage.Remove("recent");

            storage.Add("recent", data);
            storage.Save();
        }

        public List<string> Retrieve()
        {
            GuidHelper guidHelper = new GuidHelper();
            guidHelper.imageFoundEvent += new GuidHelper.imageFound(guidHelper_imageFoundEvent);

            foreach (var d in data)
            {
                guidHelper.getImageInfo(d);
            }

            return data;
        }

        void guidHelper_imageFoundEvent(string imageUrl, string detailsUrl)
        {
            EUPItem item = new EUPItem();
            item.Thumbnail = imageUrl;
            item.Link = detailsUrl;

            if (recentImageFoundEvent != null)
                recentImageFoundEvent(item);
        }

        #region Singleton instance
        private static RecentData instance;

        public static RecentData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RecentData();
                    instance.AddToRecent("No recent entries");
                }

                return instance;
            }
        }
        #endregion
    }
}
