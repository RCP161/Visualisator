﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToolBoxControl.Controls
{
    internal class ToolBoxItem : ContentControl
    {
        static ToolBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBoxItem), new FrameworkPropertyMetadata(typeof(ToolBoxItem)));
        }

        private Point? dragStartPoint = null;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            if (this.dragStartPoint.HasValue)
            {
                Point position = e.GetPosition(this);
                if ((SystemParameters.MinimumHorizontalDragDistance <= Math.Abs((double)(position.X - this.dragStartPoint.Value.X))) ||
                    (SystemParameters.MinimumVerticalDragDistance <= Math.Abs((double)(position.Y - this.dragStartPoint.Value.Y))))
                {
                    string xamlString = System.Windows.Markup.XamlWriter.Save(this.Content);
                    DataObject dataObject = new DataObject("DESIGNER_ITEM", xamlString);

                    if (dataObject != null)
                        DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
                }

                e.Handled = true;
            }
        }
    }
}
