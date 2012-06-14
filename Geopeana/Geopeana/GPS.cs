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
using System.Device.Location;

namespace Geopeana
{
    public class GPS
    {
        private GeoCoordinateWatcher gcw;
        public string status;
        public delegate void posFound(double lat, double lon);
        public event posFound posFoundEvent;

        public double lat {get; set;}
        public double lon { get; set; }

        //Constructor
        public GPS(){
            gcw = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            gcw.MovementThreshold = 20;
            gcw.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(StatusChangedHandler);
            gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(PositionChangedHandler);
            gcw.Start();
        }

        public void GetPosition(){
        gcw.Start();
        }

        private void StatusChangedHandler(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (gcw.Permission == GeoPositionPermission.Denied)
                        status = "Permission denied";
                    else
                        status = "GPS is not supported";

                    MessageBox.Show(status);
                    break;

                case GeoPositionStatus.Initializing:
                    break;

                case GeoPositionStatus.NoData:
                    status = "Cannot retrieve data";
                    MessageBox.Show(status);
                    break;

                case GeoPositionStatus.Ready:
                    status = "Data retrieved";
                    break;

                default:
                    break;
            }
        }

        private void PositionChangedHandler(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
            {
                return;
            }
            lat  = e.Position.Location.Latitude;
            lon  = e.Position.Location.Longitude;
            posFoundEvent(lat, lon);
            gcw.Stop();
        }
    }
}
