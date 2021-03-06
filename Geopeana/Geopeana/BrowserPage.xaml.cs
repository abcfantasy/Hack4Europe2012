﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;

namespace Geopeana
{
    public partial class MainPage : PhoneApplicationPage
    {
        ProgressIndicator prog;
        EUPwebclient EuropeanaAPI;
        GPS phoneGPS;
        googleCityLookup cityFinder;
        ScrollViewer scrollViewer;

        
        double lat; double lon;
        bool searchInProgress;
        int page = 1;
        String Country;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            //Load Europeana API
            EuropeanaAPI = new EUPwebclient();
            EuropeanaAPI.searchDoneEvent += new EUPwebclient.searchDone(EuropeanaAPI_searchDoneEvent);

            //Load GPS
            phoneGPS = new GPS();
            phoneGPS.posFoundEvent += new GPS.posFound(phoneGPS_posFoundEvent);

            //Load city finder
            cityFinder = new googleCityLookup();
            cityFinder.cityFoundEvent += new googleCityLookup.cityFound(cityFinder_cityFoundEvent);

            //Progress bar control
            SystemTray.SetIsVisible(this, true);
            SystemTray.SetOpacity(this, 0);
            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.IsIndeterminate = true;
            prog.Text = "Loading...";
            SystemTray.SetProgressIndicator(this, prog);

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

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (ResultsListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/Details.xaml?selectedItem=" + ((EUPItem)ResultsListBox.SelectedItem).Link, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            ResultsListBox.SelectedIndex = -1;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void phoneGPS_posFoundEvent(double lat, double lon)
        {
            if (lat == this.lat && lon == this.lon)
                return;

            this.lat = lat;
            this.lon = lon;

            cityFinder.SendRequest(lat, lon);
            EuropeanaAPI.lookup(lat, lon);
        }

        public void cityFinder_cityFoundEvent(string city, string country)
        {
            Country = country;

            SearchFilter.country = country;
            prog.Text = "Current city: " + city;
            prog.IsIndeterminate = true;
            prog.IsVisible = true;

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 3); // Display for 3 seconds
            dt.Tick += new EventHandler(elapsedTimerHandler);
            dt.Start();
        }

        private void elapsedTimerHandler(object sender, EventArgs e)
        {
            prog.IsIndeterminate = false;
            prog.IsVisible = false;
            prog.Text = "Loading...";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (CityBox.Text.Length > 0)
            {
                page = 0;
                searchInProgress = true;
                ResultsListBox.Items.Clear();

                prog.IsIndeterminate = true;
                prog.IsVisible = true;
                EuropeanaAPI.lookup(CityBox.Text);
            }
        }

        //Show the searchresults in the list
        void EuropeanaAPI_searchDoneEvent(XElement SearchResults)
        {
            searchInProgress = false;

            if (SearchResults == null)
            {
                prog.IsIndeterminate = false;
                prog.IsVisible = false;
                return;
            }

            /*
            ResultsListBox.ItemsSource = from item in SearchResults.Element("channel").Descendants("item")
                                         select new EUPItem
                                         {
                                             Thumbnail = item.Element("enclosure") != null ? item.Element("enclosure").Attribute("url").Value : "image_not_available.jpg",
                                             Link = item.Element("guid").Value,
                                             Title = item.Element("title").Value
                                         };
            */
            var items = from item in SearchResults.Element("channel").Descendants("item")
                                         select new EUPItem
                                         {
                                             Thumbnail = item.Element("enclosure") != null ? item.Element("enclosure").Attribute("url").Value : "image_not_available.jpg",
                                             Link = item.Element("guid").Value,
                                             Title = item.Element("title").Value
                                         };
            foreach (var item in items)
            {
                if ( !(SearchFilter.unplaced && LocationData.Instance.Contains(item.Link) ) )
                    ResultsListBox.Items.Add(item);
            }

            if (ResultsListBox.Items.Count > 0)
            {
                //Dispatcher.BeginInvoke(new Action(delegate() { ResultsListBox.ScrollIntoView(ResultsListBox.Items[0]); }));
                prog.IsIndeterminate = false;
                prog.IsVisible = false;

                // get scroll view
                scrollViewer = ResultsListBox.GetScrollViewer();
                scrollViewer.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(ResultsListBox_ManipulationCompleted);
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvancedSearch.xaml", UriKind.Relative));
        }

        private void ResultsListBox_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            ResultsListBox.EnsureBoundToScrollViewer();
        }

        private void ResultsListBox_ApproachingEndOfListEvent( object sender, EventArgs e )
        {
            // increment page here, search and retrieve more results
            if (!searchInProgress)
            {
                page++;
                EuropeanaAPI.repeatSearch(page);
                searchInProgress = true;
                
            }
        }
    }
}