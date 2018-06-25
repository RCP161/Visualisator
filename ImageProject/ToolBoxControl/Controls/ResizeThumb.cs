using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace ToolBoxControl.Controls
{
    internal class ResizeThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private double angle;
        private Adorner adorner;
        private Point transformOrigin;
        private ContentControl designerItem;
        private Canvas canvas;

        public ResizeThumb()
        {
            DragStarted += new DragStartedEventHandler( ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler( ResizeThumb_DragDelta);
            DragCompleted += new DragCompletedEventHandler( ResizeThumb_DragCompleted);
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
             designerItem =  DataContext as ContentControl;

            if ( designerItem != null)
            {
                 canvas = VisualTreeHelper.GetParent( designerItem) as Canvas;

                if ( canvas != null)
                {
                     transformOrigin =  designerItem.RenderTransformOrigin;

                     rotateTransform =  designerItem.RenderTransform as RotateTransform;
                    if ( rotateTransform != null)
                    {
                         angle =  rotateTransform.Angle * Math.PI / 180.0;
                    }
                    else
                    {
                         angle = 0.0d;
                    }
                }
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if ( designerItem != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange,  designerItem.ActualHeight -  designerItem.MinHeight);
                        Canvas.SetTop( designerItem, Canvas.GetTop( designerItem) + ( transformOrigin.Y * deltaVertical * (1 - Math.Cos(- angle))));
                        Canvas.SetLeft( designerItem, Canvas.GetLeft( designerItem) - deltaVertical *  transformOrigin.Y * Math.Sin(- angle));
                         designerItem.Height -= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange,  designerItem.ActualHeight -  designerItem.MinHeight);
                        Canvas.SetTop( designerItem, Canvas.GetTop( designerItem) + deltaVertical * Math.Cos(- angle) + ( transformOrigin.Y * deltaVertical * (1 - Math.Cos(- angle))));
                        Canvas.SetLeft( designerItem, Canvas.GetLeft( designerItem) + deltaVertical * Math.Sin(- angle) - ( transformOrigin.Y * deltaVertical * Math.Sin(- angle)));
                         designerItem.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange,  designerItem.ActualWidth -  designerItem.MinWidth);
                        Canvas.SetTop( designerItem, Canvas.GetTop( designerItem) + deltaHorizontal * Math.Sin( angle) -  transformOrigin.X * deltaHorizontal * Math.Sin( angle));
                        Canvas.SetLeft( designerItem, Canvas.GetLeft( designerItem) + deltaHorizontal * Math.Cos( angle) + ( transformOrigin.X * deltaHorizontal * (1 - Math.Cos( angle))));
                         designerItem.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,  designerItem.ActualWidth -  designerItem.MinWidth);
                        Canvas.SetTop( designerItem, Canvas.GetTop( designerItem) -  transformOrigin.X * deltaHorizontal * Math.Sin( angle));
                        Canvas.SetLeft( designerItem, Canvas.GetLeft( designerItem) + (deltaHorizontal *  transformOrigin.X * (1 - Math.Cos( angle))));
                         designerItem.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if ( adorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer( canvas);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove( adorner);
                }

                 adorner = null;
            }
        }
    }
}
