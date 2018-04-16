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
Imports DevExpress.Xpf.Editors
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Security.Permissions
Imports System.Security

Namespace PathEditor
	Public Class BreadcrumbControl
		Inherits TextEdit
		Shared Sub New()
			DefaultStyleKeyProperty.OverrideMetadata(GetType(BreadcrumbControl), New FrameworkPropertyMetadata(GetType(BreadcrumbControl)))
		End Sub
		Public Sub New()
			Dim descriptor As DependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextEdit.EditValueProperty, GetType(TextEdit))
			descriptor.AddValueChanged(Me, AddressOf EditValChanged)
		End Sub
		Private Sub EditValChanged(ByVal sender As Object, ByVal args As EventArgs)
			Dim editor As BreadcrumbControl = TryCast(sender, BreadcrumbControl)
			PathItems.Clear()
			Dim valueString As String = TryCast(editor.EditValue, String)
			Dim dirsInCurrentPath() As String = valueString.Split("\"c)
			Dim pathString As String = ""
			For i As Integer = 0 To dirsInCurrentPath.Count() - 1
				If dirsInCurrentPath(i) = "" Then
					Exit For
				End If
				pathString &= dirsInCurrentPath(i) & "\"

				If (Not Directory.Exists(pathString)) Then
					Text = pathString.Substring(0, pathString.Length - dirsInCurrentPath(i).Length-1)
					Exit For
				End If
				PathItems.Add(New PathItem(GetDirs(pathString, i), dirsInCurrentPath(i), pathString, Me))
			Next i
		End Sub
		Private Function GetDirs(ByVal path As String, ByVal index As Integer) As ObservableCollection(Of String)
			Dim col As New ObservableCollection(Of String)()
			Dim drs() As String
			Try
				drs = Directory.GetDirectories(path)
			Catch e1 As UnauthorizedAccessException
				col = Nothing
				Return col
			End Try

			For Each s As String In drs
				Try
					Directory.GetDirectories(s)
					Dim dirsInCurrentPath() As String = s.Split("\"c)
					col.Add(dirsInCurrentPath(dirsInCurrentPath.Count()-1))
				Catch e2 As UnauthorizedAccessException
				End Try
			Next s
			Return col
		End Function
		Private pathItems_Renamed As New ObservableCollection(Of PathItem)()
		Public Property PathItems() As ObservableCollection(Of PathItem)
			Get
				Return pathItems_Renamed
			End Get
			Set(ByVal value As ObservableCollection(Of PathItem))
				pathItems_Renamed = value
			End Set
		End Property

	End Class
End Namespace
