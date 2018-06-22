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
    internal class MoveThumb : Thumb
    {
        private DesignerItem designerItem;
        private double offsetX;
        private double offsetY;

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as DesignerItem;

            if (this.designerItem != null)
            {
                var pos = Mouse.GetPosition(designerItem);
                offsetX = pos.X;
                offsetY = pos.Y;
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null && this.designerItem.IsSelected && designerItem.DesignerCanvas != null)
            {
                // Aktuelle Position
                double x = Mouse.GetPosition(designerItem.DesignerCanvas).X - offsetX;
                double y = Mouse.GetPosition(designerItem.DesignerCanvas).Y - offsetY;

                // Dockverhalten
                if(designerItem.DesignerCanvas.DesignerControl.ItemCanDock)
                {
                    double dixd, diyd;
                    double? dix = null;
                    double? diy = null;

                    var items = designerItem.DesignerCanvas.Children.Cast<DesignerItem>();
                    
                    foreach (DesignerItem i in items)
                    {
                        if (i.IsSelected)
                            continue;

                        dixd = Math.Abs(i.Position.X - x);
                        diyd = Math.Abs(i.Position.Y - y);

                        if ((!dix.HasValue || dix > dixd) && dixd <= Designer.ItemDockRange)
                            dix = i.Position.X - x;

                        if((!diy.HasValue || diy > diyd) && diyd <= Designer.ItemDockRange)
                            diy = i.Position.Y - y;
                    }

                    if (dix.HasValue)
                        x = x + dix.Value;

                    if (diy.HasValue)
                        y = y + diy.Value;
                }


                // Delta
                x = x - designerItem.Position.X;
                y = y - designerItem.Position.Y;

               
                if (!designerItem.DesignerCanvas.DesignerControl.ItemCanLeaveDesigner)
                {
                    double xMin = designerItem.DesignerCanvas.SelectedItems.Min(i => i.Position.X);
                    double yMin = designerItem.DesignerCanvas.SelectedItems.Min(i => i.Position.Y);

                    if (x < 0 && xMin - x < 0)
                        x = xMin;

                    if (y < 0 && yMin - y < 0)
                        y = yMin;

                    if(!designerItem.DesignerCanvas.DesignerControl.DesignerCanResize)
                    {
                        double xMax = designerItem.DesignerCanvas.SelectedItems.Max(i => i.Position.X) + designerItem.ActualWidth;
                        double yMax = designerItem.DesignerCanvas.SelectedItems.Max(i => i.Position.Y) + designerItem.ActualHeight;

                        if (x > 0 && xMax + x > designerItem.DesignerCanvas.DesignerControl.DesignerWidth)
                            x = xMax;

                        if (y > 0 && yMin - y > designerItem.DesignerCanvas.DesignerControl.DesignerWidth)
                            y = yMin;
                    }
                }

                //System.Diagnostics.Debug.WriteLine("Pos: " + x.ToString("000000.000") + " / " + y.ToString("000000.000"));

                foreach (DesignerItem item in this.designerItem.DesignerCanvas.SelectedItems)
                {
                    item.Position = new Point(item.Position.X + x, item.Position.Y + y);
                    Canvas.SetLeft(item, item.Position.X);
                    Canvas.SetTop(item, item.Position.Y);
                }

                designerItem.DesignerCanvas.InvalidateMeasure();
                e.Handled = true;
            }
        }
    }
}