Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Data
Namespace BandedViewExtension
	Public Class GridLengthValueConverter
		Implements IValueConverter
		Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
			Dim res As Double = CInt(Fix(value)) * CDbl(parameter)
			Return New GridLength(res)
		End Function
		Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function
	End Class
End Namespace