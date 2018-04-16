using System.Windows;
using System.Windows.Controls;
using DevExpress.Utils;
using StdGrid = System.Windows.Controls.Grid;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid.Native;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
namespace BandedViewExtension {
    public class ColumnsLayoutControl : HeaderItemsControl {
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(DataViewBase), typeof(ColumnsLayoutControl), null);
        public DataViewBase View {
            get { return (DataViewBase)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }
        public BandedViewBehavior BandBehavior { get { return BandedViewBehavior.GetBandBehaviour((TableView)View); } }
        public ResizableGrid LayoutPanel { get { return (ResizableGrid)Panel; } }
        public ColumnsLayoutControl() {
            Loaded += OnLoaded;
        }
        public void Resize(ColumnBase column, double value) {
            ContentPresenter columnPresenter = null;
            foreach(ContentPresenter cp in LayoutPanel.Children) {
                if(((GridColumnData)(cp.Content)).Column == column)
                    columnPresenter = cp;
            }
            double diff = value - columnPresenter.ActualWidth;
            LayoutPanel.Resize(column, diff);
        }
        protected override FrameworkElement CreateChild(object item) {
            ContentPresenter child = (ContentPresenter)base.CreateChild(item);
            ColumnBase column = ((GridColumnData)item).Column;
            BandBehavior.UpdateColumnHeaderTemplate(column);
            BandedViewBehavior.SetColumnsLayoutControl(column, this);
            BandedViewBehavior.UpdateColumnPosition(BandBehavior, column);
            PrepareChild(child, column);
            return child;
        }
        protected override void ValidateVisualTree() {
            base.ValidateVisualTree();
        }
        void OnLoaded(object sender, RoutedEventArgs e) {
            ClearPanel();
            PreparePanel();
        }
        void PreparePanel() {
            if(View == null || LayoutPanel == null) return;
            if(LayoutPanel.ColumnDefinitions.Count != 0 || LayoutPanel.RowDefinitions.Count != 0) return;
            LayoutPanel.ClearPanel();
        }
        void ClearPanel() {
            if(View == null || LayoutPanel == null) return;
            InvalidateMeasure();
            LayoutPanel.Initialize(this);
            LayoutPanel.PreparePanel();
        }
        void PrepareChild(ContentPresenter child, ColumnBase column) {
            int columnCorrectingCoef = BandedViewBehavior.GetIsLeftColumn(column) ? 0 : 1;
            int columnSpanCorrectingCoef = BandedViewBehavior.GetIsLeftColumn(column) ? 1 : 0;
            StdGrid.SetRow(child, BandedViewBehavior.GetRow(column));
            StdGrid.SetColumn(child, BandedViewBehavior.GetColumn(column) + columnCorrectingCoef);
            StdGrid.SetRowSpan(child, BandedViewBehavior.GetRowSpan(column));
            StdGrid.SetColumnSpan(child, BandedViewBehavior.GetColumnSpan(column) + columnSpanCorrectingCoef);
        }
    }
    public class BandGridColumnHeader : GridColumnHeader {
        ColumnBase GridColumn { get { return DataContext as ColumnBase; } }
        TableView View { get { return (TableView)GridColumn.View; } }
        BandedViewBehavior BandBehavior { get { return BandedViewBehavior.GetBandBehaviour(View); } }
        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
            if(GridColumn == null) return;
            Margin = BandBehavior.GetColumnHeaderMargin(GridColumn);
            if(BandedViewBehavior.GetIsBand(GridColumn)) BarManager.SetDXContextMenu(this, null);
            else BarManager.SetDXContextMenu(this, View.DataControlMenu);
        }
    }

    public class BandColumnHeaderPanelDropTargetFactoryExtension : ColumnHeaderDropTargetFactoryExtension {
        protected override IDropTarget CreateDropTargetCore(Panel panel) {
            return new BandColumnHeaderPanelDropTarget();
        }
    }
    public class BandColumnHeaderPanelDropTarget : RemoveColumnDropTarget {
        GridColumnHeader GridColumnHeader;
        GridColumn Column;
        ColumnsLayoutControl ColumnsLayoutControl;
        UIElement SourceElement;
        TableView View;

        void Init(UIElement source) {
            GridColumnHeader = ((GridColumnHeader)source);
            ISupportDragDrop iSupportDragDrop = GridColumnHeader;
            SourceElement = iSupportDragDrop.SourceElement;
            Column = (GridColumn)GridColumnHeader.GetGridColumn(GridColumnHeader);
            ColumnsLayoutControl = BandedViewBehavior.GetColumnsLayoutControl(Column);
            View = (TableView)Column.View;
        }

        public override void OnDragLeave() {
            if(SourceElement is BandGridColumnHeader) return;
            base.OnDragLeave();
        }
        public override void OnDragOver(UIElement source, Point pt) {
            Init(source);
            if(SourceElement is BandGridColumnHeader) return;
            base.OnDragOver(source, pt);
        }
        public override void Drop(UIElement source, Point pt) {
            Init(source);
            base.Drop(source, pt);
            Column.Visible = true;
        }

        protected override bool IsPositionInDropZone(UIElement source, Point pt) {
            if(SourceElement is BandGridColumnHeader) return false;
            return true;
        }
    }
}
