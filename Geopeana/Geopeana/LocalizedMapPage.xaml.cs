using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Collections;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;

namespace Geopeana
{
    public partial class LocalizedMapPage : PhoneApplicationPage
    {
        private string currentCity;
        private MapLayer imageLayer;

        public LocalizedMapPage( string currentCity )
        {
            InitializeComponent();

            this.currentCity = currentCity;

            // test data
            //LocationData.Instance["abc"] = new SimpleCoordinates("Valletta", 14.508628, 35.896);
            //LocationData.Instance["bcd"] = new SimpleCoordinates("Valletta", 14.505367, 35.894453);
            
            InitializeMap();
        }

        public void InitializeMap()
        {
            // get all localized entries
            IEnumerator localizedEntries = LocationData.Instance.GetEnumerator();
            List<GeoCoordinate> locations = new List<GeoCoordinate>();

            imageLayer = new MapLayer();
            map1.Children.Add(imageLayer);

            // loop through localized entries
            while ( localizedEntries.MoveNext() )
            {
                KeyValuePair<string, SimpleCoordinates> entry = (KeyValuePair<string, SimpleCoordinates>)localizedEntries.Current;
                
                if ( entry.Value.City == currentCity )
                {
                    //Pushpin pin = new Pushpin();
                    GeoCoordinate location = new System.Device.Location.GeoCoordinate(entry.Value.Latitude, entry.Value.Longitude);
                    locations.Add(location);

                    Image pinImage = new Image();
                    pinImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("marker-red.png", UriKind.Relative));
                    pinImage.Opacity = 0.9;
                    pinImage.Stretch = System.Windows.Media.Stretch.None;
                    pinImage.Tag = entry.Value;
                    pinImage.Tap += new EventHandler<GestureEventArgs>(pin_Tap);

                    imageLayer.AddChild(pinImage, location);
                }
            }

            // set map view
            map1.SetView( LocationRect.CreateLocationRect(locations) );
        }

        void pin_Tap(object sender, GestureEventArgs e)
        {
            // get SimpleCoordinates object from the source object Tag
            throw new NotImplementedException();
        }
    }
}