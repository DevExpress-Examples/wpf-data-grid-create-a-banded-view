using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
namespace BandedViewExtension {
    public class BandedViewBehavior : Behavior<TableView> {
        #region static
        internal static readonly DependencyProperty ColumnsLayoutControlProperty =
            DependencyProperty.RegisterAttached("ColumnsLayoutControl", typeof(ColumnsLayoutControl), typeof(BandedViewBehavior), null);

        public static readonly DependencyProperty TemplatesContainerProperty =
            DependencyProperty.Register("TemplatesContainer", typeof(TemplatesContainer), typeof(BandedViewBehavior), null);

        public static readonly DependencyProperty ColumnDefinitionsProperty =
            DependencyProperty.Register("ColumnDefinitions", typeof(ColumnDefinitions), typeof(BandedViewBehavior), null);
        public static readonly DependencyProperty RowDefinitionsProperty =
            DependencyProperty.Register("RowDefinitions", typeof(RowDefinitions), typeof(BandedViewBehavior), null);

        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached("Column", typeof(int), typeof(BandedViewBehavior), new PropertyMetadata(0));
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.RegisterAttached("Row", typeof(int), typeof(BandedViewBehavior), new PropertyMetadata(0));

        public static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.RegisterAttached("ColumnSpan", typeof(int), typeof(BandedViewBehavior), new PropertyMetadata(1));
        public static readonly DependencyProperty RowSpanProperty =
            DependencyProperty.RegisterAttached("RowSpan", typeof(int), typeof(BandedViewBehavior), new PropertyMetadata(1));

