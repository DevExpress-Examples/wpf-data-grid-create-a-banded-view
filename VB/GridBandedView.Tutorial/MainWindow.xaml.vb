Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes

Namespace GridBandedView.Tutorial
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			DataContext = New DataList()
			InitializeComponent()
		End Sub
	End Class
	Public Class DataList
		Inherits List(Of Data)
		Public Sub New()
			For i As Integer = 0 To 19
				Dim d As New Data() With {.First = "First #" & i.ToString(), .Second = "Second #" & i.ToString()}
				Add(d)
			Next i
		End Sub
	End Class

	Public Class Data
		Private privateFirst As String
		Public Property First() As String
			Get
				Return privateFirst
			End Get
			Set(ByVal value As String)
				privateFirst = value
			End Set
		End Property
		Private privateSecond As String
		Public Property Second() As String
			Get
				Return privateSecond
			End Get
			Set(ByVal value As String)
				privateSecond = value
			End Set
		End Property
	End Class
End Namespace
