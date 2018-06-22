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
            Unloaded += new RoutedEventHandler(this.DesignerItemDecorator_Unloaded);
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
            if (this.adorner != null)
                this.adorner.Visibility = Visibility.Hidden;
        }

        private void ShowAdorner()
        {
            if (this.adorner == null)
            {
                DesignerItem designerItem = this.DataContext as DesignerItem;
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(designerItem);

                if (adornerLayer != null)
                {
                    this.adorner = new ResizeRotateAdorner(designerItem);
                    adornerLayer.Add(this.adorner);

                    if (this.ShowItemUI)
                        this.adorner.Visibility = Visibility.Visible;
                    else
                        this.adorner.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                this.adorner.Visibility = Visibility.Visible;
            }
        }

        private void DesignerItemDecorator_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.adorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                    adornerLayer.Remove(this.adorner);

                this.adorner = null;
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
