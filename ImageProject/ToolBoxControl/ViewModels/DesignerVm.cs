using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ToolBoxControl.Controls;

namespace ToolBoxControl.ViewModels
{
    internal class DesignerVm : BaseViewModel
    {
        internal DesignerVm()
        { 
            Planes = new ObservableCollection<DesignerCanvas>();
        }

        #region private fields

        private Dialogs.ZoomDialog zoomDialog;
        private Dialogs.PlaneDialog planeDialog;

        #endregion

        #region Properties

        public bool _isZoomDialogActiv;
        public bool IsZoomDialogActiv
        {
            get { return _isZoomDialogActiv; }
            set { SetField(ref _isZoomDialogActiv, value); }
        }

        public bool _isPlaneDialogActiv;
        public bool IsPlaneDialogActiv
        {
            get { return _isPlaneDialogActiv; }
            set { SetField(ref _isPlaneDialogActiv, value); }
        }

        public double _designerWidth;
        public double DesignerWidth
        {
            get { return _designerWidth; }
            set { SetField(ref _designerWidth, value); }
        }
        
        public double _designerHeight;
        public double DesignerHeight
        {
            get { return _designerHeight; }
            set { SetField(ref _designerHeight, value); }
        }

        public IList<DesignerCanvas> _planes;
        public IList<DesignerCanvas> Planes
        {
            get { return _planes; }
            set { SetField(ref _planes, value); }
        }

        public DesignerCanvas _selectedPlane;
        public DesignerCanvas SelectedPlane
        {
            get { return _selectedPlane; }
            set { SetField(ref _selectedPlane, value); }
        }
        
        public IList<DesignerCanvas> AktivPlanes
        {
            get
            {
                return Planes.Where(p => p.IsVisibleInDesigner).ToList();
            }
        }

        // Hier muss leider die Ausnahme sein
        public ScrollViewer DesignerScroller { get; set; }
        public ItemsControl DesignerArea { get; set; }
        public Designer DesignerControl { get; set; }

        #endregion

        #region Methods

        internal void AfterStartUp()
        {
            AddPlane();
            ShowPlaneDialog();
            ShowZoomBoxDialog();
        }

        private  void AddPlane()
        {
            DesignerCanvas desgnCanv = new DesignerCanvas();
            desgnCanv.DesignerControl = DesignerControl;

            if(Planes.Count < 1)
                desgnCanv.Background = Brushes.White;

            Planes.Add(desgnCanv);
            RefreshAktivPlaneStates();
        }

        private void ShowZoomBoxDialog()
        {
            if(zoomDialog != null)
            {
                zoomDialog.Activate();
                return;
            }

            zoomDialog = new Dialogs.ZoomDialog(this);
            zoomDialog.Show();
        }

        private void ShowPlaneDialog()
        {
            if(planeDialog != null)
            {
                planeDialog.Activate();
                return;
            }

            Dialogs.PlaneDialog pd = new Dialogs.PlaneDialog
            {
                DataContext = this
            };

            pd.Show();

            planeDialog = pd;
        }

        internal void RefreshAktivPlaneStates()
        {
            OnPropertyChanged("AktivPlanes");
        }

        #endregion
    }
}
