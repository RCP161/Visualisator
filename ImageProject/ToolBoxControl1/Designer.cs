﻿using System;
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

namespace ToolBoxControl
{
    /// <summary>
    /// Interaktionslogik für Designer.xaml
    /// </summary>
    public class Designer : ContentControl
    {
        public static readonly int DesignerDefaultWidth = 800;
        public static readonly int DesignerDefaultHeight = 600;
        public static readonly int ItemDefaultWidth = 65;
        public static readonly int ItemDefaultHeight = 65;
        public static readonly int ItemDockRange = 8;

        public Designer() : base()
        {
            ClipToBounds = true;
            SnapsToDevicePixels = true;

            DataContext = this;
        }

        // TODO : Properties einpflegen

        // Colors (generell, dass es normal gestylet werden kann)
        // Raster an DesignerCanvas 

        // Funktion zum aufrufen der ZoomBox 
        // Dialog zum hinzufügen/entfernen/verschieben von Ebenen 
        // Beim schließen alle Fenster schließen

        // Prio Z
        // Dockverhalten ausbauen mit Docken auch an die anderen Punkte, oder auch beim Resize eines Items



        #region WPF Properties

        /// <summary>
        /// Gibt an, ob die ZoomBox gestartet werden soll
        /// </summary>
        public bool IsZoomBoxActiv
        {
            get { return (bool)GetValue(IsZoomBoxActivProperty); }
            set { SetValue(IsZoomBoxActivProperty, value); }
        }

        public static readonly DependencyProperty IsZoomBoxActivProperty = DependencyProperty.Register("IsZoomBoxActiv", typeof(bool), typeof(Designer), new FrameworkPropertyMetadata(true)); // TODO : Änderun zu flase


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

        internal  ScrollViewer DesignerScroller { get; private set; }

        internal Canvas DesignerArea { get; private set; }

        #endregion

        #region Methods

        public void AddNewLevel()
        {
            DesignerCanvas desgnCanv = new DesignerCanvas();
            desgnCanv.DesignerControl = this;
            desgnCanv.IsSelected = true;

            if(DesignerArea.Children.Count < 1)
                desgnCanv.Background = BackgroundColor;

            DesignerArea.Children.Add(desgnCanv);

            desgnCanv.HorizontalAlignment = HorizontalAlignment.Stretch;
            desgnCanv.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public void ShowZoomBoxDialog()
        {
            if(ZoomDialog != null)
            {
                ZoomDialog.Activate();
                return;
            }

            Dialogs.ZoomDialog zd = new Dialogs.ZoomDialog();
            zd.ScrollViewer = DesignerScroller;
            zd.DesignerArea = DesignerArea;
            zd.Designer = this;
            zd.Show();

            ZoomDialog = zd;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DesignerScroller = GetTemplateChild("PART_DESIGNERSCROLLER") as ScrollViewer;
            DesignerArea = GetTemplateChild("PART_DESIGNERAREA") as Canvas;


            AddNewLevel();

            Loaded += Designer_Loaded;
            Unloaded += Designer_Unloaded;
        }

        #endregion

        #region Events

        private void Designer_Unloaded(object sender, RoutedEventArgs e)
        {
            if(ZoomDialog != null)
                ZoomDialog.Close();
        }

        private void Designer_Loaded(object sender, RoutedEventArgs e)
        {
            if(IsZoomBoxActiv)
                ShowZoomBoxDialog();
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
