using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ToolBoxControl.Controls
{
    internal class RubberbandAdorner : Adorner
    {
        private Point? startPoint, endPoint;
        private Rectangle rubberband;
        private DesignerCanvas designerCanvas;
        private VisualCollection visuals;
        private Canvas adornerCanvas;

        protected override int VisualChildrenCount
        {
            get
            {
                return  visuals.Count;
            }
        }

        public RubberbandAdorner(DesignerCanvas designerCanvas, Point? dragStartPoint) : base(designerCanvas)
        {
             this.designerCanvas = designerCanvas;
             startPoint = dragStartPoint;

            adornerCanvas = new Canvas
            {
                Background = Brushes.Transparent
            };

            visuals = new VisualCollection(this)
            {
                adornerCanvas
            };

            rubberband = new Rectangle
            {
                Stroke = Brushes.Navy,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection(new double[] { 2 })
            };

            adornerCanvas.Children.Add( rubberband);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (! IsMouseCaptured)
                {
                     CaptureMouse();
                }

                 endPoint = e.GetPosition(this);
                 UpdateRubberband();
                 UpdateSelection();
                e.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if ( IsMouseCaptured)
            {
                 ReleaseMouseCapture();
            }

            if(Parent is AdornerLayer adornerLayer)
            {
                adornerLayer.Remove(this);
            }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
             adornerCanvas.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            return  visuals[index];
        }

        private void UpdateRubberband()
        {
            double left = Math.Min( startPoint.Value.X,  endPoint.Value.X);
            double top = Math.Min( startPoint.Value.Y,  endPoint.Value.Y);

            double width = Math.Abs( startPoint.Value.X -  endPoint.Value.X);
            double height = Math.Abs( startPoint.Value.Y -  endPoint.Value.Y);

             rubberband.Width = width;
             rubberband.Height = height;
            Canvas.SetLeft( rubberband, left);
            Canvas.SetTop( rubberband, top);
        }

        private void UpdateSelection()
        {
            Rect rubberBand = new Rect( startPoint.Value,  endPoint.Value);
            foreach (DesignerItem item in  designerCanvas.Children)
            {
                Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                Rect itemBounds = item.TransformToAncestor(designerCanvas).TransformBounds(itemRect);

                if (rubberBand.Contains(itemBounds))
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }
    }
}