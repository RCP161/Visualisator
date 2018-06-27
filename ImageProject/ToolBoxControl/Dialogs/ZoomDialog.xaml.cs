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
using ToolBoxControl.Controls;
using ToolBoxControl.ViewModels;

namespace ToolBoxControl.Dialogs
{
    /// <summary>
    /// Interaktionslogik für ZoomDialog.xaml
    /// </summary>
    internal partial class ZoomDialog : Window
    {
        private readonly DesignerVm viewModel;
        
        public ZoomDialog(DesignerVm vm)
        {
            InitializeComponent();
            viewModel = vm;
            DataContext = vm;
        }

        private Thumb zoomThumb;
        private Canvas zoomCanvas;
        private Slider zoomSlider;
        private ScaleTransform scaleTransform;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            zoomThumb = Template.FindName("PART_ZoomThumb", this) as Thumb;
            if(zoomThumb == null)
                throw new Exception("PART_ZoomThumb template is missing!");

            zoomCanvas = Template.FindName("PART_ZoomCanvas", this) as Canvas;
            if(zoomCanvas == null)
                throw new Exception("PART_ZoomCanvas template is missing!");

            zoomSlider = Template.FindName("PART_ZoomSlider", this) as Slider;
            if(zoomSlider == null)
                throw new Exception("PART_ZoomSlider template is missing!");

            DesignerArea.LayoutUpdated += new EventHandler(DesignerArea_LayoutUpdated);

            zoomThumb.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);

            zoomSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ZoomSlider_ValueChanged);

            scaleTransform = new ScaleTransform();
            DesignerArea.LayoutTransform = scaleTransform;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double scale = e.NewValue / e.OldValue;

            double halfViewportHeight = viewModel.DesignerScroller.ViewportHeight / 2;
            double newVerticalOffset = ((viewModel.DesignerScroller.VerticalOffset + halfViewportHeight) * scale - halfViewportHeight);

            double halfViewportWidth = viewModel.DesignerScroller.ViewportWidth / 2;
            double newHorizontalOffset = ((viewModel.DesignerScroller.HorizontalOffset + halfViewportWidth) * scale - halfViewportWidth);

            scaleTransform.ScaleX *= scale;
            scaleTransform.ScaleY *= scale;

            viewModel.DesignerScroller.ScrollToHorizontalOffset(newHorizontalOffset);
            viewModel.DesignerScroller.ScrollToVerticalOffset(newVerticalOffset);

        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            InvalidateScale(out double scale, out double xOffset, out double yOffset);

            viewModel.DesignerScroller.ScrollToHorizontalOffset(viewModel.DesignerScroller.HorizontalOffset + e.HorizontalChange / scale);
            viewModel.DesignerScroller.ScrollToVerticalOffset(viewModel.DesignerScroller.VerticalOffset + e.VerticalChange / scale);
        }


        private void DesignerArea_LayoutUpdated(object sender, EventArgs e)
        {
            double viewWidth, viewHeight;
            InvalidateScale(out double scale, out double xOffset, out double yOffset);

            if(viewModel.DesignerScroller.ComputedHorizontalScrollBarVisibility != Visibility.Visible)
                viewWidth = DesignerArea.ActualWidth;
            else
                viewWidth = viewModel.DesignerScroller.ViewportWidth;

            if(viewModel.DesignerScroller.ComputedVerticalScrollBarVisibility != Visibility.Visible)
                viewHeight = DesignerArea.ActualHeight;
            else
                viewHeight = viewModel.DesignerScroller.ViewportHeight;

            zoomThumb.Width = viewWidth * scale;
            zoomThumb.Height = viewHeight * scale;

            // TODO : Wenn zoomSlider.Value <= 100 muss auch der inhalt der Zoombox schrumpfen. Zudem Beachten: Nach vergößern/verkleinern der ZoomBox muss der Rahmen auch noch stimmen

            // System.Diagnostics.Debug.WriteLine("Zoomthumb: " + zoomThumb.Width.ToString("000000.000") + " / " + zoomThumb.Height.ToString("000000.000"));

            Canvas.SetLeft(zoomThumb, xOffset + viewModel.DesignerScroller.HorizontalOffset * scale);
            Canvas.SetTop(zoomThumb, yOffset + viewModel.DesignerScroller.VerticalOffset * scale);
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            // DesignerArea size
            double w = DesignerArea.ActualWidth * scaleTransform.ScaleX;
            double h = DesignerArea.ActualHeight * scaleTransform.ScaleY;

            // zoom canvas size
            double x = zoomCanvas.ActualWidth;
            double y = zoomCanvas.ActualHeight;

            double scaleX = x / w;
            double scaleY = y / h;

            scale = (scaleX < scaleY) ? scaleX : scaleY;

            xOffset = (x - scale * w) / 2;
            yOffset = (y - scale * h) / 2;
        }
    }
}
