Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Interactivity
Imports DevExpress.Utils
Imports DevExpress.Xpf.Bars
Imports DevExpress.Xpf.Grid
Namespace BandedViewExtension
	Public Class BandedViewBehavior
		Inherits Behavior(Of TableView)
		#Region "static"
		Friend Shared ReadOnly ColumnsLayoutControlProperty As DependencyProperty = DependencyProperty.RegisterAttached("ColumnsLayoutControl", GetType(ColumnsLayoutControl), GetType(BandedViewBehavior), Nothing)

		Public Shared ReadOnly TemplatesContainerProperty As DependencyProperty = DependencyProperty.Register("TemplatesContainer", GetType(TemplatesContainer), GetType(BandedViewBehavior), Nothing)

		Public Shared ReadOnly ColumnDefinitionsProperty As DependencyProperty = DependencyProperty.Register("ColumnDefinitions", GetType(ColumnDefinitions), GetType(BandedViewBehavior), Nothing)
		Public Shared ReadOnly RowDefinitionsProperty As DependencyProperty = DependencyProperty.Register("RowDefinitions", GetType(RowDefinitions), GetType(BandedViewBehavior), Nothing)

		Public Shared ReadOnly ColumnProperty As DependencyProperty = DependencyProperty.RegisterAttached("Column", GetType(Integer), GetType(BandedViewBehavior), New PropertyMetadata(0))
		Public Shared ReadOnly RowProperty As DependencyProperty = DependencyProperty.RegisterAttached("Row", GetType(Integer), GetType(BandedViewBehavior), New PropertyMetadata(0))

		Public Shared ReadOnly ColumnSpanProperty As DependencyProperty = DependencyProperty.RegisterAttached("ColumnSpan", GetType(Integer), GetType(BandedViewBehavior), New PropertyMetadata(1))
		Public Shared ReadOnly RowSpanProperty As DependencyProperty = DependencyProperty.RegisterAttached("RowSpan", GetType(Integer), GetType(BandedViewBehavior), New PropertyMetadata(1))

		Public Shared ReadOnly IsBandProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsBand", GetType(Boolean), GetType(BandedViewBehavior), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsBandPropertyChanged)))

		Friend Shared ReadOnly IsLeftColumnProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsLeftColumn", GetType(Boolean), GetType(ColumnsLayoutControl), Nothing)
		Friend Shared ReadOnly IsTopColumnProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsTopColumn", GetType(Boolean), GetType(ColumnsLayoutControl), Nothing)
		Friend Shared ReadOnly IsRightColumnProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsRightColumn", GetType(Boolean), GetType(ColumnsLayoutControl), Nothing)
		Friend Shared ReadOnly IsBottomColumnProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsBottomColumn", GetType(Boolean), GetType(ColumnsLayoutControl), Nothing)

		Friend Shared Function GetColumnsLayoutControl(ByVal obj As ColumnBase) As ColumnsLayoutControl
			Return CType(obj.GetValue(ColumnsLayoutControlProperty), ColumnsLayoutControl)
		End Function
		Friend Shared Sub SetColumnsLayoutControl(ByVal obj As ColumnBase, ByVal value As ColumnsLayoutControl)
			obj.SetValue(ColumnsLayoutControlProperty, value)
		End Sub

		Public Shared Function GetColumn(ByVal obj As ColumnBase) As Integer
			Return CInt(Fix(obj.GetValue(ColumnProperty)))
		End Function
		Public Shared Sub SetColumn(ByVal obj As ColumnBase, ByVal value As Integer)
			obj.SetValue(ColumnProperty, value)
		End Sub

		Public Shared Function GetRow(ByVal obj As ColumnBase) As Integer
			Return CInt(Fix(obj.GetValue(RowProperty)))
		End Function
		Public Shared Sub SetRow(ByVal obj As ColumnBase, ByVal value As Integer)
			obj.SetValue(RowProperty, value)
		End Sub

		Public Shared Function GetColumnSpan(ByVal obj As ColumnBase) As Integer
			Return CInt(Fix(obj.GetValue(ColumnSpanProperty)))
		End Function
		Public Shared Sub SetColumnSpan(ByVal obj As ColumnBase, ByVal value As Integer)
			obj.SetValue(ColumnSpanProperty, value)
		End Sub

		Public Shared Function GetRowSpan(ByVal obj As ColumnBase) As Integer
			Return CInt(Fix(obj.GetValue(RowSpanProperty)))
		End Function
		Public Shared Sub SetRowSpan(ByVal obj As ColumnBase, ByVal value As Integer)
			obj.SetValue(RowSpanProperty, value)
		End Sub

		Public Shared Function GetIsBand(ByVal obj As ColumnBase) As Boolean
			Return CBool(obj.GetValue(IsBandProperty))
		End Function
		Public Shared Sub SetIsBand(ByVal obj As ColumnBase, ByVal value As Boolean)
			obj.SetValue(IsBandProperty, value)
		End Sub
		Private Shared Sub OnIsBandPropertyChanged(ByVal obj As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			Dim column As ColumnBase = CType(obj, ColumnBase)
			If (Not CBool(e.NewValue)) Then
				Return
			End If
			CheckDefaultPropertyValue(column, "AllowSorting", column.AllowSorting)
			column.AllowSorting = DefaultBoolean.False
			CheckDefaultPropertyValue(column, "AllowMoving", column.AllowMoving)
			column.AllowMoving = DefaultBoolean.False
			CheckDefaultPropertyValue(column, "AllowResizing", column.AllowResizing)
			column.AllowResizing = DefaultBoolean.True
			CheckDefaultPropertyValue(column, "AllowColumnFiltering", column.AllowColumnFiltering)
			column.AllowColumnFiltering = DefaultBoolean.False
		End Sub
		Private Shared Sub CheckDefaultPropertyValue(ByVal column As ColumnBase, ByVal propertyName As String, ByVal value As DefaultBoolean)
			If value = DefaultBoolean.Default Then
				Return
			End If
			Dim TempInvalidColumnPropertyValueException As InvalidColumnPropertyValueException = New InvalidColumnPropertyValueException(column, propertyName)
		End Sub

		Friend Shared Function GetIsLeftColumn(ByVal obj As ColumnBase) As Boolean
			Return CBool(obj.GetValue(IsLeftColumnProperty))
		End Function
		Friend Shared Sub SetIsLeftColumn(ByVal obj As ColumnBase, ByVal value As Boolean)
			obj.SetValue(IsLeftColumnProperty, value)
		End Sub
		Friend Shared Function GetIsTopColumn(ByVal obj As ColumnBase) As Boolean
			Return CBool(obj.GetValue(IsTopColumnProperty))
		End Function
		Friend Shared Sub SetIsTopColumn(ByVal obj As ColumnBase, ByVal value As Boolean)
			obj.SetValue(IsTopColumnProperty, value)
		End Sub
		Friend Shared Function GetIsRightColumn(ByVal obj As ColumnBase) As Boolean
			Return CBool(obj.GetValue(IsRightColumnProperty))
		End Function
		Friend Shared Sub SetIsRightColumn(ByVal obj As ColumnBase, ByVal value As Boolean)
			obj.SetValue(IsRightColumnProperty, value)
		End Sub
		Friend Shared Function GetIsBottomColumn(ByVal obj As ColumnBase) As Boolean
			Return CBool(obj.GetValue(IsBottomColumnProperty))
		End Function
		Friend Shared Sub SetIsBottomColumn(ByVal obj As ColumnBase, ByVal value As Boolean)
			obj.SetValue(IsBottomColumnProperty, value)
		End Sub

		Friend Shared Sub UpdateColumnPosition(ByVal bandBehavior As BandedViewBehavior, ByVal obj As ColumnBase)
			Dim row As Integer = GetRow(obj)
			Dim column As Integer = GetColumn(obj)
			Dim rowSpan As Integer = GetRowSpan(obj)
			Dim columnSpan As Integer = GetColumnSpan(obj)

			SetIsLeftColumn(obj, column <= 0)
			SetIsTopColumn(obj, row <= 0)
			SetIsRightColumn(obj, column + columnSpan >= bandBehavior.ColumnDefinitions.Count)
			SetIsBottomColumn(obj, row + rowSpan >= bandBehavior.RowDefinitions.Count)
		End Sub

		Public Shared Function GetBandBehaviour(ByVal view As TableView) As BandedViewBehavior
			For Each behavior As Behavior In System.Windows.Interactivity.Interaction.GetBehaviors(view)
				If TypeOf behavior Is BandedViewBehavior Then
					Return CType(behavior, BandedViewBehavior)
				End If
			Next behavior
			Return Nothing
		End Function
		#End Region
		Public Property TemplatesContainer() As TemplatesContainer
			Get
				Return CType(GetValue(TemplatesContainerProperty), TemplatesContainer)
			End Get
			Set(ByVal value As TemplatesContainer)
				SetValue(TemplatesContainerProperty, value)
			End Set
		End Property
		Public Property ColumnDefinitions() As ColumnDefinitions
			Get
				Return CType(GetValue(ColumnDefinitionsProperty), ColumnDefinitions)
			End Get
			Set(ByVal value As ColumnDefinitions)
				SetValue(ColumnDefinitionsProperty, value)
			End Set
		End Property
		Public Property RowDefinitions() As RowDefinitions
			Get
				Return CType(GetValue(RowDefinitionsProperty), RowDefinitions)
			End Get
			Set(ByVal value As RowDefinitions)
				SetValue(RowDefinitionsProperty, value)
			End Set
		End Property
		Public Sub New()
#If (Not SILVERLIGHT) Then
			TemplatesContainer = New TemplatesContainer()
#End If
			ColumnDefinitions = New ColumnDefinitions()
			RowDefinitions = New RowDefinitions()
		End Sub
		<Browsable(False)> _
		Public Function GetColumnHeaderMargin(ByVal column As ColumnBase) As Thickness
			Dim res As New Thickness()
			If GetIsBottomColumn(column) Then
				res = TemplatesContainer.GetBottomColumnHeaderIndent()
			ElseIf GetIsTopColumn(column) Then
				res = TemplatesContainer.GetTopColumnHeaderIndent()
			Else
				res = TemplatesContainer.GetMiddleColumnHeaderIndent()
			End If
			Return res
		End Function
		<Browsable(False)> _
		Public Sub UpdateColumnHeaderTemplate(ByVal column As ColumnBase)
			If GetIsBand(column) Then
				column.HeaderTemplate = TemplatesContainer.BandColumnHeaderTemplate
			End If
		End Sub
		Protected Overrides Sub OnAttached()
			If TemplatesContainer Is Nothing Then
				Throw New InvalidOperationException("The TemplatesContainer property cannot be null, please set it in the xaml.")
			End If
			MyBase.OnAttached()
			AssociatedObject.LayoutCalculatorFactory = New BandedViewLayoutCalculatorFactory()
			AssociatedObject.HeaderTemplate = TemplatesContainer.GridHeadersTemplate
			AssociatedObject.DataRowTemplate = TemplatesContainer.GridDataRowTemplate
			AssociatedObject.ShowGroupedColumns = True
			AssociatedObject.AutoWidth = True
			AssociatedObject.AllowMoveColumnToDropArea = False
			AddHandler AssociatedObject.ShowGridMenu, AddressOf OnShowGridMenu
		End Sub
		Protected Overrides Sub OnDetaching()
			RemoveHandler AssociatedObject.ShowGridMenu, AddressOf OnShowGridMenu
			MyBase.OnDetaching()
		End Sub

		Private Sub OnShowGridMenu(ByVal sender As Object, ByVal e As GridMenuEventArgs)
			Dim showColumnChooserBarItem As BarItem = Nothing
			For Each barItem As BarItem In e.Items
				If barItem.Name = "ItemColumnChooser" OrElse (barItem.Content IsNot Nothing AndAlso barItem.Content.ToString() = "Show Column Chooser") Then
					showColumnChooserBarItem = barItem
				End If
			Next barItem
			If showColumnChooserBarItem IsNot Nothing Then
				showColumnChooserBarItem.IsVisible = False
			End If
		End Sub
	End Class
End Namespace