using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ToolBoxControl.Controls
{
    internal class AdornerVisualisator : Control
    {
        public AdornerVisualisator()
        {
            Unloaded += new RoutedEventHandler(DesignerItemDecorator_Unloaded);
        }

        private Adorner adorner;

        public bool ShowItemUI
        {
            get { return (bool)GetValue(ShowItemUIProperty); }
            set { SetValue(ShowItemUIProperty, value); }
        }

        public static readonly DependencyProperty ShowItemUIProperty = DependencyProperty.Register("ShowItemUI", typeof(bool), typeof(AdornerVisualisator), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ShowItemUIProperty_Changed)));

        public static void SetShowItemUI(UIElement element, bool value)
        {
            element.SetValue(ShowItemUIProperty, value);
        }

        public static bool GetShowItemUI(UIElement element)
        {
            return (bool)element.GetValue(ShowItemUIProperty);
        }

        private void HideAdorner()
        {
            if (adorner != null)
                adorner.Visibility = Visibility.Hidden;
        }

        private void ShowAdorner()
        {
            if (adorner == null)
            {
                DesignerItem designerItem =  DataContext as DesignerItem;
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(designerItem);

                if (adornerLayer != null)
                {
                     adorner = new ResizeRotateAdorner(designerItem);
                    adornerLayer.Add( adorner);

                    if ( ShowItemUI)
                         adorner.Visibility = Visibility.Visible;
                    else
                         adorner.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                 adorner.Visibility = Visibility.Visible;
            }
        }

        private void DesignerItemDecorator_Unloaded(object sender, RoutedEventArgs e)
        {
            if ( adorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                    adornerLayer.Remove( adorner);

                 adorner = null;
            }
        }

        private static void ShowItemUIProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdornerVisualisator av = (AdornerVisualisator)d;
            bool show = (bool)e.NewValue;

            if (show)
                av.ShowAdorner();
            else
                av.HideAdorner();
        }
    }
}
