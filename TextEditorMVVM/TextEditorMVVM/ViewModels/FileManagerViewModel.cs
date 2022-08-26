using System.ComponentModel;
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
        public ICommand CreateCommand { get ; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand RenameCommand { get; set; }
        private Models.FileManager _fileManager;
        private string _textFileExtension = ".txt";
        private string _createNewFileString = "Создание нового файла";
        private string _deleteFileString = "Удаление файла";
        private string _renameFileString = "Переименовать файл ";
        private string _errorString = "Ошибка.";
        private string _OKstring = "OK";
        public ObservableCollection<File> Files { get; set; }

        public FileManagerViewModel()
        {
            _fileManager = new Models.FileManager();
            SelectCommand = new Command(Select);
            RefreshCommand = new Command(Refresh);
            CreateCommand = new Command(Create);
            DeleteCommand = new Command(Delete);
            RenameCommand = new Command(Rename);
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
        private string _file;
        public string File
        {
            get { return _file; }
            set
            {
                _file = value;
                OnPropertyChanged("File");
                Files.Add(new Models.FileManager.File(){ 
                    Value = _file, 
                    Size = _fileManager.GetSizeFile(_file), 
                    Change = _fileManager.GetFileChange(_file) });
            }
        }

        public void Refresh()
        {
            IEnumerable<string> files = _fileManager.GetFilesList();
            foreach (var file in files)
            {
                if(file == "username.txt" || file == "temp.txt")
                {
                    continue;
                }
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
        private async void Select()
        {
            if (SelectItem != null)
            {
                MessagingCenter.Send<Application, string>(Application.Current, "SelectItem", SelectItem.Value);
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Выбор файла", _errorString + " Выберите файл.", _OKstring); ;
            }
        }

        public async void Create()
        {
            string fileName = await Application.Current.MainPage.DisplayPromptAsync(_createNewFileString, "Введите имя нового файла") + _textFileExtension;
            if (!_fileManager.IsExist(fileName) && fileName != _textFileExtension)
            {
                if (fileName != null && _fileManager.IsValidfileName(fileName))
                {
                    bool result = await _fileManager.CreateFile(fileName);
                    if(result)
                    {
                        await Application.Current.MainPage.DisplayAlert(_createNewFileString, "Файл создан " + fileName, _OKstring);
                        File = fileName;
                        SelectItem = Files.First(p => p.Value == fileName);
                        OnPropertyChanged("SelectItem");
                        Refresh();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(_createNewFileString, _errorString + " Файл не создан " + fileName, _OKstring);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(_createNewFileString, _errorString + " Недопустимое имя файла " + fileName, _OKstring);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(_createNewFileString, _errorString + " Файл уже существует или введено нулевое имя.", _OKstring);
            }
        }
        public async void Delete()
        {
            if (SelectItem != null)
            {
                if (_fileManager.IsExist(SelectItem.Value))
                {
                    bool result = await _fileManager.DeleteFile(SelectItem.Value);
                    if (result)
                    {
                        await Application.Current.MainPage.DisplayAlert(_deleteFileString, "Выбранный файл удален " + SelectItem.Value, _OKstring);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(_deleteFileString, _errorString + " Выбранный файл не удален " + SelectItem.Value, _OKstring);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(_deleteFileString, _errorString + " Выбранный файл отсутствует " + SelectItem.Value, _OKstring);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(_renameFileString, _errorString + " Выберите файл.", _OKstring); ;
            }
            Files.Clear();
            Refresh();
        }
        public async void Rename()
        {
            if (SelectItem != null)
            {
                if (_fileManager.IsExist(SelectItem.Value) && (SelectItem.Value != null))
                {
                    string newFileName = await Application.Current.MainPage.DisplayPromptAsync(_renameFileString + SelectItem.Value, "Введите новое имя файла ") + _textFileExtension;
                    if (_fileManager.IsValidfileName(newFileName))
                    {
                        bool result = await _fileManager.RenameFile(SelectItem.Value, newFileName);
                        if (result)
                        {
                            await Application.Current.MainPage.DisplayAlert(_renameFileString + SelectItem.Value, "Файл " + SelectItem.Value + " переименован в " + newFileName, _OKstring);
                            Files.Clear();
                            Refresh();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(_renameFileString + SelectItem.Value, _errorString + " Выбранный файл не переименован " + SelectItem.Value, _OKstring);
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(_renameFileString + SelectItem.Value, _errorString + " Файл не переименован", _OKstring);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(_renameFileString, _errorString + " Выберите файл.", _OKstring);
            }
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
