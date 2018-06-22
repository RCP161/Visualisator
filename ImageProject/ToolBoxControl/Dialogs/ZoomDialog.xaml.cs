﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolBoxControl.Controls;

namespace ToolBoxControl.Dialogs
{
    /// <summary>
    /// Interaktionslogik für ZoomDialog.xaml
    /// </summary>
    internal partial class ZoomDialog : Window
    {
        public ZoomDialog()
        {
            DataContext = this;
            InitializeComponent();
        }

        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(ZoomDialog), new FrameworkPropertyMetadata(null));

        public DesignerCanvas DesignerCanvas
        {
            get { return (DesignerCanvas)GetValue(DesignerCanvasProperty); }
            set { SetValue(DesignerCanvasProperty, value); }
        }

        public static readonly DependencyProperty DesignerCanvasProperty = DependencyProperty.Register("DesignerCanvas", typeof(DesignerCanvas), typeof(ZoomDialog), new FrameworkPropertyMetadata(null));

        public Designer Designer
        {
            get { return (Designer)GetValue(DesignerProperty); }
            set { SetValue(DesignerProperty, value); }
        }

        public static readonly DependencyProperty DesignerProperty = DependencyProperty.Register("Designer", typeof(Designer), typeof(ZoomDialog), new FrameworkPropertyMetadata(null));

    }
}
