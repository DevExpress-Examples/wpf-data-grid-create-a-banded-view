Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Documents

Namespace BandedViewSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
			Dim list As New List(Of TestData)()
			For i As Integer = 0 To 99
				list.Add(New TestData() With {.Id = i, .Text = "Row" & i, .Number = i, .MultiLineText = "Row" & i & "Line0" & Environment.NewLine & "Row" & i & "Line1" & Environment.NewLine & "Row" & i & "Line2"})
			Next i
			DataContext = list
		End Sub
	End Class

	Public Class TestData
		Private privateId As Integer
		Public Property Id() As Integer
			Get
				Return privateId
			End Get
			Set(ByVal value As Integer)
				privateId = value
			End Set
		End Property
		Private privateText As String
		Public Property Text() As String
			Get
				Return privateText
			End Get
			Set(ByVal value As String)
				privateText = value
			End Set
		End Property
		Private privateNumber As Integer
		Public Property Number() As Integer
			Get
				Return privateNumber
			End Get
			Set(ByVal value As Integer)
				privateNumber = value
			End Set
		End Property
		Private privateMultiLineText As String
		Public Property MultiLineText() As String
			Get
				Return privateMultiLineText
			End Get
			Set(ByVal value As String)
				privateMultiLineText = value
			End Set
		End Property
	End Class
End Namespace
