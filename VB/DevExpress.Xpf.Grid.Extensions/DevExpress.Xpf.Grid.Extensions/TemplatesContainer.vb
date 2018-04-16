Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports DevExpress.Xpf.Core

Namespace BandedViewExtension
	Public Class TemplatesContainer
		Inherits Control
		Public Shared ReadOnly GridDataRowTemplateProperty As DependencyProperty = DependencyProperty.Register("GridDataRowTemplate", GetType(DataTemplate), GetType(TemplatesContainer), Nothing)
		Public Shared ReadOnly GridHeadersTemplateProperty As DependencyProperty = DependencyProperty.Register("GridHeadersTemplate", GetType(DataTemplate), GetType(TemplatesContainer), Nothing)
		Public Shared ReadOnly ColumnHeaderResourcesProperty As DependencyProperty = DependencyProperty.Register("ColumnHeaderResources", GetType(ResourceDictionary), GetType(TemplatesContainer), Nothing)
		Public Shared ReadOnly BandColumnHeaderTemplateProperty As DependencyProperty = DependencyProperty.Register("BandColumnHeaderTemplate", GetType(DataTemplate), GetType(TemplatesContainer), Nothing)
		Public Property GridDataRowTemplate() As DataTemplate
			Get
				Return CType(GetValue(GridDataRowTemplateProperty), DataTemplate)
			End Get
			Set(ByVal value As DataTemplate)
				SetValue(GridDataRowTemplateProperty, value)
			End Set
		End Property
		Public Property GridHeadersTemplate() As DataTemplate
			Get
				Return CType(GetValue(GridHeadersTemplateProperty), DataTemplate)
			End Get
			Set(ByVal value As DataTemplate)
				SetValue(GridHeadersTemplateProperty, value)
			End Set
		End Property
		Public Property BandColumnHeaderTemplate() As DataTemplate
			Get
				Return CType(GetValue(BandColumnHeaderTemplateProperty), DataTemplate)
			End Get
			Set(ByVal value As DataTemplate)
				SetValue(BandColumnHeaderTemplateProperty, value)
			End Set
		End Property
		<Browsable(False)> _
		Public Property ColumnHeaderResources() As ResourceDictionary
			Get
				Return CType(GetValue(ColumnHeaderResourcesProperty), ResourceDictionary)
			End Get
			Set(ByVal value As ResourceDictionary)
				SetValue(ColumnHeaderResourcesProperty, value)
			End Set
		End Property
		Public Sub New()
			DefaultStyleKey = GetType(TemplatesContainer)
		End Sub
		<Browsable(False)> _
		Public Function GetTopColumnHeaderIndent() As Thickness
			Return GetColumnHeaderIndentThemedResource("TopRowColumnIndent")
		End Function
		<Browsable(False)> _
		Public Function GetMiddleColumnHeaderIndent() As Thickness
			Return GetColumnHeaderIndentThemedResource("MiddleRowColumnIndent")
		End Function
		<Browsable(False)> _
		Public Function GetBottomColumnHeaderIndent() As Thickness
			Return GetColumnHeaderIndentThemedResource("BottomRowColumnIndent")
		End Function
		Private Function GetColumnHeaderIndentThemedResource(ByVal resourceName As String) As Thickness
			Return CType(If(GetColumnHeaderThemedResource(resourceName), New Thickness()), Thickness)
		End Function
		Private Function GetColumnHeaderThemedResource(ByVal resourceName As String) As Object
			Dim themeName As String = ThemeManager.ApplicationThemeName
#If SILVERLIGHT Then
			themeName = ThemeManager.GetThemeName(Me)
			If String.IsNullOrEmpty(themeName) Then
				themeName = "LightGray"
			End If
#Else
			If ThemeManager.GetTreeWalker(Me) IsNot Nothing Then
				themeName = ThemeManager.GetTreeWalker(Me).ThemeName
			End If
			If String.IsNullOrEmpty(themeName) Then
				themeName = "DeepBlue"
			End If
#End If
			Dim themedResourceName As String = resourceName & themeName
			If ColumnHeaderResources Is Nothing OrElse (Not ColumnHeaderResources.Contains(themedResourceName)) Then
				Return Nothing
			End If
			Return ColumnHeaderResources(themedResourceName)
		End Function
	End Class
End Namespace
