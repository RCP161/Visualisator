using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolBoxControl.Controls
{
    /// <summary>
    /// Interaktionslogik für ZoomBox.xaml
    /// </summary>
    internal partial class ZoomBox : UserControl
    {
        private ScaleTransform scaleTransform;

        public ZoomBox() : base()
        {
            InitializeComponent();

            SnapsToDevicePixels = true;
            Loaded += ZoomBox_Loaded;
        }

        private void ZoomBox_Loaded(object sender, RoutedEventArgs e)
        {
            DesignerArea.LayoutUpdated += new EventHandler(this.DesignerCanvas_LayoutUpdated);
            ZoomThumb.DragDelta += new DragDeltaEventHandler(this.Thumb_DragDelta);
            ZoomSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.ZoomSlider_ValueChanged);

            this.scaleTransform = new ScaleTransform();
            this.DesignerArea.LayoutTransform = this.scaleTransform;
        }

        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(ZoomBox), new FrameworkPropertyMetadata(null));

        public Canvas DesignerArea
        {
            get { return (Canvas)GetValue(DesignerAreaProperty); }
            set { SetValue(DesignerAreaProperty, value); }
        }

        public static readonly DependencyProperty DesignerAreaProperty = DependencyProperty.Register("DesignerArea", typeof(Canvas), typeof(ZoomBox), new FrameworkPropertyMetadata(null));

        public Designer Designer
        {
            get { return (Designer)GetValue(DesignerProperty); }
            set { SetValue(DesignerProperty, value); }
        }

        public static readonly DependencyProperty DesignerProperty = DependencyProperty.Register("Designer", typeof(Designer), typeof(ZoomBox), new FrameworkPropertyMetadata(null));

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
                viewWidth = DesignerArea.ActualWidth;
            else
                viewWidth = this.ScrollViewer.ViewportWidth;

            if (ScrollViewer.ComputedVerticalScrollBarVisibility != Visibility.Visible)
                viewHeight = DesignerArea.ActualHeight;
            else
                viewHeight = this.ScrollViewer.ViewportHeight;

            ZoomThumb.Width = viewWidth * scale;
            ZoomThumb.Height = viewHeight * scale;

            // TODO : Wenn zoomSlider.Value <= 100 muss auch der inhalt der Zoombox schrumpfen. Zudem Beachten: Nach vergößern/verkleinern der ZoomBox muss der Rahmen auch noch stimmen

            // System.Diagnostics.Debug.WriteLine("Zoomthumb: " + zoomThumb.Width.ToString("000000.000") + " / " + zoomThumb.Height.ToString("000000.000"));

            Canvas.SetLeft(ZoomThumb, xOffset + this.ScrollViewer.HorizontalOffset * scale);
            Canvas.SetTop(ZoomThumb, yOffset + this.ScrollViewer.VerticalOffset * scale);
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            // designer canvas size
            double w = this.DesignerArea.ActualWidth * this.scaleTransform.ScaleX;
            double h = this.DesignerArea.ActualHeight * this.scaleTransform.ScaleY;

            // zoom canvas size
            double x = ZoomCanvas.ActualWidth;
            double y = ZoomCanvas.ActualHeight;

            double scaleX = x / w;
            double scaleY = y / h;

            scale = (scaleX < scaleY) ? scaleX : scaleY;

            xOffset = (x - scale * w) / 2;
            yOffset = (y - scale * h) / 2;
        }
    }
}