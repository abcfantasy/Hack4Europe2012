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

            // add sample test data
            this["http://www.europeana.eu/portal/record/09421/81F7D02542BF7B77BE9F0DBC1D2017D5503953A5.html"] = new SimpleCoordinates(4.699835, 50.874304);
            this["http://www.europeana.eu/portal/record/09421/401D3574118D2D7166F73946C56FCD73434C1D15.html"] = new SimpleCoordinates(4.698633, 50.877689);
            this["http://www.europeana.eu/portal/record/92106/FA96E7D51F97A14C7FF2548E03F3CDF14132A8D0.html"] = new SimpleCoordinates(4.706666, 50.878421);
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

        public void Remove(string guid)
        {
            if (data.ContainsKey(guid))
                data.Remove(guid);
            Save();
        }

        public Dictionary<string, SimpleCoordinates>.Enumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public bool Contains(string guid)
        {
            return data.ContainsKey(guid);
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
