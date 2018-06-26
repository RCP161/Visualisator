using System;
using System.Collections.Generic;
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
using System.Windows.Media;
using System.Xml;
using ToolBoxControl.Controls;

namespace ToolBoxControl.Controls
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DesignerCanvas : Canvas
    {
        private Point? dragStartPoint = null;

        static DesignerCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerCanvas), new FrameworkPropertyMetadata(typeof(DesignerCanvas)));
        }

        public Designer DesignerControl
        {
            get { return (Designer)GetValue(DesignerControlProperty); }
            set { SetValue(DesignerControlProperty, value); }
        }

        public static readonly DependencyProperty DesignerControlProperty = DependencyProperty.Register("DesignerControl", typeof(Designer), typeof(DesignerCanvas), new FrameworkPropertyMetadata(null));


        public bool IsVisibleInDesigner
        {
            get { return (bool)GetValue(IsVisibleInDesignerProperty); }
            set { SetValue(IsVisibleInDesignerProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleInDesignerProperty = DependencyProperty.Register("IsVisibleInDesigner", typeof(bool), typeof(DesignerCanvas), new FrameworkPropertyMetadata(true));


        public IEnumerable<DesignerItem> SelectedItems
        {
            get
            {
                var selectedItems = from item in  Children.OfType<DesignerItem>()
                                    where item.IsSelected == true
                                    select item;

                return selectedItems;
            }
        }

        public void DeselectAll()
        {
            foreach (DesignerItem item in  SelectedItems)
                item.IsSelected = false;
        }
        
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                 dragStartPoint = new Point?(e.GetPosition(this));
                 DeselectAll();
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
                 dragStartPoint = null;

            if ( dragStartPoint.HasValue)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this,  dragStartPoint);
                    if (adorner != null)
                        adornerLayer.Add(adorner);
                }

                e.Handled = true;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            string xamlString = e.Data.GetData("DESIGNER_ITEM") as string;
            if (!String.IsNullOrEmpty(xamlString))
            {
                DesignerItem newItem = null;

                if(XamlReader.Load(XmlReader.Create(new StringReader(xamlString))) is FrameworkElement content)
                {
                    newItem = new DesignerItem
                    {
                        Content = content,
                        DesignerCanvas = this
                    };

                    var pos = e.GetPosition(this);

                    if(content.MinHeight != 0 && content.MinWidth != 0)
                    {
                        newItem.Width = content.MinWidth * 2;
                        newItem.Height = content.MinHeight * 2;
                    }
                    else
                    {
                        newItem.Width = Designer.ItemDefaultWidth;
                        newItem.Height = Designer.ItemDefaultHeight;
                    }

                    DesignerCanvas.SetLeft(newItem, Math.Max(0, pos.X - newItem.Width / 2));
                    DesignerCanvas.SetTop(newItem, Math.Max(0, pos.Y - newItem.Height / 2));
                    Children.Add(newItem);

                    newItem.Position = new Point(DesignerCanvas.GetLeft(newItem), DesignerCanvas.GetTop(newItem));

                    DeselectAll();
                    newItem.IsSelected = true;
                }

                e.Handled = true;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();

            if (!DesignerControl.DesignerCanResize)
            {
                foreach (UIElement element in Children)
                    element.Measure(constraint);

                return size;
            }

            foreach (UIElement element in Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = Double.IsNaN(left) ? 0 : left;
                top = Double.IsNaN(top) ? 0 : top;

                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!Double.IsNaN(desiredSize.Width) && !Double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }

            // add some extra margin
            size.Width += 10;
            size.Height += 10;

            return size;
        }
    }
}