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
        static ResizeRotateControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeRotateControl), new FrameworkPropertyMetadata(typeof(ResizeRotateControl)));
        }
    }
}
