using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using TextEditorMVVM.Models;
using TextEditorMVVM.Classes;
using Xamarin.Forms;

namespace TextEditorMVVM.ViewModels
{
    public class TextEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OpenFileCommand { get; }
        public ICommand CloseFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand SwitchCommand { get; }

        private FileManager fileManager;

        public TextEditorViewModel()
        {
            fileManager = new FileManager();
            OpenFileCommand = new Command(OpenFile);
            CloseFileCommand = new Command(CloseFile);
            SaveFileCommand = new Command(SaveFile);
            SwitchCommand = new Command(Switch);
        }

        public string IsReadOnly
        {
            get { return fileManager.IsReadOnly; }
            set
            {
                if (fileManager.IsReadOnly != value)
                {
                    fileManager.IsReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }
            }
        }

        private void Switch()
        {
            if (IsReadOnly == "False")
            {
                IsReadOnly = "True";
            }
            else
            {
                IsReadOnly = "False";
            }
        }

        public void OpenFile()
        {
            Debug.WriteLine("Open File");
        }
        public void CloseFile()
        {
            Debug.WriteLine("Close File");
        }
        public void SaveFile()
        {
            Debug.WriteLine("Save File");
        }

        protected void OnPropertyChanged (string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
