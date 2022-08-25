using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using static TextEditorMVVM.Models.FileManager;
using System.Collections.Generic;
using System.Linq;
using TextEditorMVVM.Views;

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
        private string _file;
        private Models.FileManager _fileManager;
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
        private async void Select()
        {
            if (SelectItem != null)
            {
                MessagingCenter.Send<Application, string>(Application.Current, "SelectItem", SelectItem.Value);
            }
            await App.Current.MainPage.Navigation.PopAsync();
        }

        public async void Create()
        {
            string fileName = await Application.Current.MainPage.DisplayPromptAsync("Создать новый файл", "Введите имя файла") + ".txt";
            if (!_fileManager.IsExist(fileName))
            {
                if (fileName != null && _fileManager.IsValidfileName(fileName))
                {
                    bool result = await _fileManager.CreateFile(fileName);
                    if(result)
                    {
                        await Application.Current.MainPage.DisplayAlert("Файл создан!", "Файл создан " + fileName, "Ок");
                        File = fileName;
                        SelectItem = Files.First(p => p.Value == fileName);
                        OnPropertyChanged("SelectItem");
                        Refresh();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Файл не создан " + fileName, "Ок");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Недопустимое имя файла " + fileName, "Ок");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Файл уже существует " + fileName, "Ок");
            }
        }
        public async void Delete()
        {
            if (_fileManager.IsExist(SelectItem.Value) && (SelectItem.Value != null))
            {
                bool result = await _fileManager.DeleteFile(SelectItem.Value);
                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Удаление", "Выбранный файл удален " + SelectItem.Value, "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Выбранный файл не удален " + SelectItem.Value, "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка удаления файла", "Выбранный файл отсутствует " + SelectItem.Value, "OK");
            }
            Files.Clear();
            Refresh();
        }
        public async void Rename()
        {
            if (_fileManager.IsExist(SelectItem.Value) && (SelectItem.Value != null))
            {
                string newFileName = await Application.Current.MainPage.DisplayPromptAsync("Переименовать " + SelectItem.Value, "Введите новое имя файла ") + ".txt";
                if (_fileManager.IsValidfileName(newFileName))
                {
                    bool result = await _fileManager.RenameFile(SelectItem.Value, newFileName);
                    if (result)
                    {
                        await Application.Current.MainPage.DisplayAlert("Переименовать файл " + SelectItem.Value, "Файл " + SelectItem.Value + " переименован в " + newFileName, "OK");
                        Files.Clear();
                        Refresh();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Выбранный файл не переименован " + SelectItem.Value, "OK");
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Файл не переименован", "OK");
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
