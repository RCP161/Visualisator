using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ToolBoxControl
{
    public class ToolBoxControl : ItemsControl
    {
        private static Style _toolboxControlStyle;
        private Style ToolBoxControlStyle
        {
            get
            {
                if (_toolboxControlStyle == null)
                {
                    Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/ToolBoxControl;component/Styles/ToolBoxControlStyle.xaml", UriKind.RelativeOrAbsolute) });
                    _toolboxControlStyle = FindResource("ToolBoxControlStyle") as Style;
                }
                return _toolboxControlStyle;
            }
        }

        public ToolBoxControl() : base()
        {
            Style = ToolBoxControlStyle;
        }

        private Size defaultItemSize = new Size(65, 65);
        public Size DefaultItemSize
        {
            get { return this.defaultItemSize; }
            set { this.defaultItemSize = value; }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Controls.ToolBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is Controls.ToolBoxItem);
        }
    }
}
