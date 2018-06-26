using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using ToolBoxControl.Controls;

namespace ToolBoxControl
{
    public class Designer : ContentControl
    {
        public static readonly int DesignerDefaultWidth = 800;
        public static readonly int DesignerDefaultHeight = 600;
        public static readonly int ItemDefaultWidth = 65;
        public static readonly int ItemDefaultHeight = 65;
        public static readonly int ItemDockRange = 8;

        static Designer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Designer), new FrameworkPropertyMetadata(typeof(Designer)));
        }

        public Designer() : base()
        {
            DataContext = this;

            Planes = new ObservableCollection<DesignerCanvas>();

            Loaded += Designer_Loaded;
            Unloaded += Designer_Unloaded;
        }

        // TODO Liste
        // Eigene Property für Imagebackground (Derzeit nur Background)
        // Interface für Spezial Docking (DockingPunkte)
        // Docking Linien Adorner einblenden bei Dockverhalten
        // DesignerCollection für Ebenen
        // Raster an DesignerCanvas über Docking
        // Dialog zum hinzufügen/entfernen/verschieben von Ebenen
        // KontextMenü Item nach vorne / Hinten
        // Mehrsprachenfähigkeit

        // TODO : Prüfen ob ich den DataContext der Dialoge auf den Designer setzen kann und somit direkt daran binden

        #region WPF Properties

        // TODO : Standard ändern auf false
        /// <summary>
        /// Gibt an, ob der ZoomDialog sichtbar ist
        /// </summary>
        public bool IsZoomDialogActiv
        {
            get { return (bool)GetValue(IsZoomDialogActivProperty); }
            set { SetValue(IsZoomDialogActivProperty, value); }
        }

        public static readonly DependencyProperty IsZoomDialogActivProperty = DependencyProperty.Register("IsZoomDialogActiv", typeof(bool), typeof(Designer), new FrameworkPropertyMetadata(true));


        // TODO : Standard ändern auf false
        /// <summary>
        /// Gibt an, ob der EbenenDialog sichtbar ist
        /// </summary>
        public bool IsPlaneDialogActiv
        {
            get { return (bool)GetValue(IsPlaneDialogActivProperty); }
            set { SetValue(IsPlaneDialogActivProperty, value); }
        }


        public static readonly DependencyProperty IsPlaneDialogActivProperty = DependencyProperty.Register("IsPlaneDialogActiv", typeof(bool), typeof(Designer), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Gibt an, ob sich der Designer selber vergrößern darf
        /// </summary>
        public bool DesignerCanResize
        {
            get { return (bool)GetValue(DesignerCanResizeProperty); }
            set { SetValue(DesignerCanResizeProperty, value); }
        }

        public static readonly DependencyProperty DesignerCanResizeProperty = DependencyProperty.Register("DesignerCanResize", typeof(bool), typeof(Designer), new FrameworkPropertyMetadata(false));


        /// <summary>
        /// Breite des Designers, wenn IsResizable == false ist
        /// </summary>
        public int DesignerWidth
        {
            get { return (int)GetValue(DesignerWidthProperty); }
            set { SetValue(DesignerWidthProperty, value); }
        }

        public static readonly DependencyProperty DesignerWidthProperty = DependencyProperty.Register("DesignerWidth", typeof(int), typeof(Designer), new FrameworkPropertyMetadata(DesignerDefaultWidth));


        /// <summary>
        /// Höhe des Designers, wenn IsResizable == false ist
        /// </summary>
        public int DesignerHeight
        {
            get { return (int)GetValue(DesignerHeightProperty); }
            set { SetValue(DesignerHeightProperty, value); }
        }

        public static readonly DependencyProperty DesignerHeightProperty = DependencyProperty.Register("DesignerHeight", typeof(int), typeof(Designer), new FrameworkPropertyMetadata(DesignerDefaultHeight));


        /// <summary>
        /// Gibt die Hintergrindfarbe der ersten Ebene an
        /// </summary>
        public System.Windows.Media.Brush BackgroundColor
        {
            get { return (System.Windows.Media.Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(System.Windows.Media.Brush), typeof(Designer), new FrameworkPropertyMetadata(System.Windows.Media.Brushes.White));


        /// <summary>
        /// Gibt an, ob die Items die Bearbeitungsfläche verlassen werden können
        /// </summary>
        public bool ItemCanLeaveDesigner
        {
            get { return (bool)GetValue(ItemCanLeaveDesignerProperty); }
            set { SetValue(ItemCanLeaveDesignerProperty, value); }
        }

        public static readonly DependencyProperty ItemCanLeaveDesignerProperty = DependencyProperty.Register("ItemCanLeaveDesigner", typeof(bool), typeof(Designer), new FrameworkPropertyMetadata(true));


        /// <summary>
        /// Gibt an, ob die Items ein Dockverhalten aufweißen
        /// </summary>
        public bool ItemCanDock
        {
            get { return (bool)GetValue(ItemCanDockProperty); }
            set { SetValue(ItemCanDockProperty, value); }
        }

        public static readonly DependencyProperty ItemCanDockProperty = DependencyProperty.Register("ItemCanDock", typeof(bool), typeof(Designer), new FrameworkPropertyMetadata(true));


        /// <summary>
        /// Gibt an, wie die Items im Designer bearbeitet werden können
        /// </summary>
        public ItemEditMode ItemEditMode
        {
            get { return (ItemEditMode)GetValue(ItemEditModeProperty); }
            set { SetValue(ItemEditModeProperty, value); }
        }

        public static readonly DependencyProperty ItemEditModeProperty = DependencyProperty.Register("ItemEditMode", typeof(ItemEditMode), typeof(Designer), new FrameworkPropertyMetadata(ItemEditMode.All));

        #endregion

        #region Properties

        internal Dialogs.ZoomDialog ZoomDialog { get; set; }
        internal Dialogs.PlaneDialog PlaneDialog { get; set; }
        internal Grid DesignerArea { get; set; }
        internal ScrollViewer DesignerScroller { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<DesignerCanvas> Planes { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public DesignerCanvas SelectedPlane { get; set; } // TODO : INotyfyPropertyChanged

        #endregion

        #region Methods

        private void AddNewLevel()
        {
            DesignerCanvas desgnCanv = new DesignerCanvas
            {
                DesignerControl = this
            };

            if(DesignerArea.Children.Count < 1)
                desgnCanv.Background = BackgroundColor;

            // TODO : Prüfen ob ich stattdessen Planes an das Grid binden kann
            DesignerArea.Children.Add(desgnCanv);
            Planes.Add(desgnCanv);

            desgnCanv.HorizontalAlignment = HorizontalAlignment.Stretch;
            desgnCanv.VerticalAlignment = VerticalAlignment.Stretch;
        }

        private void ShowZoomBoxDialog()
        {
            if(ZoomDialog != null)
            {
                ZoomDialog.Activate();
                return;
            }

            Dialogs.ZoomDialog zd = new Dialogs.ZoomDialog
            {
                ScrollViewer = DesignerScroller,
                DesignerArea = DesignerArea
            };

            zd.Show();

            ZoomDialog = zd;
        }

        private void ShowPlaneDialog()
        {
            if(PlaneDialog != null)
            {
                PlaneDialog.Activate();
                return;
            }

            Dialogs.PlaneDialog pd = new Dialogs.PlaneDialog();
            pd.DataContext = this;

            pd.Show();

            PlaneDialog = pd;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DesignerScroller = GetTemplateChild("PART_DESIGNERSCROLLER") as ScrollViewer;
            DesignerArea = GetTemplateChild("PART_DESIGNERAREA") as Grid;

            AddNewLevel();
        }

        #endregion

        #region Events

        private void Designer_Loaded(object sender, RoutedEventArgs e)
        {
            if(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            if(IsZoomDialogActiv)
                ShowZoomBoxDialog();

            if(IsPlaneDialogActiv)
                ShowPlaneDialog();
        }

        private void Designer_Unloaded(object sender, RoutedEventArgs e)
        {
            if(ZoomDialog != null)
                ZoomDialog.Close();

            if(PlaneDialog != null)
                PlaneDialog.Close();
        }

        #endregion

    }

    public enum ItemEditMode
    {
        All = 0,
        ResizeOnly = 1,
        RotateOnly = 2,
        None = 3
    }
}