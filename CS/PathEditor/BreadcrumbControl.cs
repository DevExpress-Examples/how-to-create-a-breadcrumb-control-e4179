using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Editors;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Security;

namespace PathEditor
{
    public class BreadcrumbControl : TextEdit
    {
        static BreadcrumbControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbControl), new FrameworkPropertyMetadata(typeof(BreadcrumbControl)));
        }
        public BreadcrumbControl()
        {
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(TextEdit.EditValueProperty, typeof(TextEdit));
            descriptor.AddValueChanged(this, EditValChanged);
        }
        void EditValChanged(object sender, EventArgs args)
        {
            BreadcrumbControl editor = sender as BreadcrumbControl;
            PathItems.Clear();
            string valueString = editor.EditValue as string;
            string[] dirsInCurrentPath = valueString.Split('\\');
            string pathString = "";
            for (int i = 0; i < dirsInCurrentPath.Count(); i++)
            {
                if (dirsInCurrentPath[i] == "")
                    break;
                pathString += dirsInCurrentPath[i] + "\\";

                if (!Directory.Exists(pathString))
                {
                    Text = pathString.Substring(0, pathString.Length - dirsInCurrentPath[i].Length-1);
                    break;
                }
                PathItems.Add(new PathItem(GetDirs(pathString, i), dirsInCurrentPath[i], pathString, this));
            }
        }
        ObservableCollection<string> GetDirs(string path, int index)
        {
            ObservableCollection<string> col = new ObservableCollection<string>();
            string[] drs;
            try
            {
                drs = Directory.GetDirectories(path);
            }
            catch (UnauthorizedAccessException)
            {
                col = null;
                return col;
            }
            
            foreach (string s in drs)
            {
                try
                {
                    Directory.GetDirectories(s);
                    string[] dirsInCurrentPath = s.Split('\\');
                    col.Add(dirsInCurrentPath[dirsInCurrentPath.Count()-1]);
                }
                catch (UnauthorizedAccessException)
                { }
            }
            return col;
        }
        ObservableCollection<PathItem> pathItems = new ObservableCollection<PathItem>();
        public ObservableCollection<PathItem> PathItems
        {
            get { return pathItems; }
            set { pathItems = value; }
        }

    }
}
