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
    public partial class SuccessPage : PhoneApplicationPage
    {
        public SuccessPage()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //base.OnBackKeyPress(e);
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void buttonMap_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LocalizedMapPage.xaml", UriKind.Relative));
        }
    }
}