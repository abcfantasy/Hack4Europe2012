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

namespace Geopeana
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        public PanoramaPage1()
        {
            InitializeComponent();
            browseTextBlock.MouseLeftButtonDown += new MouseButtonEventHandler(browseTextBlock_MouseLeftButtonDown);
            MapTextBlock.MouseLeftButtonDown += new MouseButtonEventHandler(MapTextBlock_MouseLeftButtonDown);
        }

        private void browseTextBlock_MouseLeftButtonDown(object sender,MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BrowserPage.xaml", UriKind.Relative));
        }

        private void MapTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LocalizedMapPage.xaml", UriKind.Relative));
        }
    }
}