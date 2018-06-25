using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ToolBoxControl.Controls
{
    internal class RotateThumb : Thumb
    {
        private Vector startVector;
        private Point centerPoint;
        private DesignerItem designerItem;

        public RotateThumb()
        {
            DragDelta += new DragDeltaEventHandler( RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler( RotateThumb_DragStarted);
            DragCompleted += new DragCompletedEventHandler( RotateThumb_DragCompleted);
        }


        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
             designerItem = DataContext as DesignerItem;

            if ( designerItem != null)
            {
                 centerPoint =  designerItem.TranslatePoint(
                    new Point( designerItem.Width *  designerItem.RenderTransformOrigin.X,
                               designerItem.Height *  designerItem.RenderTransformOrigin.Y),
                               designerItem.DesignerCanvas);

                Point startPoint = Mouse.GetPosition( designerItem.DesignerCanvas);
                 startVector = Point.Subtract(startPoint,  centerPoint);
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if ( designerItem != null)
            {
                Point currentPoint = Mouse.GetPosition( designerItem.DesignerCanvas);
                Vector deltaVector = Point.Subtract(currentPoint,  centerPoint);

                double angle = Vector.AngleBetween( startVector, deltaVector);

                foreach (DesignerItem item in  designerItem.DesignerCanvas.SelectedItems)
                {
                    if(!(item.RenderTransform is RotateTransform rotateTransform))
                    {
                        item.Angle = 0;
                        item.RenderTransform = new RotateTransform(0);
                        rotateTransform = item.RenderTransform as RotateTransform;
                    }

                    rotateTransform.Angle = item.Angle + Math.Round(angle, 0);
                    item.InvalidateMeasure();
                }
            }
        }
        private void RotateThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if ( designerItem != null)
            {
                foreach (DesignerItem item in  designerItem.DesignerCanvas.SelectedItems)
                {
                    RotateTransform rotateTransform = item.RenderTransform as RotateTransform;
                    item.Angle = rotateTransform.Angle;
                }
            }
        }
    }
}
