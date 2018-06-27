using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

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
        private ItemsControl designerArea;

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
        
        public ScrollViewer DesignerScroller { get; set; }

        #endregion

        #region Methods

        private void AddNewLevel()
        {
            DesignerCanvas desgnCanv = new DesignerCanvas
            {
                DesignerControl = this
            };

            if(Planes.Count < 1)
                desgnCanv.Background = BackgroundColor;

            Binding widthBnd = new Binding
            {
                Source = this,
                Path = new PropertyPath("DesignerWidth"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(desgnCanv, DesignerCanvas.WidthProperty, widthBnd);

            Binding heighthBnd = new Binding
            {
                Source = this,
                Path = new PropertyPath("DesignerHeight"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(desgnCanv, DesignerCanvas.HeightProperty, heighthBnd);

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

            Dialogs.ZoomDialog zd = new Dialogs.ZoomDialog
            {
                ScrollViewer = designerScroller,
                DesignerArea = designerArea
            };

            zd.Show();

            zoomDialog = zd;
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

        // TODO : Das muss vom Viewmodel dann getriggert werden
        internal void RefreshAktivPlaneStates()
        {
            OnPropertyChanged("AktivPlanes");
        }

        #endregion
    }
}
