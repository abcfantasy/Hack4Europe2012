﻿using System;
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
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(MainPage_BackKeyPress);
        }

        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/PanoramaMenuPage.xaml", UriKind.Relative));
            });
        }


        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BrowserPage.xaml", UriKind.Relative));
        }

        private void buttonMap_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LocalizedMapPage.xaml", UriKind.Relative));
        }
    }
}