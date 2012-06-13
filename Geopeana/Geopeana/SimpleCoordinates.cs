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

namespace Geopeana
{
    public struct SimpleCoordinates
    {
        public double Longitude;
        public double Latitude;

        public SimpleCoordinates(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

    }
}
