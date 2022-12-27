Imports System.Collections.ObjectModel
Imports System.Windows

Namespace BandedViewSample

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
            Me.grid.ItemsSource = Vehicles.GetVehicles()
        End Sub
    End Class

    Public Class Vehicle

        Public Property Trademark As String

        Public Property Model As String

        Public Property Modification As String

        Public Property Doors As Integer

        Public Property Cyl As Integer

        Public Property MPGCity As Integer

        Public Property MPGHighway As Integer

        Public Property Transmission As String

        Public Property Gears As Integer

        Public Property Description As String
    End Class

    Public Class Vehicles

        Public Shared Function GetVehicles() As ObservableCollection(Of Vehicle)
            Dim vehicles = New ObservableCollection(Of Vehicle)()
            vehicles.Add(New Vehicle() With {.Trademark = "Honda", .Model = "Crosstour", .Modification = "EX 2WD 2.4L I4 5A", .Doors = 4, .Cyl = 4, .MPGCity = 22, .MPGHighway = 31, .Transmission = "Automatic", .Gears = 5, .Description = "The Crosstour (initially branded the Accord Crosstour) is a full-size crossover SUV manufactured by Japanese automaker Honda. Sales began in November 2009 for the 2010 model year."})
            vehicles.Add(New Vehicle() With {.Trademark = "Ford", .Model = "Edge", .Modification = "SEL FWD 3.5L V6 6A", .Doors = 4, .Cyl = 6, .MPGCity = 19, .MPGHighway = 27, .Transmission = "Automatic", .Gears = 6, .Description = "The Ford Edge is a mid-size crossover SUV (CUV) manufactured by Ford, based on the Ford CD3 platform shared with the Mazda CX-9."})
            Return vehicles
        End Function
    End Class
End Namespace
