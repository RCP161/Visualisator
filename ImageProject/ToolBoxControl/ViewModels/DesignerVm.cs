using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ToolBoxControl.Controls;

namespace ToolBoxControl.ViewModels
{
    internal class DesignerVm : BaseViewModel
    {
        internal DesignerVm()
        { 
            Planes = new ObservableCollection<DesignerCanvas>();

            RefreshActivPlaneCommand = new RelayCommand(p => true, p => RefreshPlaneStates());
        }

        #region private fields

        private Dialogs.ZoomDialog zoomDialog;
        private Dialogs.PlaneDialog planeDialog;

        #endregion

        #region Properties

        public double _designerWidth = DefaultValues.DesignerDefaultWidth;
        public double DesignerWidth
        {
            get { return _designerWidth; }
            set { SetField(ref _designerWidth, value); }
        }
        
        public double _designerHeight = DefaultValues.DesignerDefaultHeight;
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

        public DesignerCanvas ActivPlane
        {
            get { return Planes.SingleOrDefault(x => x.IsHitTestVisible); }
            set
            {
                if(value == ActivPlane)
                    return;

                IEnumerable<DesignerCanvas> list = Planes.Where(x => x.IsHitTestVisible).ToList();
                foreach(DesignerCanvas dc in list)
                {
                    dc.DeselectAll();
                    dc.IsHitTestVisible = false;
                }

                value.IsHitTestVisible = true;
                OnPropertyChanged(nameof(ActivPlane));
            }
        }

        public IList<DesignerCanvas> VisiblePlanes
        {
            get
            {
                return Planes.Where(p => p.IsVisibleInDesigner).ToList();
            }
        }

        public ICommand RefreshActivPlaneCommand { get; set; }

        // Hier muss leider die Ausnahme sein
        public ScrollViewer DesignerScroller { get; set; }
        public ItemsControl DesignerArea { get; set; }
        public Designer DesignerControl { get; set; }

        #endregion

        #region Methods

        internal void AfterStartUp()
        {
            AddPlane();
            AddPlane();
            ShowPlaneDialog();
            ShowZoomBoxDialog();
        }

        private  void AddPlane()
        {
            DesignerCanvas desgnCanv = new DesignerCanvas
            {
                DesignerControl = DesignerControl,
                IsHitTestVisible = false
            };

            if(Planes.Count < 1)
                desgnCanv.Background = Brushes.White;
            else
                desgnCanv.Background = Brushes.Transparent;

            Planes.Add(desgnCanv);
            ActivPlane = desgnCanv;

            RefreshPlaneStates();
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

            planeDialog = new Dialogs.PlaneDialog(this);
            planeDialog.Show();
        }

        internal void OnClose()
        {
            if(zoomDialog != null)
                zoomDialog.Close();

            if(planeDialog != null)
                planeDialog.Close();
        }

        internal void RefreshPlaneStates()
        {
            OnPropertyChanged(nameof(VisiblePlanes));
        }

        #endregion
    }
}
