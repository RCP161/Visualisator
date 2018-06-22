using System;
using System.Collections.Generic;
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

        private static Style _designerStyle;
        private Style DesignerStyle
        {
            get
            {
                if (_designerStyle == null)
                {
                    Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/ToolBoxControl;component/Styles/DesignerStyle.xaml", UriKind.RelativeOrAbsolute) });
                    _designerStyle = FindResource("DesignerStyle") as Style;
                }
                return _designerStyle;
            }
        }

        public Designer() : base()
        {
            Style = DesignerStyle;
            DataContext = this;
        }

        // TODO : Properties einpflegen

        // DesignerCollection für ebenen
        // Colors (generell, dass es normal gestylet werden kann)
        // Raster an DesignerCanvas 

        // Funktion zum aufrufen der ZoomBox 
        // Funktion zum hinzufügen/entfernen/verschieben von Ebenen 
        // Beim schließen alle Fenster schließen

        // Prio Z
        // Dockverhalten ausbauen mit Docken auch an die anderen Punkte, oder auch beim Resize eines Items


        // Fehler: 
        // System.Windows.Data Error: 2 : Cannot find governing FrameworkElement or FrameworkContentElement for target element. BindingExpression:Path=ScrollViewer.Content; DataItem=null; target element is 'VisualBrush' (HashCode=30123835); target property is 'Visual' (type 'Visual')


        /// <summary>
        /// Gibt an, ob die ZoomBox gestartet werden soll
        /// </summary>
        public bool IsZoomBoxActiv
        {
            get { return (bool)GetValue(IsZoomBoxActivProperty); }
            set { SetValue(IsZoomBoxActivProperty, value); }
        }

        public static readonly DependencyProperty IsZoomBoxActivProperty = DependencyProperty.Register("IsZoomBoxActiv", typeof(bool), typeof(Designer), new FrameworkPropertyMetadata(false));

        
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

    }

    public enum ItemEditMode
    {
        All = 0,
        ResizeOnly = 1,
        RotateOnly = 2,
        None = 3
    }
}