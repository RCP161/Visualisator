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
        static ToolBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBoxControl), new FrameworkPropertyMetadata(typeof(ToolBoxControl)));
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
