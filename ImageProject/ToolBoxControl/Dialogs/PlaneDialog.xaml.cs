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
using ToolBoxControl.Controls;
using ToolBoxControl.ViewModels;

namespace ToolBoxControl.Dialogs
{
    /// <summary>
    /// Interaktionslogik für ZoomDialog.xaml
    /// </summary>
    internal partial class PlaneDialog : Window
    {
        public PlaneDialog(DesignerVm vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
