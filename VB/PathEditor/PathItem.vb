Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Collections.ObjectModel
Imports System.Windows.Input

Namespace PathEditor
	Public Class PathItem
		Private owner As BreadcrumbControl
		Private _selectedItem As String
		Public Property selectedItem() As String
			Get
				Return _selectedItem
			End Get
			Set(ByVal value As String)
				_selectedItem = value
				If value IsNot Nothing Then
					owner.EditValue = FullPathToFolder & value
				End If
			End Set
		End Property
		Private privateDirs As ObservableCollection(Of String)
		Public Property Dirs() As ObservableCollection(Of String)
			Get
				Return privateDirs
			End Get
			Set(ByVal value As ObservableCollection(Of String))
				privateDirs = value
			End Set
		End Property
		Private privateFolder As String
		Public Property Folder() As String
			Get
				Return privateFolder
			End Get
			Set(ByVal value As String)
				privateFolder = value
			End Set
		End Property
		Private privateFullPathToFolder As String
		Public Property FullPathToFolder() As String
			Get
				Return privateFullPathToFolder
			End Get
			Set(ByVal value As String)
				privateFullPathToFolder = value
			End Set
		End Property
		Public Sub New(ByVal dr As ObservableCollection(Of String), ByVal fl As String, ByVal fp As String, ByVal ow As BreadcrumbControl)
			Dirs = dr
			Folder = fl
			FullPathToFolder = fp
			owner = ow
			clickCommand_Renamed = New FolderButtonClick(ow)
		End Sub

		Private clickCommand_Renamed As FolderButtonClick
		Public ReadOnly Property ClickCommand() As ICommand
			Get
				Return clickCommand_Renamed
			End Get
		End Property
	End Class

	Public Class FolderButtonClick
		Implements ICommand
		Private owner As BreadcrumbControl
		Public Sub New(ByVal ow As BreadcrumbControl)
			owner = ow
		End Sub
		Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
			Return True
		End Function
		Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
			AddHandler(ByVal value As EventHandler)
				AddHandler CommandManager.RequerySuggested, value
			End AddHandler
			RemoveHandler(ByVal value As EventHandler)
				RemoveHandler CommandManager.RequerySuggested, value
			End RemoveHandler
			RaiseEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
			End RaiseEvent
		End Event
		Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
			owner.EditValue = parameter
		End Sub
	End Class
End Namespace
