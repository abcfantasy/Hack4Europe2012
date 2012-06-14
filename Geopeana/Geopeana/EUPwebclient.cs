using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Geopeana
{
    public class EUPwebclient
    {
        public XElement SearchResults;
        public delegate void searchDone(XElement Result);
        public event searchDone searchDoneEvent;
        private static readonly string Europeana_API_key = "ZICPOGYUWT";

        public void lookup(string city)
        {
            // Build the query string
            string query = "http://api.europeana.eu/api/opensearch.rss?searchTerms=" + city + "+AND+europeana_type:*IMAGE*&wskey=" + Europeana_API_key;

            WebClient EuropeanaClient = new WebClient();
            EuropeanaClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EUP_DownloadStringCompleted);
            EuropeanaClient.DownloadStringAsync(new Uri(query));
        }

        public void lookup(double lat, double lon)
        {
            // Lower- and upper bounds for latitude and longitude
            string lb_lat = (lat - 0.01).ToString().Replace(",", ".");
            string ub_lat = (lat + 0.01).ToString().Replace(",", ".");
            string lb_lon = (lon - 0.01).ToString().Replace(",", ".");
            string ub_lon = (lon + 0.01).ToString().Replace(",", ".");
            string query = "http://api.europeana.eu/api/opensearch.rss?searchTerms=enrichment_place_latitude%3A[" + lb_lat + "+TO+" + ub_lat + "]+AND+enrichment_place_longitude%3A[" + lb_lon + "+TO+" + ub_lon + "]+AND+europeana_type:*IMAGE*&wskey=" + Europeana_API_key;

            WebClient EuropeanaClient = new WebClient();
            EuropeanaClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EUP_DownloadStringCompleted);
            EuropeanaClient.DownloadStringAsync(new Uri(query));
        }

        void EUP_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            XElement xmlItems = XElement.Parse(e.Result);

            if (xmlItems.Element("channel").Element("item") == null)
            {
                if (searchDoneEvent != null) searchDoneEvent(null);
                return;
            }

            SearchResults = xmlItems;
            if (searchDoneEvent != null) searchDoneEvent(SearchResults);

        }
    }
}
