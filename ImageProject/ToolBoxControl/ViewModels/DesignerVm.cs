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

            AddPlaneCommand = new RelayCommand(p => true, p => AddPlane());
            DeletePlaneCommand = new RelayCommand(p => true, p => DeletePlane());
            PlaneUpCommand = new RelayCommand(p => true, p => PutActivPlaneUp());
            PlaneDownCommand = new RelayCommand(p => true, p => PutActivPlaneDown());
        }


        // TODO Liste
        // Interface für Spezial Docking (DockingPunkte)
        // Docking Linien Adorner einblenden bei Dockverhalten
        // Raster an DesignerCanvas über Docking
        // KontextMenü Item nach vorne / Hinten
        // Mehrsprachenfähigkeit
        // Ebenen ToFront und ToBack?


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

                if(value != null)
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

        public ICommand AddPlaneCommand { get; set; }
        public ICommand DeletePlaneCommand { get; set; }
        public ICommand PlaneUpCommand { get; set; }
        public ICommand PlaneDownCommand { get; set; }

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
            DesignerCanvas desgnCanv = new DesignerCanvas
            {
                DesignerControl = DesignerControl,
                IsHitTestVisible = false,
                PlaneName = "Name"
            };

            if(Planes.Count < 1)
                desgnCanv.Background = Brushes.White;
            else
                desgnCanv.Background = Brushes.Transparent;

            if(ActivPlane == null)
                Planes.Add(desgnCanv);
            else
                Planes.Insert(Planes.IndexOf(ActivPlane) + 1, desgnCanv);

            ActivPlane = desgnCanv;

            RefreshPlaneStates();
        }

        private void DeletePlane()
        {
            if(ActivPlane == null)
                return;

            DesignerCanvas dc = ActivPlane;
            ActivPlane = VisiblePlanes.Where(x => x != ActivPlane).LastOrDefault();
            Planes.Remove(dc);

            RefreshPlaneStates();
        }

        private void PutActivPlaneDown()
        {
            if(ActivPlane == null || Planes.Count < 2)
                return;

            int index = Planes.IndexOf(ActivPlane);

            if(index < 1)
                return;

            Planes.RemoveAt(index);
            Planes.Insert(index - 1, ActivPlane);

            RefreshPlaneStates();
        }

        private void PutActivPlaneUp()
        {
            if(ActivPlane == null || Planes.Count < 2)
                return;

            int index = Planes.IndexOf(ActivPlane);

            if(index >= Planes.IndexOf(Planes.Last()))
                return;

            Planes.RemoveAt(index);
            Planes.Insert(index + 1, ActivPlane);

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
