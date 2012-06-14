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
        public String Name = "http://www.europeana.eu/portal/record/08547/6A0C00645813BEF4B4D7CDB36AFAFB9524B99F97.srw?wskey=ZICPOGYUWT";
        private static readonly XNamespace dc = "http://purl.org/dc/elements/1.1/";
        private static readonly XNamespace europeana = "http://www.europeana.eu";
        private static readonly XNamespace srw = "http://www.loc.gov/zing/srw/";
        private static readonly XNamespace enrichment = "http://www.europeana.eu/schemas/ese/enrichment/";
        private static readonly XNamespace dcterms = "http://purl.org/dc/terms/";

        public Details()
        {
            InitializeComponent();
            Parsing();
        }

        public void Parsing()
        {
            WebClient Europeana = new WebClient();

            Europeana.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Europeana_DownloadStringCompleted);
            Europeana.DownloadStringAsync(new Uri(this.Name));
        }

        void Europeana_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            XElement xmlItems = XElement.Parse(e.Result);
            //if (xmlits.Element("channel").Element("item") == null)
            //  return;
            //String country = 
            String imageUrl = xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "object") != null ? xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "object").Value : "";
            String title = xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(dc + "title") != null ? xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(dc + "title").Value : "";
            String str = xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "country") != null ? "Country: " + ToUpperFirstLetter(xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "country").Value) + "\n" : "";
            str += xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "year") != null ? "Year: " + xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "year").Value + "\n" : "";
            str += xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(dc + "description") != null ? "Description: " + xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(dc + "description").Value : "";

            textBlockDetails.Text = str;
            
            PageTitle.Text = title;
            //textBlock1.Text = item.Description;
            Uri uri = new Uri(imageUrl, UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            image1.Source = imgSource;

            hyperlinkButton1.Content = xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "uri") != null ? xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "uri").Value : "";

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        public string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
                WebBrowserTask webBrowserTask = new WebBrowserTask();

                webBrowserTask.Uri = new Uri(hyperlinkButton1.Content.ToString(), UriKind.Absolute);

               webBrowserTask.Show();

        }
        }

        
    
}