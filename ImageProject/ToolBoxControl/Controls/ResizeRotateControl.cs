using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ToolBoxControl.Controls
{
    internal class ResizeRotateControl : Control
    {
        private static Style _resizeRotateStyle;
        private Style ResizeRotateStyle
        {
            get
            {
                if (_resizeRotateStyle == null)
                {
                    Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/ToolBoxControl;component/Styles/ResizeRotateControlStyle.xaml", UriKind.RelativeOrAbsolute) });
                    _resizeRotateStyle = FindResource("ResizeRotateControlStyle") as Style;
                }
                return _resizeRotateStyle;
            }
        }

        public ResizeRotateControl()
        {
            Style = ResizeRotateStyle;
        }
    }
}
