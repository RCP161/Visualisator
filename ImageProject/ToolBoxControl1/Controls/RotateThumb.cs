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
            DragDelta += new DragDeltaEventHandler(this.RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(this.RotateThumb_DragStarted);
            DragCompleted += new DragCompletedEventHandler(this.RotateThumb_DragCompleted);
        }


        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as DesignerItem;

            if (this.designerItem != null)
            {
                this.centerPoint = this.designerItem.TranslatePoint(
                    new Point(this.designerItem.Width * this.designerItem.RenderTransformOrigin.X,
                              this.designerItem.Height * this.designerItem.RenderTransformOrigin.Y),
                              this.designerItem.DesignerCanvas);

                Point startPoint = Mouse.GetPosition(this.designerItem.DesignerCanvas);
                this.startVector = Point.Subtract(startPoint, this.centerPoint);
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                Point currentPoint = Mouse.GetPosition(this.designerItem.DesignerCanvas);
                Vector deltaVector = Point.Subtract(currentPoint, this.centerPoint);

                double angle = Vector.AngleBetween(this.startVector, deltaVector);

                foreach (DesignerItem item in this.designerItem.DesignerCanvas.SelectedItems)
                {
                    RotateTransform rotateTransform = item.RenderTransform as RotateTransform;
                    if (rotateTransform == null)
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
            if (this.designerItem != null)
            {
                foreach (DesignerItem item in this.designerItem.DesignerCanvas.SelectedItems)
                {
                    RotateTransform rotateTransform = item.RenderTransform as RotateTransform;
                    item.Angle = rotateTransform.Angle;
                }
            }
        }
    }
}