        public static readonly DependencyProperty IsBandProperty =
            DependencyProperty.RegisterAttached("IsBand", typeof(bool), typeof(BandedViewBehavior),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBandPropertyChanged)));

        internal static readonly DependencyProperty IsLeftColumnProperty =
           DependencyProperty.RegisterAttached("IsLeftColumn", typeof(bool), typeof(ColumnsLayoutControl), null);
        internal static readonly DependencyProperty IsTopColumnProperty =
            DependencyProperty.RegisterAttached("IsTopColumn", typeof(bool), typeof(ColumnsLayoutControl), null);
        internal static readonly DependencyProperty IsRightColumnProperty =
            DependencyProperty.RegisterAttached("IsRightColumn", typeof(bool), typeof(ColumnsLayoutControl), null);
        internal static readonly DependencyProperty IsBottomColumnProperty =
            DependencyProperty.RegisterAttached("IsBottomColumn", typeof(bool), typeof(ColumnsLayoutControl), null);

        internal static ColumnsLayoutControl GetColumnsLayoutControl(ColumnBase obj) {
            return (ColumnsLayoutControl)obj.GetValue(ColumnsLayoutControlProperty);
        }
        internal static void SetColumnsLayoutControl(ColumnBase obj, ColumnsLayoutControl value) {
            obj.SetValue(ColumnsLayoutControlProperty, value);
        }

        public static int GetColumn(ColumnBase obj) {
            return (int)obj.GetValue(ColumnProperty);
        }
        public static void SetColumn(ColumnBase obj, int value) {
            obj.SetValue(ColumnProperty, value);
        }

        public static int GetRow(ColumnBase obj) {
            return (int)obj.GetValue(RowProperty);
        }
        public static void SetRow(ColumnBase obj, int value) {
            obj.SetValue(RowProperty, value);
        }

        public static int GetColumnSpan(ColumnBase obj) {
            return (int)obj.GetValue(ColumnSpanProperty);
        }
        public static void SetColumnSpan(ColumnBase obj, int value) {
            obj.SetValue(ColumnSpanProperty, value);
        }

        public static int GetRowSpan(ColumnBase obj) {
            return (int)obj.GetValue(RowSpanProperty);
        }
        public static void SetRowSpan(ColumnBase obj, int value) {
            obj.SetValue(RowSpanProperty, value);
        }

        public static bool GetIsBand(ColumnBase obj) {
            return (bool)obj.GetValue(IsBandProperty);
        }
        public static void SetIsBand(ColumnBase obj, bool value) {
            obj.SetValue(IsBandProperty, value);
        }
        static void OnIsBandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            ColumnBase column = (ColumnBase)obj;
            if(!(bool)e.NewValue) return;
            CheckDefaultPropertyValue(column, "AllowSorting", column.AllowSorting); column.AllowSorting = DefaultBoolean.False;
            CheckDefaultPropertyValue(column, "AllowMoving", column.AllowMoving); column.AllowMoving = DefaultBoolean.False;
            CheckDefaultPropertyValue(column, "AllowResizing", column.AllowResizing); column.AllowResizing = DefaultBoolean.True;
            CheckDefaultPropertyValue(column, "AllowColumnFiltering", column.AllowColumnFiltering); column.AllowColumnFiltering = DefaultBoolean.False;
        }
        static void CheckDefaultPropertyValue(ColumnBase column, string propertyName, DefaultBoolean value) {
            if(value == DefaultBoolean.Default) return;
            new InvalidColumnPropertyValueException(column, propertyName);
        }

        internal static bool GetIsLeftColumn(ColumnBase obj) {
            return (bool)obj.GetValue(IsLeftColumnProperty);
        }
        internal static void SetIsLeftColumn(ColumnBase obj, bool value) {
            obj.SetValue(IsLeftColumnProperty, value);
        }
        internal static bool GetIsTopColumn(ColumnBase obj) {
            return (bool)obj.GetValue(IsTopColumnProperty);
        }
        internal static void SetIsTopColumn(ColumnBase obj, bool value) {
            obj.SetValue(IsTopColumnProperty, value);
        }
        internal static bool GetIsRightColumn(ColumnBase obj) {
            return (bool)obj.GetValue(IsRightColumnProperty);
        }
        internal static void SetIsRightColumn(ColumnBase obj, bool value) {
            obj.SetValue(IsRightColumnProperty, value);
        }
        internal static bool GetIsBottomColumn(ColumnBase obj) {
            return (bool)obj.GetValue(IsBottomColumnProperty);
        }
        internal static void SetIsBottomColumn(ColumnBase obj, bool value) {
            obj.SetValue(IsBottomColumnProperty, value);
        }

        internal static void UpdateColumnPosition(BandedViewBehavior bandBehavior,ColumnBase obj) {
            int row = GetRow(obj);
            int column = GetColumn(obj);
            int rowSpan = GetRowSpan(obj);
            int columnSpan = GetColumnSpan(obj);

            SetIsLeftColumn(obj, column <= 0);
            SetIsTopColumn(obj, row <= 0);
            SetIsRightColumn(obj, column + columnSpan >= bandBehavior.ColumnDefinitions.Count);
            SetIsBottomColumn(obj, row + rowSpan >= bandBehavior.RowDefinitions.Count);
        }

        public static BandedViewBehavior GetBandBehaviour(TableView view) {
            foreach(Behavior behavior in Interaction.GetBehaviors(view))
                if(behavior is BandedViewBehavior) return (BandedViewBehavior)behavior;
            return null;
        }
        #endregion
        public TemplatesContainer TemplatesContainer {
            get { return (TemplatesContainer)GetValue(TemplatesContainerProperty); }
            set { SetValue(TemplatesContainerProperty, value); }
        }
        public ColumnDefinitions ColumnDefinitions {
            get { return (ColumnDefinitions)GetValue(ColumnDefinitionsProperty); }
            set { SetValue(ColumnDefinitionsProperty, value); }
        }
        public RowDefinitions RowDefinitions {
            get { return (RowDefinitions)GetValue(RowDefinitionsProperty); }
            set { SetValue(RowDefinitionsProperty, value); }
        }
        public BandedViewBehavior() {
#if !SILVERLIGHT
            TemplatesContainer = new TemplatesContainer();
#endif
            ColumnDefinitions = new ColumnDefinitions();
            RowDefinitions = new RowDefinitions();
        }
        [Browsable(false)]
        public Thickness GetColumnHeaderMargin(ColumnBase column) {
            Thickness res = new Thickness();
            if(GetIsBottomColumn(column))
                res = TemplatesContainer.GetBottomColumnHeaderIndent();
            else if(GetIsTopColumn(column))
                res = TemplatesContainer.GetTopColumnHeaderIndent();
            else res = TemplatesContainer.GetMiddleColumnHeaderIndent();
            return res;
        }
        [Browsable(false)]
        public void UpdateColumnHeaderTemplate(ColumnBase column) {
            if(GetIsBand(column)) column.HeaderTemplate = TemplatesContainer.BandColumnHeaderTemplate;
        }
        protected override void OnAttached() {
            if(TemplatesContainer == null)
                throw new InvalidOperationException("The TemplatesContainer property cannot be null, please set it in the xaml.");
            base.OnAttached();
            AssociatedObject.LayoutCalculatorFactory = new BandedViewLayoutCalculatorFactory();
            AssociatedObject.HeaderTemplate = TemplatesContainer.GridHeadersTemplate;
            AssociatedObject.DataRowTemplate = TemplatesContainer.GridDataRowTemplate;
            AssociatedObject.ShowGroupedColumns = true;
            AssociatedObject.AutoWidth = true;
            AssociatedObject.AllowMoveColumnToDropArea = false;
            AssociatedObject.ShowGridMenu += OnShowGridMenu;
        }
        protected override void OnDetaching() {
            AssociatedObject.ShowGridMenu -= OnShowGridMenu;
            base.OnDetaching();
        }

        void OnShowGridMenu(object sender, GridMenuEventArgs e) {
            BarItem showColumnChooserBarItem = null;
            foreach(BarItem barItem in e.Items)
                if(barItem.Name == "ItemColumnChooser" || (barItem.Content != null && barItem.Content.ToString() == "Show Column Chooser"))
                    showColumnChooserBarItem = barItem;
            if(showColumnChooserBarItem != null)
                showColumnChooserBarItem.IsVisible = false;
        }
    }
}