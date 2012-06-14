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
        private double currentLat;
        private double currentLon;
        private MapLayer imageLayer;

        public LocalizedMapPage()
        {
            InitializeComponent();

            GPS gps = new GPS();
            gps.posFoundEvent += new GPS.posFound(gps_posFoundEvent);
            gps.GetPosition();

            // test data
            //LocationData.Instance["abc"] = new SimpleCoordinates("Valletta", 14.508628, 35.896);
            //LocationData.Instance["bcd"] = new SimpleCoordinates("Valletta", 14.505367, 35.894453);
        }

        void gps_posFoundEvent(double lat, double lon)
        {
            //googleCityLookup cityLookup = new googleCityLookup(lat, lon);
            currentLat = lat;
            currentLon = lon;
            //cityLookup.cityFoundEvent += new googleCityLookup.cityFound(cityLookup_cityFoundEvent);
        }

        //void cityLookup_cityFoundEvent(string city)
        //{
            //this.currentCity = city;
            //InitializeMap();
        //}

        public void InitializeMap()
        {
            //currentCity = "Valletta";

            // get all localized entries
            IEnumerator localizedEntries = LocationData.Instance.GetEnumerator();
            List<GeoCoordinate> locations = new List<GeoCoordinate>();

            imageLayer = new MapLayer();
            map1.Children.Add(imageLayer);

            // loop through localized entries
            while ( localizedEntries.MoveNext() )
            {
                KeyValuePair<string, SimpleCoordinates> entry = (KeyValuePair<string, SimpleCoordinates>)localizedEntries.Current;
                
                if ( LocationIsClose(entry.Value.Latitude, entry.Value.Longitude) )
                {
                    //Pushpin pin = new Pushpin();
                    GeoCoordinate location = new System.Device.Location.GeoCoordinate(entry.Value.Latitude, entry.Value.Longitude);
                    locations.Add(location);

                    Image pinImage = new Image();
                    pinImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("marker-red.png", UriKind.Relative));
                    pinImage.Opacity = 0.9;
                    pinImage.Stretch = System.Windows.Media.Stretch.None;
                    pinImage.Tag = entry.Key;
                    pinImage.Tap += new EventHandler<GestureEventArgs>(pin_Tap);

                    imageLayer.AddChild(pinImage, location);
                }
            }

            // set map view
            map1.SetView( LocationRect.CreateLocationRect(locations) );
        }

        private bool LocationIsClose(double lat, double lon)
        {
            return ((currentLat - lat <= 0.1) && (currentLat - lat >= -0.1) && (currentLon - lon <= 0.1) && (currentLon - lon >= -0.1));
        }

        void pin_Tap(object sender, GestureEventArgs e)
        {
            throw new NotImplementedException();
            /*
            string guid = (String)((Image)e.OriginalSource).Tag;
            
            TextBlock t = new TextBlock();
            t.Margin = new Thickness(10.0, 10.0, 0.0, 0.0);
            t.Height = 20;
            t.Width = 50;
            t.Text = guid;
            map1.Children.Add(t);*/
        }
    }
}