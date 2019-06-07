using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DiagramDesigner
{
    public class Designer : Canvas
    {
        public Designer() : base()
        {
            ClipToBounds = true;
            SnapsToDevicePixels = true;
        }
    }
}
