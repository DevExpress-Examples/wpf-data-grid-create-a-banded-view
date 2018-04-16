Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Data
Imports DevExpress.Utils
Imports GridColumnDefinition = System.Windows.Controls.ColumnDefinition
Imports GridRowDefinition = System.Windows.Controls.RowDefinition
Namespace BandedViewExtension
	Public Class ColumnDefinition
		Inherits DependencyObject
		Public Shared ReadOnly WidthProperty As DependencyProperty = DependencyProperty.Register("Width", GetType(GridLength), GetType(ColumnDefinition), Nothing)
		Public Shared ReadOnly MinWidthProperty As DependencyProperty = DependencyProperty.Register("MinWidth", GetType(Double), GetType(ColumnDefinition), New PropertyMetadata(8R))
		Public Property Width() As GridLength
			Get
				Return CType(GetValue(WidthProperty), GridLength)
			End Get
			Set(ByVal value As GridLength)
				SetValue(WidthProperty, value)
			End Set
		End Property
		Public Property MinWidth() As Double
			Get
				Return CDbl(GetValue(MinWidthProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(MinWidthProperty, value)
			End Set
		End Property
		Public Function CreateGridColumnDefinition() As GridColumnDefinition
			Dim column As New GridColumnDefinition() With {.MinWidth = Me.MinWidth}
			BindingOperations.SetBinding(column, GridColumnDefinition.WidthProperty, New Binding("Width") With {.Source = Me})
			Return column
		End Function
	End Class
	Public Class RowDefinition
		Inherits DependencyObject
		Public Shared ReadOnly HeightProperty As DependencyProperty = DependencyProperty.Register("Height", GetType(GridLength), GetType(RowDefinition), Nothing)
		Public Property Height() As GridLength
			Get
				Return CType(GetValue(HeightProperty), GridLength)
			End Get
			Set(ByVal value As GridLength)
				SetValue(HeightProperty, value)
			End Set
		End Property

		Public Function CreateGridRowDefinition() As GridRowDefinition
			Return New GridRowDefinition() With {.Height = Me.Height}
		End Function
	End Class

	Public Class ColumnDefinitions
		Inherits List(Of ColumnDefinition)
	End Class
	Public Class RowDefinitions
		Inherits List(Of RowDefinition)
	End Class
End Namespace
