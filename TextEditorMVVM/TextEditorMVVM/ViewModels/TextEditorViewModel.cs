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

        public ICommand CloseFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand SwitchCommand { get; }
        public ICommand SelectFileCommand { get; }
        public INavigation Navigation { get; set; }

        private Models.TextEditor _textEditor;
        private bool _needSave = false;
        private string _closeFileString = "Закрытие файла ";
        private string _OKstring = "OK";
        public TextEditorViewModel()
        {
            _textEditor = new Models.TextEditor();
            CloseFileCommand = new Command(CloseFile);
            SaveFileCommand = new Command(SaveFile);
            SwitchCommand = new Command(Switch);
            SelectFileCommand = new Command(Select);
            IsReadOnly = "False";
            MessagingCenter.Subscribe<Application, string>(Application.Current, "SelectItem", (sender, arg) =>
            {
                FileName = arg;
                OpenFile();
                Debug.WriteLine(arg);
            });
        }
        
        public string IsReadOnly
        {
            get { return _textEditor.IsReadOnly; }
            set
            {
                if (_textEditor.IsReadOnly != value)
                {
                    _textEditor.IsReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }
            }
        }

        public string Text
        {
            get { return _textEditor.Text; }
            set
            {
                if (_textEditor.Text != value)
                {
                    _textEditor.Text = value;
                    OnPropertyChanged("Text");
                    Debug.WriteLine("Change Text...");
                }
            }
        }
        public string FileName
        {
            get { return _textEditor.FileName; }
            set
            {
                if (_textEditor.FileName != value)
                {
                    _textEditor.FileName = value;
                    OnPropertyChanged("FileName");
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
            if (_textEditor.IsExist(FileName))
            {
                Text = _textEditor.GetText(FileName);
                OnPropertyChanged("Text");
            }
            else
            {

                Debug.WriteLine(FileName + " not exist");
            }
        }
        public bool NeedSaveFile
        {
            get { return _needSave; }
            set
            {
                _needSave = value;
                OnPropertyChanged("NeedSaveFile");
            }
        }
        public async void CloseFile()
        {
            if (_needSave)
            {
                bool resultCloseFile = await Application.Current.MainPage.DisplayAlert("Закрыть файл " + FileName, "Не сохранены изменения в файле. Сохранить? ", "Да", "Нет");
                if (resultCloseFile)
                {
                    SaveFile();
                }
                else
                {
                    if (Text != null || Text != "")
                    {
                        Text = null;
                    }
                }
            }

        }
        public async void SaveFile()
        {
            if (FileName != null)
            {
                if (_textEditor.IsExist(FileName))
                {
                    bool resulatOverwrittenText = await Application.Current.MainPage.DisplayAlert(_closeFileString, FileName + " уже существует. Перезаписать файл?", "Да", "Нет");
                    if (resulatOverwrittenText)
                    {
                        _textEditor.SaveText(Text, FileName);
                        await Application.Current.MainPage.DisplayAlert(_closeFileString, FileName + " перезаписан", _OKstring);
                        NeedSaveFile = false;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(_closeFileString, FileName + " не перезаписан", _OKstring);
                    }
                }
                else
                {
                    _textEditor.SaveText(Text, FileName);
                    await Application.Current.MainPage.DisplayAlert(_closeFileString, FileName + " сохранен", _OKstring);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(_closeFileString, "Введено неккоретное имя файла: " + FileName, _OKstring);
            }
        }

        public int SymbolsCount
        {
            get { return _textEditor.GetSymbolsCount(_textEditor.Text); }
            set
            {
                if (_textEditor.SymbolsCount != value)
                {
                    _textEditor.SymbolsCount = value;
                    OnPropertyChanged("SymbolsCount");
                }
            }
        }

        public int WordsCount
        {
            get { return _textEditor.GetWordsCount(_textEditor.Text); }
            set
            {
                if (_textEditor.WordsCount != value)
                {
                    _textEditor.WordsCount = value;
                    OnPropertyChanged("WordsCount");
                }
            }
        }

        public async void Select()
        {
            await Navigation.PushModalAsync(new SelectFile());
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                if (propName == "Text")
                {
                    NeedSaveFile = true;
                    PropertyChanged(this, new PropertyChangedEventArgs("SymbolsCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("WordsCount"));
                }
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
