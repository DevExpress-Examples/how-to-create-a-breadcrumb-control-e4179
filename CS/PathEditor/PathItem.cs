using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PathEditor
{
    public class PathItem
    {
        BreadcrumbControl owner;
        string _selectedItem;
        public string selectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (value != null)
                    owner.EditValue = FullPathToFolder+value;
            }
        }
        public ObservableCollection<string> Dirs
        {
            get;
            set;
        }
        public string Folder
        {
            get;
            set;
        }
        public string FullPathToFolder
        {
            get;
            set;
        }
        public PathItem(ObservableCollection<string> dr, string fl, string fp, BreadcrumbControl ow)
        {
            Dirs = dr;
            Folder = fl;
            FullPathToFolder = fp;
            owner = ow;
            clickCommand = new FolderButtonClick(ow);
        }

        FolderButtonClick clickCommand;
        public ICommand ClickCommand
        {
            get { return clickCommand; }
        }
    }

    public class FolderButtonClick : ICommand
    {
        BreadcrumbControl owner;
        public FolderButtonClick(BreadcrumbControl ow)
        {
            owner = ow;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            owner.EditValue = parameter;
        }
    }
}
