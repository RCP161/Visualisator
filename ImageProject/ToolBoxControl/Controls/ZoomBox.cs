using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ToolBoxControl.Controls
{
    internal class ZoomBox : Control
    {
        private Thumb zoomThumb;
        private Canvas zoomCanvas;
        private Slider zoomSlider;
        private ScaleTransform scaleTransform;

        static ZoomBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomBox), new FrameworkPropertyMetadata(typeof(ZoomBox)));
        }

        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(ZoomBox), new FrameworkPropertyMetadata(null));

        public ItemsControl DesignerArea
        {
            get { return (ItemsControl)GetValue(DesignerAreaProperty); }
            set { SetValue(DesignerAreaProperty, value); }
        }

        public static readonly DependencyProperty DesignerAreaProperty = DependencyProperty.Register("DesignerArea", typeof(ItemsControl), typeof(ZoomBox), new FrameworkPropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

             zoomThumb = Template.FindName("PART_ZoomThumb", this) as Thumb;
            if ( zoomThumb == null)
                throw new Exception("PART_ZoomThumb template is missing!");

             zoomCanvas = Template.FindName("PART_ZoomCanvas", this) as Canvas;
            if ( zoomCanvas == null)
                throw new Exception("PART_ZoomCanvas template is missing!");

             zoomSlider = Template.FindName("PART_ZoomSlider", this) as Slider;
            if ( zoomSlider == null)
                throw new Exception("PART_ZoomSlider template is missing!");

             DesignerArea.LayoutUpdated += new EventHandler(DesignerArea_LayoutUpdated);

             zoomThumb.DragDelta += new DragDeltaEventHandler( Thumb_DragDelta);

             zoomSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>( ZoomSlider_ValueChanged);

             scaleTransform = new ScaleTransform();
             DesignerArea.LayoutTransform =  scaleTransform;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double scale = e.NewValue / e.OldValue;

            double halfViewportHeight =  ScrollViewer.ViewportHeight / 2;
            double newVerticalOffset = (( ScrollViewer.VerticalOffset + halfViewportHeight) * scale - halfViewportHeight);

            double halfViewportWidth =  ScrollViewer.ViewportWidth / 2;
            double newHorizontalOffset = (( ScrollViewer.HorizontalOffset + halfViewportWidth) * scale - halfViewportWidth);

             scaleTransform.ScaleX *= scale;
             scaleTransform.ScaleY *= scale;

             ScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);
             ScrollViewer.ScrollToVerticalOffset(newVerticalOffset);

        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            InvalidateScale(out double scale, out double xOffset, out double yOffset);

            ScrollViewer.ScrollToHorizontalOffset( ScrollViewer.HorizontalOffset + e.HorizontalChange / scale);
             ScrollViewer.ScrollToVerticalOffset( ScrollViewer.VerticalOffset + e.VerticalChange / scale);
        }

        
        private void DesignerArea_LayoutUpdated(object sender, EventArgs e)
        {
            double viewWidth, viewHeight;
             InvalidateScale(out double scale, out double xOffset, out double yOffset);

            if (ScrollViewer.ComputedHorizontalScrollBarVisibility != Visibility.Visible)
                viewWidth = DesignerArea.ActualWidth;
            else
                viewWidth =  ScrollViewer.ViewportWidth;

            if (ScrollViewer.ComputedVerticalScrollBarVisibility != Visibility.Visible)
                viewHeight = DesignerArea.ActualHeight;
            else
                viewHeight =  ScrollViewer.ViewportHeight;

             zoomThumb.Width = viewWidth * scale;
             zoomThumb.Height = viewHeight * scale;

            // TODO : Wenn zoomSlider.Value <= 100 muss auch der inhalt der Zoombox schrumpfen. Zudem Beachten: Nach vergößern/verkleinern der ZoomBox muss der Rahmen auch noch stimmen

            // System.Diagnostics.Debug.WriteLine("Zoomthumb: " + zoomThumb.Width.ToString("000000.000") + " / " + zoomThumb.Height.ToString("000000.000"));

            Canvas.SetLeft( zoomThumb, xOffset +  ScrollViewer.HorizontalOffset * scale);
            Canvas.SetTop( zoomThumb, yOffset +  ScrollViewer.VerticalOffset * scale);
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            // DesignerArea size
            double w =  DesignerArea.ActualWidth *  scaleTransform.ScaleX;
            double h =  DesignerArea.ActualHeight *  scaleTransform.ScaleY;

            // zoom canvas size
            double x =  zoomCanvas.ActualWidth;
            double y =  zoomCanvas.ActualHeight;
            
            double scaleX = x / w;
            double scaleY = y / h;

            scale = (scaleX < scaleY) ? scaleX : scaleY;

            xOffset = (x - scale * w) / 2;
            yOffset = (y - scale * h) / 2;
        }
    }
}
