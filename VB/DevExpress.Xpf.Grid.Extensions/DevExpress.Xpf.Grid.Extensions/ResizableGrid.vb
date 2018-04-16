Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports DevExpress.Utils
Imports StdColumnDefinition = System.Windows.Controls.ColumnDefinition
Imports StdGrid = System.Windows.Controls.Grid
Imports DevExpress.Xpf.Grid
Namespace BandedViewExtension
    Public Class ResizableGrid
        Inherits StdGrid
#If SILVERLIGHT Then
		Private privateIsLoaded As Boolean
		Public Property IsLoaded() As Boolean
			Get
				Return privateIsLoaded
			End Get
			Private Set(ByVal value As Boolean)
				privateIsLoaded = value
			End Set
		End Property
#End If
        Public BandBehavior As BandedViewBehavior
        Public View As TableView
        Public Owner As ColumnsLayoutControl
        Private ReadOnly Property CorrectingLeftAreaWidth() As Double
            Get
                Return CorrectingColumn.Width.Value
            End Get
        End Property
        Private CorrectingColumn As StdColumnDefinition
        Public Sub New()
            AddHandler Loaded, AddressOf OnLoaded
            AddHandler Unloaded, AddressOf OnUnloaded
        End Sub
        Public Sub Initialize(ByVal owner As ColumnsLayoutControl)
            Me.Owner = owner
            BandBehavior = Me.Owner.BandBehavior
            View = CType(Me.Owner.View, TableView)
        End Sub
        Public Sub PreparePanel()
            If ColumnDefinitions.Count <> 0 OrElse RowDefinitions.Count <> 0 Then
                Return
            End If
            CreateCorrectingColumn()
            For Each columnDefinition As ColumnDefinition In BandBehavior.ColumnDefinitions
                ColumnDefinitions.Add(columnDefinition.CreateGridColumnDefinition())
            Next columnDefinition
            For Each rowDefinition As RowDefinition In BandBehavior.RowDefinitions
                RowDefinitions.Add(rowDefinition.CreateGridRowDefinition())
            Next rowDefinition
            AddHandler LayoutUpdated, AddressOf OnLayoutUpdated
        End Sub
        Public Sub UpdateColumns()
            PrepareColumns(New Rows(Me))
        End Sub
        Private Sub OnLayoutUpdated(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsLoaded) Then
                Return
            End If
            RemoveHandler LayoutUpdated, AddressOf OnLayoutUpdated
            Dim rows As New Rows(Me)
            PrepareColumns(rows)
            CorrectColumnsWidth(rows)
        End Sub
        Public Sub ClearPanel()
            ColumnDefinitions.Clear()
            RowDefinitions.Clear()
        End Sub
        Private Sub CreateCorrectingColumn()
            CorrectingColumn = New StdColumnDefinition()
            Dim b As New Binding("GroupedColumns.Count")
            b.Source = View
            b.Converter = New GridLengthValueConverter()
            b.ConverterParameter = (CType(View, TableView)).LeftGroupAreaIndent
            BindingOperations.SetBinding(CorrectingColumn, System.Windows.Controls.ColumnDefinition.WidthProperty, b)
            ColumnDefinitions.Insert(0, CorrectingColumn)
        End Sub
        Private Sub OnLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
#If SILVERLIGHT Then
			IsLoaded = True
#End If
        End Sub
        Private Sub OnUnloaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
#If SILVERLIGHT Then
			IsLoaded = False
