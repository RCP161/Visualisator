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
        private readonly DesignerVm viewModel;

        static Designer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Designer), new FrameworkPropertyMetadata(typeof(Designer)));
        }

        public Designer() : base()
        {
            viewModel = new DesignerVm();
            DataContext = viewModel;

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

        // ViewModels einführen, um Unsichtbar Attribute wieder los zu werden
        // Drop auf der slektierten Ebene
        // Ebenen Name

        #region WPF Properties

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
            viewModel.DesignerControl = this;

            viewModel.AfterStartUp();
        }

        private void Designer_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel.OnClose();
        }
    }
}