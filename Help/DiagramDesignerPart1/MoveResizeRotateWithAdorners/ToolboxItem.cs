using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DiagramDesigner
{
    public class ToolboxItem : ContentControl
    {
        public ToolboxItem()
        {
            try
            {
                this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MoveResizeRotateWithAdorners;component/Resources/DesignerItem.xaml", UriKind.RelativeOrAbsolute) });
                Style = FindResource("DesignerItemStyle") as Style;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
