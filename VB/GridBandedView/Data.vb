Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Media
Imports System.Xml.Serialization
Imports DevExpress.Xpf.Core.Native
Imports DevExpress.Xpf.Utils
#If (Not SILVERLIGHT) Then
Imports System.IO.Compression
#Else
Imports DevExpress.Xpf.Theming
#End If
Namespace GridBandedView
	Public Class Cars
		Private privateID As Integer
		Public Property ID() As Integer
			Get
				Return privateID
			End Get
			Set(ByVal value As Integer)
				privateID = value
			End Set
		End Property
		Private privateTrademark As String
		Public Property Trademark() As String
			Get
				Return privateTrademark
			End Get
			Set(ByVal value As String)
				privateTrademark = value
			End Set
		End Property
		Private privateModel As String
		Public Property Model() As String
			Get
				Return privateModel
			End Get
			Set(ByVal value As String)
				privateModel = value
			End Set
		End Property
		Private privateHP As Integer
		Public Property HP() As Integer
			Get
				Return privateHP
			End Get
			Set(ByVal value As Integer)
				privateHP = value
			End Set
		End Property
		Private privateLiter As Double
		Public Property Liter() As Double
			Get
				Return privateLiter
			End Get
			Set(ByVal value As Double)
				privateLiter = value
			End Set
		End Property
		Private privateCyl As Integer
		Public Property Cyl() As Integer
			Get
				Return privateCyl
			End Get
			Set(ByVal value As Integer)
				privateCyl = value
			End Set
		End Property
		Private privateTransmissSpeedCount As Integer
		Public Property TransmissSpeedCount() As Integer
			Get
				Return privateTransmissSpeedCount
			End Get
			Set(ByVal value As Integer)
				privateTransmissSpeedCount = value
			End Set
		End Property
		Private privateTransmissAutomatic As String
		Public Property TransmissAutomatic() As String
			Get
				Return privateTransmissAutomatic
			End Get
			Set(ByVal value As String)
				privateTransmissAutomatic = value
			End Set
		End Property
		Private privateMPGCity As Integer
		Public Property MPGCity() As Integer
			Get
				Return privateMPGCity
			End Get
			Set(ByVal value As Integer)
				privateMPGCity = value
			End Set
		End Property
		Private privateMPGHighway As Integer
		Public Property MPGHighway() As Integer
			Get
				Return privateMPGHighway
			End Get
			Set(ByVal value As Integer)
				privateMPGHighway = value
			End Set
		End Property
		Private privateCategory As String
		Public Property Category() As String
			Get
				Return privateCategory
			End Get
			Set(ByVal value As String)
				privateCategory = value
			End Set
		End Property
		Private privateDescription As String
		Public Property Description() As String
			Get
				Return privateDescription
			End Get
			Set(ByVal value As String)
				privateDescription = value
			End Set
		End Property
		Private privateHyperlink As String
		Public Property Hyperlink() As String
			Get
				Return privateHyperlink
			End Get
			Set(ByVal value As String)
				privateHyperlink = value
			End Set
		End Property
		Private privatePicture As Byte()
		Public Property Picture() As Byte()
			Get
				Return privatePicture
			End Get
			Set(ByVal value As Byte())
				privatePicture = value
			End Set
		End Property
		Private imageSource_Renamed As ImageSource
		Public ReadOnly Property ImageSource() As ImageSource
			Get
				If imageSource_Renamed Is Nothing Then
					imageSource_Renamed = ImageHelper.CreateImageFromStream(New MemoryStream(Picture))
				End If
				Return imageSource_Renamed
			End Get
		End Property
		Private privatePrice As Decimal
		Public Property Price() As Decimal
			Get
				Return privatePrice
			End Get
			Set(ByVal value As Decimal)
				privatePrice = value
			End Set
		End Property
		Private privateDeliveryDate As DateTime
		Public Property DeliveryDate() As DateTime
			Get
				Return privateDeliveryDate
			End Get
			Set(ByVal value As DateTime)
				privateDeliveryDate = value
			End Set
		End Property
		Private privateIsInStock As Boolean
		Public Property IsInStock() As Boolean
			Get
				Return privateIsInStock
			End Get
			Set(ByVal value As Boolean)
				privateIsInStock = value
			End Set
		End Property
	End Class
	<XmlRoot("NewDataSet")> _
	Public Class CarsData
		Inherits List(Of Cars)
		Public Shared ReadOnly Property DataSource() As IList
			Get
				Dim res As IList = Nothing
				Dim s As New XmlSerializer(GetType(CarsData))
				res = CType(s.Deserialize(ResourceHelper.GetDataStreamFromResources("Cars.xml")), IList)
				Return res
			End Get
		End Property
	End Class
	Public NotInheritable Class ResourceHelper
		Private Sub New()
		End Sub
		Public Shared Function GetDataStreamFromResources(ByVal filename As String) As Stream
			Return AssemblyHelper.GetResourceStream(GetType(ResourceHelper).Assembly, String.Format("Data/{0}", filename), True)
		End Function
	End Class
End Namespace