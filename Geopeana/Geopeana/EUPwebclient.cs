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

        public void lookup(string city)
        {
            WebClient EuropeanaClient = new WebClient();
            EuropeanaClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EUP_DownloadStringCompleted);
            EuropeanaClient.DownloadStringAsync(new Uri("http://api.europeana.eu/api/opensearch.rss?searchTerms=" + city + "+AND+europeana_type:*IMAGE*&wskey=ZICPOGYUWT"));
        }

        public void lookup(double lat, double lon)
        {
            WebClient EuropeanaClient = new WebClient();
            EuropeanaClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EUP_DownloadStringCompleted);
            EuropeanaClient.DownloadStringAsync(new Uri("http://api.europeana.eu/api/opensearch.rss?searchTerms=enrichment_place_latitude%3A[" + (lat - 0.1).ToString() + "+TO+" + (lat + 0.1).ToString() + "]+AND+enrichment_place_longitude%3A[" + (lon - 0.1).ToString() + "+TO+" + (lon + 0.1).ToString() + "]+AND+europeana_type:*IMAGE*&wskey=ZICPOGYUWT"));
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
