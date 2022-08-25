using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TextEditorMVVM.Models
{
    public class FileManager
    {
        private string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public IEnumerable<string> GetFilesList()
        {
            return Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
        }

        public string GetSizeFile(string filePath)
        {
            var fileInfo = new FileInfo(Path.Combine(folderPath, filePath));
            string sLen = fileInfo.Length.ToString();
            if (fileInfo.Length >= (1 << 20))
                sLen = string.Format("{0}Mb", (fileInfo.Length / 1024) / 1024);
            else
            if (fileInfo.Length >= (1 << 10))
                string.Format("{0} kb", fileInfo.Length / 1024);
            else
                sLen = string.Format("{0} b", fileInfo.Length);
            return sLen;
        }
        public string GetFileChange(string filePath)
        {
            var fileInfo = new FileInfo(Path.Combine(folderPath, filePath));
            string sChange = fileInfo.LastWriteTime.ToString();
            return sChange;
        }

        public Task<bool> CreateFile(string fileName)
        {
            bool result = false;
            try
            {
                System.IO.File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName)).Close();
                result = true;
            }
            catch (PathTooLongException ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка создания файла", "Слишком длинное имя файла " + ex.Message, "OK");
                result = false;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка создания файла", ex.Message, "OK");
                result = false;
            }
            return Task.FromResult(result);
        }

        public Task<bool> DeleteFile(string fileName)
        {
            bool result = false;
            try
            {
                System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName));
                result = true;
            }
            catch (IOException ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка удаления файла", "Файл используется " + ex.Message, "OK");
                result = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка удаления файла", "Ошибка прав доступа " + ex.Message, "OK");
                result = false; 
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка удаления файла", ex.Message, "OK");
                result = false;
            }
            return Task.FromResult(result);
        }
        public Task<bool> RenameFile(string oldFileName, string newFileName)
        {
            oldFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), oldFileName);
            newFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newFileName);
            bool result = false;
            try
            {
                System.IO.File.Move(oldFileName, newFileName);
                result = true;
            }
            catch (IOException ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Файл уже существует " + ex.Message, "OK");
                result = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка переименовывания файла", "Ошибка прав доступа " + ex.Message, "OK");
                result = false;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
                result = false;
            }
            return Task.FromResult(result);
        }
        public bool IsExist(string filePath)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filePath);
            return System.IO.File.Exists(fileName);
        }

        public bool IsValidfileName(string fileName)
        {
            return !Regex.IsMatch(
                fileName,
                string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))));
        }
        public class File
        {
            public string Value { get; set; }
            public string Size { get; set; }
            public string Change { get; set; }
            public override string ToString()
            {
                return $"{this.Value}";
            }
        }

    }
}
