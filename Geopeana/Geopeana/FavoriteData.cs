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
        private List<string> data;
        public delegate void favoriteImageFound(EUPItem item);
        public event favoriteImageFound favoriteImageFoundEvent;

        private static readonly int HISTORY_SIZE = 5;

        private FavoriteData()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;

            if (storage.Contains("favorites"))
                data = (List<string>)storage["favorites"];
            else
                data = new List<string>();
        }

        public void AddToFavorites(string guid)
        {
            /*if (data.Count == 1)
            {
                if (data[0] == "No recent entries")
                    data.Clear();
            }*/
            if (data.Contains(guid))
                return;

            if (data.Count < HISTORY_SIZE)
                data.Add(guid);
            else
            {
                data.Insert(0, guid);
                data.RemoveAt(data.Count - 1);
            }
        }

        public bool Contains(string value)
        {
            return data.Contains(value);
        }

        public void Save()
        {
            if (storage.Contains("favorites"))
                storage.Remove("favorites");

            storage.Add("favorites", data);
            storage.Save();
        }

        public void Retrieve()
        {
            GuidHelper guidHelper = new GuidHelper();
            guidHelper.imageFoundEvent += new GuidHelper.imageFound(guidHelper_imageFoundEvent);

            foreach (var d in data)
            {
                guidHelper.getImageInfo(d);
            }

            //return data;
        }

        void guidHelper_imageFoundEvent(string imageUrl, string detailsUrl)
        {
            EUPItem item = new EUPItem();
            item.Thumbnail = imageUrl;
            item.Link = detailsUrl;

            if (favoriteImageFoundEvent != null)
                favoriteImageFoundEvent(item);
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
                    //instance.AddToFavorites("No recent entries");
                }

                return instance;
            }
        }
        #endregion
    }
}
