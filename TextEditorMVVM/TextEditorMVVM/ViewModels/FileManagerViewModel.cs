using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using static TextEditorMVVM.Models.FileManager;
using System.Collections.Generic;
using System.Linq;

namespace TextEditorMVVM.ViewModels
{
    public class FileManagerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SelectCommand { get; }
        public ICommand RefreshCommand { get; set; }
        //public IEnumerable<string> FilesList { get; set; }
        private string _file;
        private Models.FileManager _fileManager;
        //public ObservableCollection<File> _files;
        public ObservableCollection<File> Files { get; set; }

        public FileManagerViewModel()
        {
            _fileManager = new Models.FileManager();
            SelectCommand = new Command(Select);
            RefreshCommand = new Command(Refresh);
            Files = new ObservableCollection<File>();
            Refresh();
        }

        private File _selectItem;
        public File SelectItem
        {
            get { return _selectItem; }
            set 
            { 
                _selectItem = value;
                OnPropertyChanged("SelectItem");
            }
        }

        public string File
        {
            get { return _file; }
            set
            {
                _file = value;
                OnPropertyChanged("File");
                Files.Add(new Models.FileManager.File(){ Value = _file });
            }
    }

        public void Refresh()
        {
            Debug.WriteLine("Refresh");
            IEnumerable<string> files = _fileManager.GetFilesList();
            foreach (var file in files)
            {
                if (Files.Count == 0)
                {
                    File = file;
                }
                else if (!Files.Any(p => p.Value == file))
                {
                    File = file;
                }
            }
        }
        private void Select()
        {
            Debug.WriteLine("Select");
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
