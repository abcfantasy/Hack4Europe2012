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

namespace Geopeana
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();

            fromYearPicker.Visibility = Visibility.Collapsed;
            toYearPicker.Visibility = Visibility.Collapsed;

            XElement countries;

            StreamResourceInfo xml =
            Application.GetResourceStream(new Uri("countries.xml", UriKind.Relative));
            countries = XElement.Load(xml.Stream);
             
      

            var items = from entry in countries.Elements("country")
                                        select new CountryItem{country = (string) entry.Value};
            foreach (var item in items)
            {
                CountryListPicker.Items.Add(item.country);
            }
        }

        public class CountryItem
        {
            public string country { get; set; }
        }


        private void fromLimitCheck_Checked(object sender, RoutedEventArgs e)
        {
            fromYearPicker.Visibility = Visibility.Visible;
        }

        private void toLimitCheck_Checked(object sender, RoutedEventArgs e)
        {
            toYearPicker.Visibility = Visibility.Visible;
        }

        private void toLimitCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            toYearPicker.Visibility = Visibility.Collapsed;
        }

        private void fromLimitCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            fromYearPicker.Visibility = Visibility.Collapsed;
        }





    }
}