using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using ToolBoxControl.Controls;
using ToolBoxControl.ViewModels;

namespace ToolBoxControl
{
    public class Designer : ContentControl
    {
        public static readonly int DesignerDefaultWidth = 800;
        public static readonly int DesignerDefaultHeight = 600;
        public static readonly int ItemDefaultWidth = 65;
        public static readonly int ItemDefaultHeight = 65;
        public static readonly int ItemDockRange = 8;

        private readonly DesignerVm viewModel;

        static Designer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Designer), new FrameworkPropertyMetadata(typeof(Designer)));
        }

        public Designer() : base()
        {
            viewModel = new DesignerVm();
            DataContext = viewModel;
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

        // ViewModels einführen, um Unsichtbar Attribute wieder los zu werden
        // Drop auf der slektierten Ebene
        // Ebenen Name

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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            viewModel.DesignerScroller = GetTemplateChild("PART_DESIGNERSCROLLER") as ScrollViewer;
            viewModel.DesignerArea = GetTemplateChild("PART_DESIGNERAREA") as ItemsControl;
        }
    }
}