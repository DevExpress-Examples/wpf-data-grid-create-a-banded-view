Imports Microsoft.VisualBasic
Imports System.Windows
Imports System.Windows.Controls
Imports DevExpress.Utils
Imports StdGrid = System.Windows.Controls.Grid
Imports DevExpress.Xpf.Bars
Imports DevExpress.Xpf.Core
Imports DevExpress.Xpf.Grid.Native
Imports DevExpress.Xpf.Core.Native
Imports DevExpress.Xpf.Grid
Namespace BandedViewExtension
	Public Class ColumnsLayoutControl
		Inherits HeaderItemsControl
		Public Shared ReadOnly ViewProperty As DependencyProperty = DependencyProperty.Register("View", GetType(DataViewBase), GetType(ColumnsLayoutControl), Nothing)
		Public Property View() As DataViewBase
			Get
				Return CType(GetValue(ViewProperty), DataViewBase)
			End Get
			Set(ByVal value As DataViewBase)
				SetValue(ViewProperty, value)
			End Set
		End Property
		Public ReadOnly Property BandBehavior() As BandedViewBehavior
			Get
				Return BandedViewBehavior.GetBandBehaviour(CType(View, TableView))
			End Get
		End Property
		Public ReadOnly Property LayoutPanel() As ResizableGrid
			Get
				Return CType(Panel, ResizableGrid)
			End Get
		End Property
		Public Sub New()
			AddHandler Loaded, AddressOf OnLoaded
		End Sub
		Public Sub Resize(ByVal column As ColumnBase, ByVal value As Double)
			Dim columnPresenter As ContentPresenter = Nothing
			For Each cp As ContentPresenter In LayoutPanel.Children
				If (CType(cp.Content, GridColumnData)).Column Is column Then
					columnPresenter = cp
				End If
			Next cp
			Dim diff As Double = value - columnPresenter.ActualWidth
			LayoutPanel.Resize(column, diff)
		End Sub
		Protected Overrides Function CreateChild(ByVal item As Object) As FrameworkElement
			Dim child As ContentPresenter = CType(MyBase.CreateChild(item), ContentPresenter)
			Dim column As ColumnBase = (CType(item, GridColumnData)).Column
			BandBehavior.UpdateColumnHeaderTemplate(column)
			BandedViewBehavior.SetColumnsLayoutControl(column, Me)
			BandedViewBehavior.UpdateColumnPosition(BandBehavior, column)
			PrepareChild(child, column)
			Return child
		End Function
		Protected Overrides Sub ValidateVisualTree()
			MyBase.ValidateVisualTree()
		End Sub
		Private Sub OnLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			ClearPanel()
			PreparePanel()
		End Sub
		Private Sub PreparePanel()
			If View Is Nothing OrElse LayoutPanel Is Nothing Then
				Return
			End If
			If LayoutPanel.ColumnDefinitions.Count <> 0 OrElse LayoutPanel.RowDefinitions.Count <> 0 Then
				Return
			End If
			LayoutPanel.ClearPanel()
		End Sub
		Private Sub ClearPanel()
			If View Is Nothing OrElse LayoutPanel Is Nothing Then
				Return
			End If
			InvalidateMeasure()
			LayoutPanel.Initialize(Me)
			LayoutPanel.PreparePanel()
		End Sub
		Private Sub PrepareChild(ByVal child As ContentPresenter, ByVal column As ColumnBase)
			Dim columnCorrectingCoef As Integer = If(BandedViewBehavior.GetIsLeftColumn(column), 0, 1)
			Dim columnSpanCorrectingCoef As Integer = If(BandedViewBehavior.GetIsLeftColumn(column), 1, 0)
			StdGrid.SetRow(child, BandedViewBehavior.GetRow(column))
			StdGrid.SetColumn(child, BandedViewBehavior.GetColumn(column) + columnCorrectingCoef)
			StdGrid.SetRowSpan(child, BandedViewBehavior.GetRowSpan(column))
			StdGrid.SetColumnSpan(child, BandedViewBehavior.GetColumnSpan(column) + columnSpanCorrectingCoef)
		End Sub
	End Class
	Public Class BandGridColumnHeader
		Inherits GridColumnHeader
		Private ReadOnly Property GridColumn() As ColumnBase
			Get
				Return TryCast(DataContext, ColumnBase)
			End Get
		End Property
		Private ReadOnly Property View() As TableView
			Get
				Return CType(GridColumn.View, TableView)
			End Get
		End Property
		Private ReadOnly Property BandBehavior() As BandedViewBehavior
			Get
				Return BandedViewBehavior.GetBandBehaviour(View)
			End Get
		End Property
		Public Overrides Sub OnApplyTemplate()
			MyBase.OnApplyTemplate()
			If GridColumn Is Nothing Then
				Return
			End If
			Margin = BandBehavior.GetColumnHeaderMargin(GridColumn)
			If BandedViewBehavior.GetIsBand(GridColumn) Then
				BarManager.SetDXContextMenu(Me, Nothing)
			Else
				BarManager.SetDXContextMenu(Me, View.DataControlMenu)
			End If
		End Sub
	End Class

	Public Class BandColumnHeaderPanelDropTargetFactoryExtension
		Inherits ColumnHeaderDropTargetFactoryExtension
		Protected Overrides Function CreateDropTargetCore(ByVal panel As Panel) As IDropTarget
			Return New BandColumnHeaderPanelDropTarget()
		End Function
	End Class
	Public Class BandColumnHeaderPanelDropTarget
		Inherits RemoveColumnDropTarget
		Private GridColumnHeader As GridColumnHeader
		Private Column As GridColumn
		Private ColumnsLayoutControl As ColumnsLayoutControl
		Private SourceElement As UIElement
		Private View As TableView

		Private Sub Init(ByVal source As UIElement)
			GridColumnHeader = (CType(source, GridColumnHeader))
			Dim iSupportDragDrop As ISupportDragDrop = GridColumnHeader
			SourceElement = iSupportDragDrop.SourceElement
			Column = CType(GridColumnHeader.GetGridColumn(GridColumnHeader), GridColumn)
			ColumnsLayoutControl = BandedViewBehavior.GetColumnsLayoutControl(Column)
			View = CType(Column.View, TableView)
		End Sub

		Public Overrides Sub OnDragLeave()
			If TypeOf SourceElement Is BandGridColumnHeader Then
				Return
			End If
			MyBase.OnDragLeave()
		End Sub
		Public Overrides Sub OnDragOver(ByVal source As UIElement, ByVal pt As Point)
			Init(source)
			If TypeOf SourceElement Is BandGridColumnHeader Then
				Return
			End If
			MyBase.OnDragOver(source, pt)
		End Sub
		Public Overrides Sub Drop(ByVal source As UIElement, ByVal pt As Point)
			Init(source)
			MyBase.Drop(source, pt)
			Column.Visible = True
		End Sub

		Protected Overrides Function IsPositionInDropZone(ByVal source As UIElement, ByVal pt As Point) As Boolean
			If TypeOf SourceElement Is BandGridColumnHeader Then
				Return False
			End If
			Return True
		End Function
	End Class
End Namespace
