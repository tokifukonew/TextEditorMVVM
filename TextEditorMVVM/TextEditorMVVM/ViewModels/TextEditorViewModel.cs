using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using TextEditorMVVM.Models;
using TextEditorMVVM.Classes;
using Xamarin.Forms;
using System.Text;

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

        public string Text
        {
            get { return fileManager.Text; }
            set
            {
                if (fileManager.Text != value)
                {
                    fileManager.Text = value;
                    OnPropertyChanged("Text");
                    Debug.WriteLine("Change Text...");
                }
            }
        }
        public string FilePath
        {
            get { return fileManager.FilePath; }
            set
            {
                if (fileManager.FilePath != value)
                {
                    fileManager.FilePath = value;
                    OnPropertyChanged("FilePath");
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
            if (fileManager.IsExist(FilePath))
            {
                Text = fileManager.GetText(FilePath);
                OnPropertyChanged("Text");
            }
            else
            {
                Debug.WriteLine(FilePath + " not exist");
            }
        }
        public void CloseFile()
        {
            if (Text != null || Text != "")
            {
                Text = null;
            }
            else
            {
                Debug.WriteLine("Close File. Incorrect");
            }
        }
        public void SaveFile()
        {
            if (!fileManager.IsExist(FilePath))
            {
                fileManager.SaveText(Text, FilePath);
                OnPropertyChanged("Text");
            }
            else
            {
                fileManager.SaveText(Text, FilePath);
                Debug.WriteLine(FilePath + " owerwritten");
            }
        }

        protected void OnPropertyChanged (string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
