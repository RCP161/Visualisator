using System;
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

namespace ToolBoxControl.Controls
{
    /// <summary>
    /// Interaktionslogik für DesignerItem.xaml
    /// </summary>
    internal partial class DesignerItem : ContentControl
    {
        public DesignerItem()
        {
            DataContext = this;
            
            InitializeComponent();

            MinWidth = 50;
            MinHeight = 50;
            SnapsToDevicePixels = true;
            RenderTransformOrigin = new Point(0.5,0.5);
        }

        public object ControlContent
        {
            get { return (bool)GetValue(ControlContentProperty); }
            set { SetValue(ControlContentProperty, value); }
        }

        public static readonly DependencyProperty ControlContentProperty = DependencyProperty.Register("ControlContent", typeof(object), typeof(DesignerItem), new FrameworkPropertyMetadata(false));


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
                    this.IsSelected = !this.IsSelected;
                }
                else
                {
                    if (!this.IsSelected)
                    {
                        DesignerCanvas.DeselectAll();
                        this.IsSelected = true;
                    }
                }
            }

            e.Handled = false;
        }
    }
}
