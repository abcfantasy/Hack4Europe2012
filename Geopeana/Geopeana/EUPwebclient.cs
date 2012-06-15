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
using System.Collections.Generic;
using System.Linq;

namespace Geopeana
{
    public class EUPwebclient
    {
        public XElement SearchResults;
        public delegate void searchDone(XElement Result);
        public event searchDone searchDoneEvent;
        private static readonly string Europeana_API_key = "ZICPOGYUWT";
        private string query;

        public void repeatSearch(int page)
        {
            WebClient EuropeanaClient = new WebClient();
            EuropeanaClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EUP_DownloadStringCompleted);
            EuropeanaClient.DownloadStringAsync(new Uri(query + "&startPage=" + page));
        }

        public void lookup(string keyword)
        {
            if (SearchFilter.country == "All Europe")
            {
                // Build the query string
                query = "http://api.europeana.eu/api/opensearch.rss?searchTerms=" + keyword + "+AND+europeana_type:*IMAGE*&wskey=" + Europeana_API_key;

                WebClient EuropeanaClient = new WebClient();
                EuropeanaClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EUP_DownloadStringCompleted);
                EuropeanaClient.DownloadStringAsync(new Uri(query));
            }
            else
            {
                lookup(keyword, SearchFilter.country);
            }
        }

        public void lookup(string keyword, string country)
        {
            //check for yearlimits
            string curYear = DateTime.Now.Year.ToString();
            string timeelement="";
            if(SearchFilter.fromLimit && SearchFilter.toLimit)
                timeelement = "+AND+enrichment_period_begin%3A["+SearchFilter.fromYear+"-01-01T00%3A00%3A00Z+TO+"+SearchFilter.toYear+"-01-01T23%3A59%3A59Z]+AND+enrichment_period_end%3A["+SearchFilter.fromYear+"-01-01T00%3A00%3A00Z+TO+"+SearchFilter.toYear+"-01-01T23%3A59%3A59Z]";
            else if(SearchFilter.fromLimit)
                timeelement = "+AND+enrichment_period_begin%3A["+SearchFilter.fromYear+"-01-01T00%3A00%3A00Z+TO+"+curYear+"-01-01T23%3A59%3A59Z]+AND+enrichment_period_end%3A["+SearchFilter.fromYear+"-01-01T00%3A00%3A00Z+TO+"+curYear+"-01-01T23%3A59%3A59Z]";
            else if(SearchFilter.toLimit)
                timeelement = "+AND+enrichment_period_begin%3A[" + 0 + "-01-01T00%3A00%3A00Z+TO+" + SearchFilter.toYear + "-01-01T23%3A59%3A59Z]+AND+enrichment_period_end%3A[" + 0 + "-01-01T00%3A00%3A00Z+TO+" + SearchFilter.toYear + "-01-01T23%3A59%3A59Z]";


            // Build the query string

            query = "http://api.europeana.eu/api/opensearch.rss?searchTerms=" + keyword + timeelement+ "+AND+europeana_type:*IMAGE*+AND+europeana_country:*" + country.ToLower() + "*&wskey=" + Europeana_API_key;

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
            query = "http://api.europeana.eu/api/opensearch.rss?searchTerms=enrichment_place_latitude%3A[" + lb_lat + "+TO+" + ub_lat + "]+AND+enrichment_place_longitude%3A[" + lb_lon + "+TO+" + ub_lon + "]+AND+europeana_type:*IMAGE*&wskey=" + Europeana_API_key;

            WebClient EuropeanaClient = new WebClient();
            EuropeanaClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EUP_DownloadStringCompleted);
            EuropeanaClient.DownloadStringAsync(new Uri(query));
        }

        public void lookup(double lat, double lon, int page)
        {
            // Lower- and upper bounds for latitude and longitude
            string lb_lat = (lat - 0.01).ToString().Replace(",", ".");
            string ub_lat = (lat + 0.01).ToString().Replace(",", ".");
            string lb_lon = (lon - 0.01).ToString().Replace(",", ".");
            string ub_lon = (lon + 0.01).ToString().Replace(",", ".");
            query = "http://api.europeana.eu/api/opensearch.rss?searchTerms=enrichment_place_latitude%3A[" + lb_lat + "+TO+" + ub_lat + "]+AND+enrichment_place_longitude%3A[" + lb_lon + "+TO+" + ub_lon + "]+AND+europeana_type:*IMAGE*&startPage=" + page + "&wskey=" + Europeana_API_key;

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