#End If
        End Sub
        Private Sub PrepareColumns(ByVal rows As Rows)
            Dim counter As Integer = 0
            For i As Integer = 0 To rows.Count - 1
                Dim row As Row = rows(i)
                For j As Integer = 0 To row.Count - 1
                    Dim rowCell As RowCell = row(j)
                    Canvas.SetZIndex(rowCell.GridColumnPresenter, counter)
                    rowCell.GridColumn.VisibleIndex = counter
                    counter += 1
                Next j
            Next i
        End Sub
        Private Sub CorrectColumnsWidth(ByVal rows As Rows)
            For i As Integer = 0 To rows.Count - 1
                Dim row As Row = rows(i)
                For j As Integer = 0 To row.Count - 1
                    Dim rowCell As RowCell = row(j)
                    rowCell.ColumnWidth = New GridLength(rowCell.ActualWidth / row.RowWidth, GridUnitType.Star)
                Next j
            Next i
        End Sub

        Public Sub Resize(ByVal gridColumn As ColumnBase, ByVal diff As Double)
            Dim rows As New Rows(Me)
            CorrectColumnsWidth(rows)
            If diff = 0.0R OrElse Double.IsNaN(diff) Then
                Return
            End If
            Dim columnIndex As Integer = BandedViewBehavior.GetColumn(gridColumn)
            Dim rowIndex As Integer = BandedViewBehavior.GetRow(gridColumn)
            Dim row As Row = rows(rowIndex)
            row.SelectColumn(columnIndex)
            If diff > 0 Then
                DecreaseColumnsWidth(row, row.SelectedIndex + 1, diff)
                IncreaseSelectedColumnWidth(row, diff)
            End If
            If diff < 0 Then
                diff *= -1
                If Math.Abs(row.SelectedCell.ActualWidth - row.SelectedCell.ColumnMinWidth) <= 0.5 Then
                    Return
                End If
                DecreaseSelectedColumnWidth(row, diff)
                IncreaseColumnsWidth(row, row.SelectedIndex + 1, diff)
            End If
            row.Foreach(row.SelectedIndex, Function(rowCell) AnonymousMethod1(rowCell, row))
        End Sub

        Private Function AnonymousMethod1(ByVal rowCell As RowCell, ByVal row As Row) As Boolean
            Dim res As Double = rowCell.NewWidth / row.RowWidth
            rowCell.ColumnWidth = New GridLength(res, GridUnitType.Star)
            Return True
        End Function

        Private Sub IncreaseSelectedColumnWidth(ByVal row As Row, ByVal diff As Double)
            row.SelectedCell.NewWidth = row.SelectedCell.ActualWidth + diff
        End Sub
        Private Sub DecreaseSelectedColumnWidth(ByVal row As Row, ByRef diff As Double)
            If row.SelectedCell.ActualWidth - diff < row.SelectedCell.ColumnMinWidth Then
                diff = row.SelectedCell.ActualWidth - row.SelectedCell.ColumnMinWidth
                row.SelectedCell.NewWidth = row.SelectedCell.ColumnMinWidth
                Return
            End If
            row.SelectedCell.NewWidth = row.SelectedCell.ActualWidth - diff
        End Sub

        Private Sub IncreaseColumnsWidth(ByVal row As Row, ByVal startIndex As Integer, ByVal diff As Double)
            Dim one As Double = 0.0R
            row.Foreach(startIndex, Function(rowCell) AnonymousMethod2(rowCell, one))
            row.Foreach(startIndex, Function(rowCell) AnonymousMethod3(rowCell, one, diff))
        End Sub

        Private Function AnonymousMethod2(ByVal rowCell As RowCell, ByVal one As Double) As Boolean
            one += rowCell.ColumnWidth.Value
            Return True
        End Function

        Private Function AnonymousMethod3(ByVal rowCell As RowCell, ByVal one As Double, ByVal diff As Double) As Boolean
            Dim coef As Double = rowCell.ColumnWidth.Value / one
            Dim cDiff As Double = diff * coef
            rowCell.NewWidth = rowCell.ActualWidth + cDiff
            Return True
        End Function
        Private Sub DecreaseColumnsWidth(ByVal row As Row, ByVal startIndex As Integer, ByRef value As Double)
            Dim allWidth As Double = 0.0R
            Dim allMinWidth As Double = 0.0R
            row.Foreach(startIndex, Function(rowCell) AnonymousMethod4(rowCell, allWidth, allMinWidth))
            If allWidth - value < allMinWidth Then
                value = allWidth - allMinWidth
            End If
            row.Foreach(startIndex, Function(rowCell) AnonymousMethod5(rowCell))
            If value = 0.0R Then
                Return
            End If
            Dim one As Double = 0.0R
            row.Foreach(startIndex, Function(rowCell) AnonymousMethod6(rowCell, one))
            If one = 0.0R Then
                Return
            End If
            Dim one2 As Double = 0.0R
            Dim diff As Double = value
            Dim diff2 As Double = 0.0R
            Dim i As Integer = 0
            Do
                i += 1
                row.Foreach(startIndex, Function(rowCell) AnonymousMethod7(rowCell, one, diff, diff2, one2))
                If diff2 = 0.0R OrElse one2 = 0.0R Then
                    Exit Do
                End If
                diff = diff2
                one -= one2
                diff2 = 0.0R
                one2 = 0.0R
            Loop
        End Sub

        Private Function AnonymousMethod4(ByVal rowCell As RowCell, ByVal allWidth As Double, ByVal allMinWidth As Double) As Boolean
            allWidth += rowCell.ActualWidth
            allMinWidth += rowCell.ColumnMinWidth
            Return True
        End Function

        Private Function AnonymousMethod5(ByVal rowCell As RowCell) As Boolean
            rowCell.NewWidth = rowCell.ActualWidth
            Return True
        End Function

        Private Function AnonymousMethod6(ByVal rowCell As RowCell, ByVal one As Double) As Boolean
            If rowCell.NewWidth > rowCell.ColumnMinWidth Then
                one += rowCell.ColumnWidth.Value
            End If
            Return True
        End Function

        'INSTANT VB TODO TASK: The return type of this anonymous method could not be determined by Instant VB:
        Private Function AnonymousMethod7(ByVal rowCell As RowCell, ByVal one As Double, ByVal diff As Double, ByVal diff2 As Double, ByVal one2 As Double) As Object
            Dim coef As Double = rowCell.ColumnWidth.Value / one
            Dim cDiff As Double = diff * coef
            rowCell.NewWidth = rowCell.NewWidth - cDiff
            If rowCell.NewWidth <= rowCell.ColumnMinWidth Then
                rowCell.NewWidth = rowCell.ColumnMinWidth
                diff2 += rowCell.ColumnMinWidth - rowCell.NewWidth
                one2 += rowCell.ColumnWidth.Value
            End If
            Return True
        End Function

