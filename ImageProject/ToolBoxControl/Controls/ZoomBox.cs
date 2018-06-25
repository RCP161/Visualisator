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

        public DesignerCanvas DesignerCanvas
        {
            get { return (DesignerCanvas)GetValue(DesignerCanvasProperty); }
            set { SetValue(DesignerCanvasProperty, value); }
        }

        public static readonly DependencyProperty DesignerCanvasProperty = DependencyProperty.Register("DesignerCanvas", typeof(DesignerCanvas), typeof(ZoomBox), new FrameworkPropertyMetadata(null));

        public Designer Designer
        {
            get { return (Designer)GetValue(DesignerProperty); }
            set { SetValue(DesignerProperty, value); }
        }

        public static readonly DependencyProperty DesignerProperty = DependencyProperty.Register("Designer", typeof(Designer), typeof(ZoomBox), new FrameworkPropertyMetadata(null));


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

             DesignerCanvas.LayoutUpdated += new EventHandler( DesignerCanvas_LayoutUpdated);

             zoomThumb.DragDelta += new DragDeltaEventHandler( Thumb_DragDelta);

             zoomSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>( ZoomSlider_ValueChanged);

             scaleTransform = new ScaleTransform();
             DesignerCanvas.LayoutTransform =  scaleTransform;
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

        
        private void DesignerCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            double viewWidth, viewHeight;
             InvalidateScale(out double scale, out double xOffset, out double yOffset);

            if (ScrollViewer.ComputedHorizontalScrollBarVisibility != Visibility.Visible)
                viewWidth = DesignerCanvas.ActualWidth;
            else
                viewWidth =  ScrollViewer.ViewportWidth;

            if (ScrollViewer.ComputedVerticalScrollBarVisibility != Visibility.Visible)
                viewHeight = DesignerCanvas.ActualHeight;
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
            // designer canvas size
            double w =  DesignerCanvas.ActualWidth *  scaleTransform.ScaleX;
            double h =  DesignerCanvas.ActualHeight *  scaleTransform.ScaleY;

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
