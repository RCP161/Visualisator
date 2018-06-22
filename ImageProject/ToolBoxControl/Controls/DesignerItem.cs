using System;
using System.Collections.Generic;
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
        private static Style _designerItemStyle;
        private Style DesignerItemStyle
        {
            get
            {
                if (_designerItemStyle == null)
                {
                    this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/ToolBoxControl;component/Styles/DesignerItemStyle.xaml", UriKind.RelativeOrAbsolute) });
                    _designerItemStyle = FindResource("DesignerItemStyle") as Style;
                }
                return _designerItemStyle;
            }
        }

        public DesignerItem() : base()
        {
            Style = DesignerItemStyle;
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