#Region "Helpers"
        Private Class HelperBase
            Protected ReadOnly Owner As ResizableGrid
            Public Sub New(ByVal owner As ResizableGrid)
                Me.Owner = owner
            End Sub
        End Class

        Private Class RowCell
            Inherits HelperBase
            Public ReadOnly Row As Integer
            Public ReadOnly RowSpan As Integer
            Public ReadOnly Column As Integer
            Public ReadOnly ColumnSpan As Integer
            Public ReadOnly GridColumn As ColumnBase
            Public ReadOnly ColumnDefinitions As ColumnDefinitions
            Public ReadOnly GridColumnPresenter As ContentPresenter
            Public ReadOnly Property ActualWidth() As Double
                Get
                    Dim res As Double = GridColumnPresenter.ActualWidth
                    If Column = 0 Then
                        res -= Owner.CorrectingLeftAreaWidth
                    End If
                    Return res
                End Get
            End Property
            Public Property ColumnWidth() As GridLength
                Get
                    Return GetColumnWidth()
                End Get
                Set(ByVal value As GridLength)
                    SetColumnWidth(value)
                End Set
            End Property
            Public ReadOnly Property ColumnMinWidth() As Double
                Get
                    Return GetColumnMinWidth()
                End Get
            End Property
            Private privateNewWidth As Double
            Public Property NewWidth() As Double
                Get
                    Return privateNewWidth
                End Get
                Set(ByVal value As Double)
                    privateNewWidth = value
                End Set
            End Property
            Public ReadOnly Property NewWidthIsMinWidth() As Boolean
                Get
                    Return NewWidth = ColumnMinWidth
                End Get
            End Property

            Public Sub New(ByVal owner As ResizableGrid, ByVal gridColumn As ColumnBase, ByVal contentPresenter As ContentPresenter)
                MyBase.New(owner)
                GridColumnPresenter = contentPresenter
                Me.GridColumn = gridColumn
                Row = BandedViewBehavior.GetRow(Me.GridColumn)
                RowSpan = BandedViewBehavior.GetRowSpan(Me.GridColumn)
                Column = BandedViewBehavior.GetColumn(Me.GridColumn)
                ColumnSpan = BandedViewBehavior.GetColumnSpan(Me.GridColumn)
                ColumnDefinitions = New ColumnDefinitions()
                For i As Integer = Column To Column + ColumnSpan - 1
                    ColumnDefinitions.Add(Me.Owner.BandBehavior.ColumnDefinitions(i))
                Next i
            End Sub

            Private Function GetColumnMinWidth() As Double
                Dim res As Double = 0.0R
                For Each column As ColumnDefinition In ColumnDefinitions
                    res += column.MinWidth
                Next column
                Return Math.Max(res, GridColumn.MinWidth)
            End Function
            Private Function GetColumnWidth() As GridLength
                Dim res As Double = 0.0R
                For Each column As ColumnDefinition In ColumnDefinitions
                    res += column.Width.Value
                Next column
                Return New GridLength(res, GridUnitType.Star)
            End Function
            Private Sub SetColumnWidth(ByVal value As GridLength)
                If NewWidthIsMinWidth Then
                    Dim w As Double = value.Value / ColumnDefinitions.Count
                    For Each column As ColumnDefinition In ColumnDefinitions
                        column.Width = New GridLength(w, GridUnitType.Star)
                    Next column
                    Return
                End If
                Dim oldWidth As Double = ColumnWidth.Value
                Dim newWidth As Double = value.Value
                For Each column As ColumnDefinition In ColumnDefinitions
                    Dim oldColumnWidth As Double = column.Width.Value
                    Dim newColumnWidth As Double = newWidth * oldColumnWidth / oldWidth
                    If Double.IsNaN(newColumnWidth) Then
                        newColumnWidth = 0.0R
                    End If
                    column.Width = New GridLength(newColumnWidth, GridUnitType.Star)
                Next column
            End Sub
        End Class
        Private Class Row
            Inherits HelperBase
            Default Public ReadOnly Property Item(ByVal index As Integer) As RowCell
                Get
                    Return RowCellList(index)
                End Get
            End Property
            Public ReadOnly Property Count() As Integer
                Get
                    Return RowCellList.Count
                End Get
            End Property
            Public ReadOnly Property RowWidth() As Double
                Get
                    Dim res As Double = 0.0R
                    For i As Integer = 0 To Count - 1
                        res += Me(i).ActualWidth
                    Next i
                    Return res - Owner.CorrectingLeftAreaWidth
                End Get
            End Property
            Public ReadOnly Property SelectedCell() As RowCell
                Get
                    Return Me(SelectedIndex)
                End Get
            End Property
            Private privateSelectedIndex As Integer
            Public Property SelectedIndex() As Integer
                Get
                    Return privateSelectedIndex
                End Get
                Private Set(ByVal value As Integer)
                    privateSelectedIndex = value
                End Set
            End Property
            Public Sub New(ByVal owner As ResizableGrid, ByVal rowIndex As Integer)
                MyBase.New(owner)
                RowCellList = New List(Of RowCell)()
                For Each cp As ContentPresenter In Me.Owner.Children
                    Dim gridColumnData As GridColumnData = CType(cp.Content, GridColumnData)
                    Dim gridColumn As ColumnBase = gridColumnData.Column

                    Dim row As Integer = BandedViewBehavior.GetRow(gridColumn)
                    Dim rowSpan As Integer = BandedViewBehavior.GetRowSpan(gridColumn)
                    If row <= rowIndex AndAlso row + rowSpan > rowIndex Then
                        RowCellList.Add(New RowCell(Me.Owner, gridColumn, cp))
                    End If

                    Dim sortingMethos As Comparison(Of RowCell) = Function(rowCell1, rowCell2) AnonymousMethod8(rowCell1, rowCell2)
                    RowCellList.Sort(sortingMethos)
                Next cp
            End Sub

            'INSTANT VB TODO TASK: The return type of this anonymous method could not be determined by Instant VB:
            Private Function AnonymousMethod8(ByVal rowCell1 As RowCell, ByVal rowCell2 As RowCell) As Integer
                If rowCell1.Column = rowCell2.Column Then
                    Return 0
                End If
                Return If(rowCell1.Column < rowCell2.Column, -1, 1)
            End Function
            Public Sub SelectColumn(ByVal columnIndex As Integer)
                For i As Integer = 0 To Count - 1
                    If Me(i).Column = columnIndex Then
                        SelectedIndex = i
                        Return
                    End If
                Next i
            End Sub
            Public Sub Foreach(ByVal startIndex As Integer, ByVal action As Action(Of RowCell))
                For i As Integer = startIndex To Count - 1
                    action(Me(i))
                Next i
            End Sub
            Public Sub Foreach(ByVal startIndex As Integer, ByVal count As Integer, ByVal action As Action(Of RowCell))
                For i As Integer = startIndex To startIndex + count - 1
                    action(Me(i))
                Next i
            End Sub

            Private ReadOnly RowCellList As List(Of RowCell)
        End Class
        Private Class Rows
            Inherits HelperBase
            Default Public ReadOnly Property Item(ByVal index As Integer) As Row
                Get
                    Return RowList(index)
                End Get
            End Property
            Public ReadOnly Property Count() As Integer
                Get
                    Return RowList.Count
                End Get
            End Property
            Public Sub New(ByVal owner As ResizableGrid)
                MyBase.New(owner)
                RowList = New List(Of Row)()
                For i As Integer = 0 To Me.Owner.BandBehavior.RowDefinitions.Count - 1
                    RowList.Add(New Row(Me.Owner, i))
                Next i
                'UpdateAllowResizingPropertyForEachRowCell();
            End Sub

            Private Sub UpdateAllowResizingPropertyForEachRowCell()
            End Sub
            Private RowList As List(Of Row)
        End Class
#End Region
    End Class
End Namespace