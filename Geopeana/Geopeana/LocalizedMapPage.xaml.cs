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
        private Canvas detailsCanvas;
        private Image detailsImage;

        public LocalizedMapPage()
        {
            InitializeComponent();

            // test data
            LocationData.Instance["http://www.europeana.eu/portal/record/08547/6A0C00645813BEF4B4D7CDB36AFAFB9524B99F97.html"] = new SimpleCoordinates(14.508628, 35.896);
            LocationData.Instance["bcd"] = new SimpleCoordinates(14.505367, 35.894453);

            GPS gps = new GPS();
            gps.posFoundEvent += new GPS.posFound(gps_posFoundEvent);
            gps.GetPosition();
        }

        void gps_posFoundEvent(double lat, double lon)
        {
            //googleCityLookup cityLookup = new googleCityLookup(lat, lon);
            currentLat = lat;
            currentLon = lon;
            InitializeMap();
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
            //List<GeoCoordinate> locations = new List<GeoCoordinate>();

            imageLayer = new MapLayer();
            map1.Children.Add(imageLayer);

            detailsCanvas = new Canvas();
            map1.Children.Add(detailsCanvas);

            // loop through localized entries
            while ( localizedEntries.MoveNext() )
            {
                KeyValuePair<string, SimpleCoordinates> entry = (KeyValuePair<string, SimpleCoordinates>)localizedEntries.Current;
                
                if ( LocationIsClose(entry.Value.Latitude, entry.Value.Longitude) )
                {
                    //Pushpin pin = new Pushpin();
                    GeoCoordinate location = new System.Device.Location.GeoCoordinate(entry.Value.Latitude, entry.Value.Longitude);
                   // locations.Add(location);

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
            map1.SetView(new GeoCoordinate(currentLat, currentLon), 15.0);
        }

        private bool LocationIsClose(double lat, double lon)
        {
            return ((currentLat - lat <= 0.1) && (currentLat - lat >= -0.1) && (currentLon - lon <= 0.1) && (currentLon - lon >= -0.1));
        }

        void pin_Tap(object sender, GestureEventArgs e)
        {            
            string guid = (String)((Image)e.OriginalSource).Tag;

            GuidHelper guidHelper = new GuidHelper();
            guidHelper.imageFoundEvent += new GuidHelper.imageFound(guidHelper_imageFoundEvent);
            guidHelper.getImageInfo(guid);
        }

        void guidHelper_imageFoundEvent(string imageUrl, string detailsUrl)
        {
            if (detailsImage != null)
            {
                detailsImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                detailsImage.Tag = detailsUrl;
            }
            else
            {
                detailsImage = new Image();
                detailsImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                detailsImage.MaxHeight = 128.0;
                detailsImage.MaxWidth = 128.0;
                detailsImage.Stretch = Stretch.UniformToFill;
                detailsImage.Tag = detailsUrl;
                detailsImage.Tap += new EventHandler<GestureEventArgs>(detailsImage_Tap);
                Canvas.SetLeft(detailsImage, 10.0);
                Canvas.SetTop(detailsImage, 10.0);
                detailsCanvas.Children.Add(detailsImage);
            }
        }

        void detailsImage_Tap(object sender, GestureEventArgs e)
        {
            string url = (String)( (Image)e.OriginalSource ).Tag;
            NavigationService.Navigate(new Uri("/Details.xaml?selectedItem=" + url, UriKind.Relative));
        }

        
    }
}