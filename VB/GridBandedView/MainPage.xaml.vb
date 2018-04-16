Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports DevExpress.Xpf.Core
Imports System.IO
Imports DevExpress.Xpf.Utils
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Collections
Imports DevExpress.Xpf.Core.Native
#If SILVERLIGHT Then
Imports DevExpress.Xpf.Theming
#Else
Imports System.Data
Imports System.IO.Compression
#End If

Namespace GridBandedView
	Partial Public Class MainPage
		Inherits UserControl
		Public Sub New()
			ThemeManager.ApplicationThemeName = "Seven"
			InitializeComponent()
			advBandedView.DataContext = CarsData.DataSource
			simpleBandedView.DataContext = CarsData.DataSource
		End Sub
		Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			ThemeManager.ApplicationThemeName = (CType(sender, Button)).Content.ToString()
		End Sub
	End Class
End Namespace
