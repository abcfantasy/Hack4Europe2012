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

namespace Geopeana
{
    /// <summary>
    /// Usage
    /// Create new instance of class.
    /// To add elements: Data[key] = new SimpleCoordinates(long, lat);
    /// To get element: Location = Data[key];
    /// </summary>
    public class LocationData
    {
        IsolatedStorageSettings storage;

        // get virtual space for application
        public LocationData()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;
        }

        // indexer to get or add entries
        public SimpleCoordinates this[string guid]
        {
            get
            {
                return (SimpleCoordinates)storage[guid];
            }
            set
            {
                storage.Add(guid, value);
                storage.Save();
            }
        }
    }
}
