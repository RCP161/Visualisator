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

        private static Style _zoomBoxStyle;
        private Style ZoomBoxStyle
        {
            get
            {
                if (_zoomBoxStyle == null)
                {
                    Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/ToolBoxControl;component/Styles/ZoomBoxStyle.xaml", UriKind.RelativeOrAbsolute) });
                    _zoomBoxStyle = FindResource("ZoomBoxStyle") as Style;
                }
                return _zoomBoxStyle;
            }
        }

        public ZoomBox() : base()
        {
            Style = ZoomBoxStyle;
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

            // TODO : Anders machen? Binding und DependencyProperties? Doch Style auflösen?

            this.zoomThumb = Template.FindName("PART_ZoomThumb", this) as Thumb;
            if (this.zoomThumb == null)
                throw new Exception("PART_ZoomThumb template is missing!");

            this.zoomCanvas = Template.FindName("PART_ZoomCanvas", this) as Canvas;
            if (this.zoomCanvas == null)
                throw new Exception("PART_ZoomCanvas template is missing!");

            this.zoomSlider = Template.FindName("PART_ZoomSlider", this) as Slider;
            if (this.zoomSlider == null)
                throw new Exception("PART_ZoomSlider template is missing!");

            this.DesignerCanvas.LayoutUpdated += new EventHandler(this.DesignerCanvas_LayoutUpdated);

            this.zoomThumb.DragDelta += new DragDeltaEventHandler(this.Thumb_DragDelta);

            this.zoomSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.ZoomSlider_ValueChanged);

            this.scaleTransform = new ScaleTransform();
            this.DesignerCanvas.LayoutTransform = this.scaleTransform;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double scale = e.NewValue / e.OldValue;

            double halfViewportHeight = this.ScrollViewer.ViewportHeight / 2;
            double newVerticalOffset = ((this.ScrollViewer.VerticalOffset + halfViewportHeight) * scale - halfViewportHeight);

            double halfViewportWidth = this.ScrollViewer.ViewportWidth / 2;
            double newHorizontalOffset = ((this.ScrollViewer.HorizontalOffset + halfViewportWidth) * scale - halfViewportWidth);

            this.scaleTransform.ScaleX *= scale;
            this.scaleTransform.ScaleY *= scale;

            this.ScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);
            this.ScrollViewer.ScrollToVerticalOffset(newVerticalOffset);

        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double scale, xOffset, yOffset;
            this.InvalidateScale(out scale, out xOffset, out yOffset);

            this.ScrollViewer.ScrollToHorizontalOffset(this.ScrollViewer.HorizontalOffset + e.HorizontalChange / scale);
            this.ScrollViewer.ScrollToVerticalOffset(this.ScrollViewer.VerticalOffset + e.VerticalChange / scale);
        }

        
        private void DesignerCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            double scale, xOffset, yOffset, viewWidth, viewHeight;
            this.InvalidateScale(out scale, out xOffset, out yOffset);

            if (ScrollViewer.ComputedHorizontalScrollBarVisibility != Visibility.Visible)
                viewWidth = DesignerCanvas.ActualWidth;
            else
                viewWidth = this.ScrollViewer.ViewportWidth;

            if (ScrollViewer.ComputedVerticalScrollBarVisibility != Visibility.Visible)
                viewHeight = DesignerCanvas.ActualHeight;
            else
                viewHeight = this.ScrollViewer.ViewportHeight;

            this.zoomThumb.Width = viewWidth * scale;
            this.zoomThumb.Height = viewHeight * scale;

            // TODO : Wenn zoomSlider.Value <= 100 muss auch der inhalt der Zoombox schrumpfen. Zudem Beachten: Nach vergößern/verkleinern der ZoomBox muss der Rahmen auch noch stimmen

            // System.Diagnostics.Debug.WriteLine("Zoomthumb: " + zoomThumb.Width.ToString("000000.000") + " / " + zoomThumb.Height.ToString("000000.000"));

            Canvas.SetLeft(this.zoomThumb, xOffset + this.ScrollViewer.HorizontalOffset * scale);
            Canvas.SetTop(this.zoomThumb, yOffset + this.ScrollViewer.VerticalOffset * scale);
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            // designer canvas size
            double w = this.DesignerCanvas.ActualWidth * this.scaleTransform.ScaleX;
            double h = this.DesignerCanvas.ActualHeight * this.scaleTransform.ScaleY;

            // zoom canvas size
            double x = this.zoomCanvas.ActualWidth;
            double y = this.zoomCanvas.ActualHeight;
            
            double scaleX = x / w;
            double scaleY = y / h;

            scale = (scaleX < scaleY) ? scaleX : scaleY;

            xOffset = (x - scale * w) / 2;
            yOffset = (y - scale * h) / 2;
        }
    }
}
