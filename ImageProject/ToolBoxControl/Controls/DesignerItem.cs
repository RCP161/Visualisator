﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToolBoxControl.Controls
{
    internal class DesignerItem : ContentControl
    {
        static DesignerItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(DesignerItem), new FrameworkPropertyMetadata(false));

        public DesignerCanvas DesignerCanvas
        {
            get { return (DesignerCanvas)GetValue(DesignerCanvasProperty); }
            set { SetValue(DesignerCanvasProperty, value); }
        }

        public static readonly DependencyProperty DesignerCanvasProperty = DependencyProperty.Register("DesignerCanvas", typeof(DesignerCanvas), typeof(DesignerItem), new FrameworkPropertyMetadata(null));


        public Point Position { get; set; }
        public double Angle { get; set; }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (DesignerCanvas != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                     IsSelected = ! IsSelected;
                }
                else
                {
                    if (! IsSelected)
                    {
                        DesignerCanvas.DeselectAll();
                         IsSelected = true;
                    }
                }
            }

            e.Handled = false;
        }
    }
}
