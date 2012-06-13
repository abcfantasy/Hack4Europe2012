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
using System.Xml.Linq;
using Microsoft.Phone.Controls;

namespace Geopeana
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (ResultsListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + ResultsListBox.SelectedIndex, UriKind.Relative));

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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
        EUPwebclient EuropeanaAPI =new EUPwebclient();
        EuropeanaAPI.searchDoneEvent+= new EUPwebclient.searchDone(EuropeanaAPI_searchDoneEvent);
        EuropeanaAPI.lookup(CityBox.Text);
        }
        void EuropeanaAPI_searchDoneEvent(XElement SearchResults){

        ResultsListBox.ItemsSource = from item in SearchResults.Element("channel").Descendants("item")
                                        select new EUPItem
                                        {
                                            Thumbnail = item.Element("enclosure") != null ? item.Element("enclosure").Attribute("url").Value : "",
                                            Link = item.Element("link").Value,
                                            Title = item.Element("title").Value
                                        };

        }
        

    }

    public class EUPItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Thumbnail { get; set; }
    }
}