Imports Microsoft.VisualBasic
Imports DevExpress.Xpf.Grid.Native
Imports System.Collections.Generic
Imports DevExpress.Xpf.Grid
Namespace BandedViewExtension
	Public Class BandedViewColumnsLayoutCalculator
		Inherits AutoWidthColumnsLayoutCalculator
		Public Sub New(ByVal viewInfo As GridViewInfo)
			MyBase.New(viewInfo)
		End Sub
		Public Overrides Sub ApplyResize(ByVal resizeColumn As ColumnBase, ByVal newWidth As Double, ByVal maxWidth As Double, ByVal indentWidth As Double, ByVal correctWidths As Boolean)
			Dim c As ColumnsLayoutControl = BandedViewBehavior.GetColumnsLayoutControl(resizeColumn)
			c.Resize(resizeColumn, newWidth)
		End Sub
		Protected Overrides Sub UpdateHasLeftRightSibling(ByVal columns As IList(Of ColumnBase))
			For i As Integer = 0 To columns.Count - 1
				BandedViewBehavior.GetIsRightColumn(columns(i))
				columns(i).HasRightSibling = Not BandedViewBehavior.GetIsRightColumn(columns(i))
				columns(i).HasLeftSibling = Not BandedViewBehavior.GetIsLeftColumn(columns(i))
			Next i
		End Sub
	End Class
	Public Class BandedViewLayoutCalculatorFactory
		Inherits GridTableViewLayoutCalculatorFactory
		Public Overrides Function CreateCalculator(ByVal viewInfo As GridViewInfo, ByVal autoWidth As Boolean) As ColumnsLayoutCalculator
			Return New BandedViewColumnsLayoutCalculator(viewInfo)
		End Function
	End Class
End Namespace