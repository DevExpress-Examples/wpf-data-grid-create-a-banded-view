Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpf.Grid
Namespace BandedViewExtension
	Public Class InvalidColumnPropertyValueException
		Inherits InvalidOperationException
		Private privateGridColumn As ColumnBase
		Public Property GridColumn() As ColumnBase
			Get
				Return privateGridColumn
			End Get
			Private Set(ByVal value As ColumnBase)
				privateGridColumn = value
			End Set
		End Property
		Private privateGridColumnPropertyName As String
		Public Property GridColumnPropertyName() As String
			Get
				Return privateGridColumnPropertyName
			End Get
			Private Set(ByVal value As String)
				privateGridColumnPropertyName = value
			End Set
		End Property
		Public Overrides ReadOnly Property Message() As String
			Get
				Return "You cannot set the " & GridColumnPropertyName & "property for the GridColumn when BandedBehavior is used."
			End Get
		End Property
		Public Sub New(ByVal column As ColumnBase, ByVal propertyName As String)
			GridColumn = column
			GridColumnPropertyName = propertyName
		End Sub
	End Class
End Namespace