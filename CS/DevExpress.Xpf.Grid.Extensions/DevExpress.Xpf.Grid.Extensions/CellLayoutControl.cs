using System;
using System.Windows;
using System.Windows.Data;
using StdColumnDefinition = System.Windows.Controls.ColumnDefinition;
using StdGrid = System.Windows.Controls.Grid;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;

namespace BandedViewExtension {
    public class CellLayoutControl : CellItemsControl {
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(DataViewBase), typeof(CellLayoutControl), null);
        public DataViewBase View {
            get { return (DataViewBase)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }
        public BandedViewBehavior BandBehavior { get { return BandedViewBehavior.GetBandBehaviour((TableView)View); } }
        StdGrid LayoutPanel { get { return (StdGrid)Panel; } }

        public CellLayoutControl() {
            Loaded += OnLoaded;
        }
        protected override FrameworkElement CreateChild(object item) {
            GridCellData cellData = (GridCellData)item;
            ColumnBase gridColumn = cellData.Column;
            AutoWidthCellContentPresenter presenter = new AutoWidthCellContentPresenter();
            int row = BandedViewBehavior.GetRow(gridColumn);
            int column = BandedViewBehavior.GetColumn(gridColumn) + 1;
            int rowSpan = BandedViewBehavior.GetRowSpan(gridColumn);
            int columnSpan = BandedViewBehavior.GetColumnSpan(gridColumn);
            StdGrid.SetRow(presenter, row);
            StdGrid.SetColumn(presenter, column);
            StdGrid.SetRowSpan(presenter, rowSpan);
            StdGrid.SetColumnSpan(presenter, columnSpan);
            if(BandedViewBehavior.GetIsBand(gridColumn)) presenter.Visibility = Visibility.Collapsed;
            else presenter.Visibility = Visibility.Visible;
            return presenter;
        }
        protected override void ValidateVisualTree() {
            PreparePanel();
            UpdateCells();
            base.ValidateVisualTree();
        }
        void OnLoaded(object sender, RoutedEventArgs e) {
            ClearPanel();
        }
        void ClearPanel() {
            if(View == null || LayoutPanel == null) return;
            InvalidateMeasure();
            LayoutPanel.ColumnDefinitions.Clear();
            LayoutPanel.RowDefinitions.Clear();
        }
        void PreparePanel() {
            if(View == null || LayoutPanel == null) return;
            if(LayoutPanel.ColumnDefinitions.Count != 0 || LayoutPanel.RowDefinitions.Count != 0) return;
            CreateCorrectingColumn();
            foreach(ColumnDefinition columnDefinition in BandBehavior.ColumnDefinitions)
                LayoutPanel.ColumnDefinitions.Add(columnDefinition.CreateGridColumnDefinition());
            foreach(RowDefinition rowDefinition in BandBehavior.RowDefinitions)
                LayoutPanel.RowDefinitions.Add(rowDefinition.CreateGridRowDefinition());
        }
        void CreateCorrectingColumn() {
            StdColumnDefinition res = new StdColumnDefinition();
            Binding b = new Binding("Level");
            b.Source = DataContext;
            b.Converter = new GridLengthValueConverter();
            b.ConverterParameter = ((TableView)View).LeftGroupAreaIndent;
            BindingOperations.SetBinding(res, StdColumnDefinition.WidthProperty, b);
            LayoutPanel.ColumnDefinitions.Insert(0, res);

            Binding b1 = new Binding("Level");
            b1.Source = DataContext;
            b1.Converter = new ValueConverter();
            b1.ConverterParameter = ((TableView)View).LeftGroupAreaIndent;
            BindingOperations.SetBinding(this, CellLayoutControl.MarginProperty, b1);
        }
        void UpdateCells() {
            if(View == null || LayoutPanel == null) return;
            foreach(AutoWidthCellContentPresenter presenter in LayoutPanel.Children) {
                if(BandedViewBehavior.GetIsBottomColumn(presenter.Column)) {
                    presenter.SetBorderThickness(new Thickness(0, 0, 1, 0));
                } else {
                    presenter.SetBorderThickness(new Thickness(0, 0, 1, 1));
                    presenter.Padding = new Thickness(0, 0, 1, 1);
                }
            }
        }

        class ValueConverter : IValueConverter {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
                double res = (int)value * (double)parameter;
                return new Thickness() { Left = -res };
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
                throw new NotImplementedException();
            }
        }
    }
    public class AutoWidthCellContentPresenter : GridCellContentPresenter {
        public void SetBorderThickness(Thickness value) {
            Border border = (Border)GetTemplateChild("ContentBorder");
            if(border == null) return;
            border.BorderThickness = value;
        }
        public void SetBorderPadding(Thickness value) {
            Border border = (Border)GetTemplateChild("ContentBorder");
            if(border == null) return;
            border.Padding = value;
        }
        protected override void SyncWidth(GridCellData cellData) { }
        public AutoWidthCellContentPresenter() { }
    }
}
