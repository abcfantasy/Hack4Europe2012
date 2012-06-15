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
using System.Linq;
using System.Collections.Generic;

namespace Geopeana
{
    public class googleCityLookup
    {
            #region Member variables
            private static string googleapi_baserequest = "http://maps.google.com/maps/geo?q={0},{1}&output=csv&sensor=false";
            private static string googleapi_secure_xmlrequest = "https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";

            #region Properties

            private string Street { get; set; }
            public string City { get; set; }
            public string Country { get; set; }

            public delegate void cityFound(string city, string country);
            public event cityFound cityFoundEvent;
            
            #endregion
            #endregion

            #region Client API

            public googleCityLookup()
            {            
            }

            public googleCityLookup(double latitude, double longitude)
            {
                SendRequest(latitude, longitude);
            }

            public void SendRequest(double latitude, double longitude)
            {
                WebClient wc = new WebClient();

                wc.DownloadStringAsync(new Uri(String.Format(googleapi_secure_xmlrequest, latitude.ToString().Replace(",", "."), longitude.ToString().Replace(",", "."))));
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringCompletedHandler);
            }

            #endregion

            #region Private methods
            private void DownloadStringCompletedHandler(object sender, DownloadStringCompletedEventArgs e)
            {
                XElement address= XElement.Parse(e.Result);

                IEnumerable<string> adresse = from component in address.Descendants("address_component")
                                              where (string)component.Element("type").Value == "locality"
                                              select (string)component.Element("long_name").Value;
                
                City = adresse.Count<string>() > 0 ? adresse.First<string>() : "";

                IEnumerable<string> adresse2 = from component in address.Descendants("address_component")
                                              where (string)component.Element("type").Value == "country"
                                              select (string)component.Element("long_name").Value;

                Country = adresse2.Count<string>() > 0 ? adresse2.First<string>() : "";

                if (cityFoundEvent != null) cityFoundEvent(City, Country);
            }
            #endregion
        }
    
}

//from component in address.Descendants("GeocodeResponse").Descendants("result")
//where (string) component.Element("address_component").Element("type").Value == "locality"
