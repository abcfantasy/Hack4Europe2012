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
    public class DynamicListBox : ListBox
    {
        //private ScrollViewer scrollViewer;

        public DynamicListBox()
            : base()
        {
            //scrollViewer = this.GetTemplateChild("ScrollViewer") as ScrollViewer;
            //scrollViewer.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(s_ManipulationCompleted);
            
        }

        
        public ScrollViewer GetScrollViewer()
        {
            ScrollViewer s = this.GetTemplateChild("ScrollViewer") as ScrollViewer;
            //s.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(s_ManipulationCompleted);
            return s;
        }

        void s_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            //if (scrollViewer.VerticalOffset > scrollViewer.ScrollableHeight)
            //{

            //}
        }
    }
}
