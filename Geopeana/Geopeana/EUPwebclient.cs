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
            EuropeanaClient.DownloadStringAsync(new Uri("http://api.europeana.eu/api/opensearch.rss?searchTerms=" + city + "&type:IMAGE&wskey=ZICPOGYUWT"));
        }

        void EUP_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            XElement xmlItems = XElement.Parse(e.Result);

            if (xmlItems.Element("channel").Element("item") == null)
                return;

            SearchResults = xmlItems;
            if (searchDoneEvent != null) searchDoneEvent(SearchResults);

        }
    }
}
