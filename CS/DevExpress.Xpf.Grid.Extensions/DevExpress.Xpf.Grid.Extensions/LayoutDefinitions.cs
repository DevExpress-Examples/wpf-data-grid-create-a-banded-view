using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using DevExpress.Utils;
using GridColumnDefinition = System.Windows.Controls.ColumnDefinition;
using GridRowDefinition = System.Windows.Controls.RowDefinition;
namespace BandedViewExtension {
    public class ColumnDefinition : DependencyObject {
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(GridLength), typeof(ColumnDefinition), null);
        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.Register("MinWidth", typeof(double), typeof(ColumnDefinition), new PropertyMetadata(8d));
        public GridLength Width {
            get { return (GridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public double MinWidth {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }
        public GridColumnDefinition CreateGridColumnDefinition() {
            GridColumnDefinition column = new GridColumnDefinition() { MinWidth = this.MinWidth };
            BindingOperations.SetBinding(column, GridColumnDefinition.WidthProperty, new Binding("Width") { Source = this });
            return column;
        }
    }
    public class RowDefinition : DependencyObject {
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(GridLength), typeof(RowDefinition), null);
        public GridLength Height {
            get { return (GridLength)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        
        public GridRowDefinition CreateGridRowDefinition() {
            return new GridRowDefinition() { Height = this.Height };
        }
    }

    public class ColumnDefinitions : List<ColumnDefinition> { }
    public class RowDefinitions : List<RowDefinition> { }
}
