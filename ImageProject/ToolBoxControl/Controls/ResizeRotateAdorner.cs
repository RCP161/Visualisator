using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ToolBoxControl.Controls
{
    internal class ResizeRotateAdorner : Adorner
    {
        private VisualCollection visuals;
        private ResizeRotateControl chrome;

        protected override int VisualChildrenCount
        {
            get
            {
                return  visuals.Count;
            }
        }

        public ResizeRotateAdorner(DesignerItem designerItem) : base(designerItem)
        {
            SnapsToDevicePixels = true;

            chrome = new ResizeRotateControl
            {
                DataContext = designerItem
            };

            visuals = new VisualCollection(this)
            {
                chrome
            };
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
             chrome.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            return  visuals[index];
        }
    }
}
