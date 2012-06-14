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
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections;

namespace Geopeana
{
    /// <summary>
    /// Usage
    /// Do not create new instance. Get singleton instance using Data.Instance
    /// To add elements: LocationData.Instance[key] = new SimpleCoordinates(city, long, lat);
    /// To get element: Location = LocationData.Instance[key];
    /// </summary>
    public class LocationData
    {
        IsolatedStorageSettings storage;
        private Dictionary<string, SimpleCoordinates> data;

        // get virtual space for application
        private LocationData()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;   
            
            if ( storage.Contains("data") )
                data = (Dictionary<string, SimpleCoordinates>)storage["data"];
            else
                data = new Dictionary<string, SimpleCoordinates>();
        }

        // indexer to get or add entries
        public SimpleCoordinates this[string guid]
        {
            get
            {
                return data[guid];
            }
            set
            {
                if (!data.ContainsKey(guid))
                {
                    data.Add(guid, value);
                    Save();
                }
            }
        }

        public Dictionary<string, SimpleCoordinates>.Enumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public void Save()
        {
            if (storage.Contains("data"))
                storage.Remove("data");

            storage.Add("data", data);

            storage.Save();
        }

        #region Singleton Instance
        private static LocationData instance;

        public static LocationData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocationData();
                    //instance.data = new Dictionary<string, SimpleCoordinates>();
                }
                return instance;
            }
        }
        #endregion
    }
}
