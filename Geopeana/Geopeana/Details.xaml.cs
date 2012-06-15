using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace Geopeana
{
    public partial class Details : PhoneApplicationPage
    {
        private string guid;

        public Details()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";
            
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out msg))
            {
                guid = msg;

                // Add to recent entry visits
                RecentData.Instance.AddToRecent(guid);

                // was entry faved?
                if (FavoriteData.Instance.Contains(guid))
                {
                    buttonFav.Content = "Faved!";
                    buttonFav.IsEnabled = false;
                }

                // was entry pinned?
                if (LocationData.Instance.Contains(guid))
                {
                    PinIt.Content = "Unpin it";
                    PinIt.Click -= PinIt_Click;
                    PinIt.Click += new RoutedEventHandler(UnpinIt_Click);
                }

                try
                {
                    webBrowser1.Navigate(new Uri(msg));
                }
                catch (System.UriFormatException)
                {
                    return;
                }
            }
        }

        private void PinIt_Click(object sender, RoutedEventArgs e)
        {
            GPS gps = new GPS();
            gps.posFoundEvent += new GPS.posFound(gps_posFoundEvent);
            NavigationService.Navigate(new Uri("/SuccessPage.xaml", UriKind.Relative));
        }

        private void UnpinIt_Click(object sender, RoutedEventArgs e)
        {
            LocationData.Instance.Remove(guid);
            PinIt.Content = "Pin It!";
            PinIt.Click -= UnpinIt_Click;
            PinIt.Click += new RoutedEventHandler(PinIt_Click);
        }

        void gps_posFoundEvent(double lat, double lon)
        {
            LocationData.Instance[guid] = new SimpleCoordinates(lon, lat);
        }

        // pin it, fav it, share it!
        private void FavIt_Click(object sender, RoutedEventArgs e)
        {
            FavoriteData.Instance.AddToFavorites(guid);
            buttonFav.Content = "Faved!";
            buttonFav.IsEnabled = false;
        }
    }
}