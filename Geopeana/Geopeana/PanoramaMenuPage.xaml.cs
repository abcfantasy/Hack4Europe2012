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
            RecentData.Instance.recentImageFoundEvent += new RecentData.recentImageFound(Instance_recentImageFoundEvent);
            FavoriteData.Instance.favoriteImageFoundEvent += new FavoriteData.favoriteImageFound(Instance_favoriteImageFoundEvent);
            //RecentListBox.ItemsSource = RecentData.Instance.Retrieve();
            //FavoritesListBox.ItemsSource = FavoriteData.Instance.Retrieve();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RecentListBox.Items.Clear();
            FavoritesListBox.Items.Clear();

            RecentData.Instance.Retrieve();
            FavoriteData.Instance.Retrieve();
        }

        void Instance_recentImageFoundEvent(EUPItem item)
        {
            RecentListBox.Items.Add(item);
        }

        void Instance_favoriteImageFoundEvent(EUPItem item)
        {
            FavoritesListBox.Items.Add(item);
        }

        private void browseTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BrowserPage.xaml", UriKind.Relative));
        }

        private void MapTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LocalizedMapPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
                while (NavigationService.CanGoBack)
                    NavigationService.RemoveBackEntry();
        }

        private void RecentSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            if (RecentListBox.SelectedIndex == -1)
                return;

            // Navigate to the entry's page
            NavigationService.Navigate(new Uri("/Details.xaml?selectedItem=" + ((EUPItem)RecentListBox.SelectedItem).Link, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            RecentListBox.SelectedIndex = -1;
        }

        private void FavoriteSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            if (FavoritesListBox.SelectedIndex == -1)
                return;

            // Navigate to the entry's page
            NavigationService.Navigate(new Uri("/Details.xaml?selectedItem=" + ((EUPItem)FavoritesListBox.SelectedItem).Link, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            FavoritesListBox.SelectedIndex = -1;
        }
    }
}