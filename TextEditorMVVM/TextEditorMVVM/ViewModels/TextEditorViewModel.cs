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
        public ICommand ExecuteSwitchCommand { get; }

        private FileManager fileManager;

        public TextEditorViewModel()
        {
            fileManager = new FileManager();
            OpenFileCommand = new Command(OpenFile);
            CloseFileCommand = new Command(CloseFile);
            SaveFileCommand = new Command(SaveFile);
            ExecuteSwitchCommand = new Command(ExecuteSwitch);
        }

        private void ExecuteSwitch()
        {
            Debug.WriteLine("ExecuteSwitch");
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

        public ICommand MySwitchCommand { get; set; }

        protected void OnPropertyChanged (string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
