Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Data
Imports StdColumnDefinition = System.Windows.Controls.ColumnDefinition
Imports StdGrid = System.Windows.Controls.Grid
Imports System.Windows.Controls
Imports DevExpress.Xpf.Grid

Namespace BandedViewExtension
	Public Class CellLayoutControl
		Inherits CellItemsControl
		Public Shared ReadOnly ViewProperty As DependencyProperty = DependencyProperty.Register("View", GetType(DataViewBase), GetType(CellLayoutControl), Nothing)
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
		Private ReadOnly Property LayoutPanel() As StdGrid
			Get
				Return CType(Panel, StdGrid)
			End Get
		End Property

		Public Sub New()
			AddHandler Loaded, AddressOf OnLoaded
		End Sub
		Protected Overrides Function CreateChild(ByVal item As Object) As FrameworkElement
			Dim cellData As GridCellData = CType(item, GridCellData)
			Dim gridColumn As ColumnBase = cellData.Column
			Dim presenter As New AutoWidthCellContentPresenter()
			Dim row As Integer = BandedViewBehavior.GetRow(gridColumn)
			Dim column As Integer = BandedViewBehavior.GetColumn(gridColumn) + 1
			Dim rowSpan As Integer = BandedViewBehavior.GetRowSpan(gridColumn)
			Dim columnSpan As Integer = BandedViewBehavior.GetColumnSpan(gridColumn)
			StdGrid.SetRow(presenter, row)
			StdGrid.SetColumn(presenter, column)
			StdGrid.SetRowSpan(presenter, rowSpan)
			StdGrid.SetColumnSpan(presenter, columnSpan)
			If BandedViewBehavior.GetIsBand(gridColumn) Then
				presenter.Visibility = Visibility.Collapsed
			Else
				presenter.Visibility = Visibility.Visible
			End If
			Return presenter
		End Function
		Protected Overrides Sub ValidateVisualTree()
			PreparePanel()
			UpdateCells()
			MyBase.ValidateVisualTree()
		End Sub
		Private Sub OnLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			ClearPanel()
		End Sub
		Private Sub ClearPanel()
			If View Is Nothing OrElse LayoutPanel Is Nothing Then
				Return
			End If
			InvalidateMeasure()
			LayoutPanel.ColumnDefinitions.Clear()
			LayoutPanel.RowDefinitions.Clear()
		End Sub
		Private Sub PreparePanel()
			If View Is Nothing OrElse LayoutPanel Is Nothing Then
				Return
			End If
			If LayoutPanel.ColumnDefinitions.Count <> 0 OrElse LayoutPanel.RowDefinitions.Count <> 0 Then
				Return
			End If
			CreateCorrectingColumn()
			For Each columnDefinition As ColumnDefinition In BandBehavior.ColumnDefinitions
				LayoutPanel.ColumnDefinitions.Add(columnDefinition.CreateGridColumnDefinition())
			Next columnDefinition
			For Each rowDefinition As RowDefinition In BandBehavior.RowDefinitions
				LayoutPanel.RowDefinitions.Add(rowDefinition.CreateGridRowDefinition())
			Next rowDefinition
		End Sub
		Private Sub CreateCorrectingColumn()
			Dim res As New StdColumnDefinition()
			Dim b As New Binding("Level")
			b.Source = DataContext
			b.Converter = New GridLengthValueConverter()
			b.ConverterParameter = (CType(View, TableView)).LeftGroupAreaIndent
			BindingOperations.SetBinding(res, StdColumnDefinition.WidthProperty, b)
			LayoutPanel.ColumnDefinitions.Insert(0, res)

			Dim b1 As New Binding("Level")
			b1.Source = DataContext
			b1.Converter = New ValueConverter()
			b1.ConverterParameter = (CType(View, TableView)).LeftGroupAreaIndent
			BindingOperations.SetBinding(Me, CellLayoutControl.MarginProperty, b1)
		End Sub
		Private Sub UpdateCells()
			If View Is Nothing OrElse LayoutPanel Is Nothing Then
				Return
			End If
			For Each presenter As AutoWidthCellContentPresenter In LayoutPanel.Children
				If BandedViewBehavior.GetIsBottomColumn(presenter.Column) Then
					presenter.SetBorderThickness(New Thickness(0, 0, 1, 0))
				Else
					presenter.SetBorderThickness(New Thickness(0, 0, 1, 1))
					presenter.Padding = New Thickness(0, 0, 1, 1)
				End If
			Next presenter
		End Sub

		Private Class ValueConverter
			Implements IValueConverter
			Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
				Dim res As Double = CInt(Fix(value)) * CDbl(parameter)
				Return New Thickness() With {.Left = -res}
			End Function
			Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
				Throw New NotImplementedException()
			End Function
		End Class
	End Class
	Public Class AutoWidthCellContentPresenter
		Inherits GridCellContentPresenter
		Public Sub SetBorderThickness(ByVal value As Thickness)
			Dim border As Border = CType(GetTemplateChild("ContentBorder"), Border)
			If border Is Nothing Then
				Return
			End If
			border.BorderThickness = value
		End Sub
		Public Sub SetBorderPadding(ByVal value As Thickness)
			Dim border As Border = CType(GetTemplateChild("ContentBorder"), Border)
			If border Is Nothing Then
				Return
			End If
			border.Padding = value
		End Sub
		Protected Overrides Sub SyncWidth(ByVal cellData As GridCellData)
		End Sub
		Public Sub New()
		End Sub
	End Class
End Namespace
