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
using System.Xml.Linq;
using System.Windows.Resources;
using System.IO;
using System.Text;

namespace Geopeana
{
    public partial class Page1 : PhoneApplicationPage
    {
        Boolean firstLoad = true;
        
        public Page1()
        {
            InitializeComponent();

            fromTextBox.Visibility = Visibility.Collapsed;
            toTextBox.Visibility = Visibility.Collapsed;


            
            
        }

        public class CountryItem
        {
            public string country { get; set; }
        }


        private void fromLimitCheck_Checked(object sender, RoutedEventArgs e)
        {
            fromTextBox.Visibility = Visibility.Visible;
        }

        private void toLimitCheck_Checked(object sender, RoutedEventArgs e)
        {
            toTextBox.Visibility = Visibility.Visible;
        }

        private void toLimitCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            toTextBox.Visibility = Visibility.Collapsed;
        }

        private void fromLimitCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            fromTextBox.Visibility = Visibility.Collapsed;
        }

        private void CountryListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!firstLoad)
                SearchFilter.country = CountryListPicker.SelectedItem.ToString();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            XElement countries = XElement.Load("countries.xml");


            var items = from entry in countries.Elements("country")
                        select new CountryItem { country = (string)entry.Value };
            foreach (var item in items)
            {
                CountryListPicker.Items.Add(item.country);
            }
            firstLoad = false;

            try
            {
                CountryListPicker.SelectedItem = SearchFilter.country;
            }
            catch
            {
                CountryListPicker.SelectedItem = "All Europe";
            }
        }





    }
}