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
    public class GuidHelper
    {
        private static readonly XNamespace dc = "http://purl.org/dc/elements/1.1/";
        private static readonly XNamespace europeana = "http://www.europeana.eu";
        private static readonly XNamespace srw = "http://www.loc.gov/zing/srw/";
        private static readonly XNamespace enrichment = "http://www.europeana.eu/schemas/ese/enrichment/";
        private static readonly XNamespace dcterms = "http://purl.org/dc/terms/";

        public delegate void imageFound(string imageUrl, string detailsUrl);
        public event imageFound imageFoundEvent;

        public const string APIkey = "ZICPOGYUWT";

        public void getImageInfo(string guid)
        {
            WebClient Europeana = new WebClient();

            Europeana.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Europeana_DownloadStringCompleted);
            string test = xmlUrl(guid);
            Europeana.DownloadStringAsync(new Uri(xmlUrl(guid)));
        }

        void Europeana_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            XElement xmlItems = XElement.Parse(e.Result);
            string imageUrl = xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "object") != null ? xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "object").Value : "";
            string europeanaLink = xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "object") != null ? xmlItems.Element(srw + "records").Element(srw + "record").Element(srw + "recordData").Element(dc + "dc").Element(europeana + "uri").Value : "";
            if (imageFoundEvent != null) imageFoundEvent(imageUrl, europeanaLink);
        }

        string xmlUrl(string guid)
        {
            char[] delimiterChars = {'.'};
            string[] part = guid.Split(delimiterChars);
            string url = "";
            for (int i = 0; i < part.Length - 1; i++)
                url += part[i] + ".";
            return url + "srw?wskey=" + APIkey;
        }
    }
}
