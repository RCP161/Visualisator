using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ToolBoxControl.ViewModels
{
    internal class DesignerCanvasVm : BaseViewModel
    {
        public bool _isVisibleInDesigner;
        public bool IsVisibleInDesigner
        {
            get { return _isVisibleInDesigner; }
            set { SetField(ref _isVisibleInDesigner, value); }
        }

        public Brush _backGround;
        public Brush BackGround
        {
            get { return _backGround; }
            set { SetField(ref _backGround, value); }
        }
    }
}
