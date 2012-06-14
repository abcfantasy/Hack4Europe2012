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
using System.Windows.Data;
using System.Linq;

namespace Geopeana
{
    public class DynamicListBox : ListBox
    {
        public delegate void ApproachingEndOfList();
        public event ApproachingEndOfList ApproachingEndOfListEvent;

        private ScrollViewer _sv;

        DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register(
               "ListVerticalOffset",
               typeof(double),
               typeof(DynamicListBox),
               new PropertyMetadata(new PropertyChangedCallback(OnListVerticalOffsetChanged)));

        private static void OnListVerticalOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DynamicListBox control = obj as DynamicListBox;
            control.OnListVerticalOffsetChanged();
        }

        private void OnListVerticalOffsetChanged()
        {
            if (_sv.VerticalOffset >= _sv.ScrollableHeight - _sv.ViewportHeight )
            {
                if (ApproachingEndOfListEvent != null)
                    ApproachingEndOfListEvent();
            }
        }

        public void EnsureBoundToScrollViewer()
        {
            if (_sv != null)
                return;

            //_sv = this.GetTemplateChild("ScrollViewer") as ScrollViewer;
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(new Rect(0, 0, this.Width, this.Height), this);

            _sv = elements.Where(x => x is ScrollViewer).FirstOrDefault() as ScrollViewer;

            //if (_listScrollViewer == null)
            //    return;

            Binding binding = new Binding();
            binding.Source = _sv;
            binding.Path = new PropertyPath("VerticalOffset");
            binding.Mode = BindingMode.OneWay;
            this.SetBinding(ListVerticalOffsetProperty, binding);
        }

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
