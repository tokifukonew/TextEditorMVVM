using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using TextEditorMVVM.Views;

namespace TextEditorMVVM.ViewModels
{
    public class TextEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Action DisplayMessage;

        public ICommand OpenFileCommand { get; }
        public ICommand CloseFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand SwitchCommand { get; }
        public ICommand SelectFileCommand { get; }
        public INavigation Navigation { get; set; }

        private Models.TextEditor textEditor;

        public TextEditorViewModel()
        {
            textEditor = new Models.TextEditor();
            OpenFileCommand = new Command(OpenFile);
            CloseFileCommand = new Command(CloseFile);
            SaveFileCommand = new Command(SaveFile);
            SwitchCommand = new Command(Switch);
            SelectFileCommand = new Command(Select);
            IsReadOnly = "False";
        }
        
        public string IsReadOnly
        {
            get { return textEditor.IsReadOnly; }
            set
            {
                if (textEditor.IsReadOnly != value)
                {
                    textEditor.IsReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }
            }
        }

        public string Text
        {
            get { return textEditor.Text; }
            set
            {
                if (textEditor.Text != value)
                {
                    textEditor.Text = value;
                    OnPropertyChanged("Text");
                    Debug.WriteLine("Change Text...");
                }
            }
        }
        public string FilePath
        {
            get { return textEditor.FilePath; }
            set
            {
                if (textEditor.FilePath != value)
                {
                    textEditor.FilePath = value;
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
            if (textEditor.IsExist(FilePath))
            {
                Text = textEditor.GetText(FilePath);
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
        public async void SaveFile()
        {
            if (FilePath != null)
            {
                if (textEditor.IsExist(FilePath))
                {
                    bool resulatOverwrittenText = await Application.Current.MainPage.DisplayAlert("Внимание!", FilePath + " уже существует. Перезаписать файл?", "Да", "Нет");
                    if (resulatOverwrittenText)
                    {
                        textEditor.SaveText(Text, FilePath);
                        await Application.Current.MainPage.DisplayAlert("Внимание!", FilePath + " перезаписан", "Ок");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Внимание!", FilePath + " не перезаписан", "Ок");
                    }
                }
                else
                {
                    textEditor.SaveText(Text, FilePath);
                    await Application.Current.MainPage.DisplayAlert("Внимание!", FilePath + " сохранен", "Ок");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Внимание!", "Введено неккоретное имя файла: " + FilePath, "Ок");
            }
        }

        public int SymbolsCount
        {
            get { return textEditor.GetSymbolsCount(textEditor.Text); }
            set
            {
                if (textEditor.SymbolsCount != value)
                {
                    textEditor.SymbolsCount = value;
                    OnPropertyChanged("SymbolsCount");
                }
            }
        }

        public int WordsCount
        {
            get { return textEditor.GetWordsCount(textEditor.Text); }
            set
            {
                if (textEditor.WordsCount != value)
                {
                    textEditor.WordsCount = value;
                    OnPropertyChanged("WordsCount");
                }
            }
        }

        public void Select()
        {
            Debug.WriteLine("Select");
            Navigation.PushAsync(new SelectFile());
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                if (propName == "Text")
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SymbolsCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("WordsCount"));
                }
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
