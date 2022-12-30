Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Documents

Namespace BandedViewSample

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
            Dim list As List(Of TestData) = New List(Of TestData)()
            For i As Integer = 0 To 100 - 1
                list.Add(New TestData() With {.Id = i, .Text = "Row" & i, .Number = i, .MultiLineText = "Row" & i & "Line0" & Environment.NewLine & "Row" & i & "Line1" & Environment.NewLine & "Row" & i & "Line2"})
            Next

            DataContext = list
        End Sub
    End Class

    Public Class TestData

        Public Property Id As Integer

        Public Property Text As String

        Public Property Number As Integer

        Public Property MultiLineText As String
    End Class
End Namespace
