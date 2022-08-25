using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TextEditorMVVM.Models
{
    public class TextEditor
    {
        public string Text { get; set; }
        public string FilePath { get; set; }

        public string IsReadOnly { get; set; }
        public int SymbolsCount { get ; set; }
        public int WordsCount { get ; set; }
        private string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        //public Encoding Encoding { get; set; }
        //private readonly Encoding _defaultEncoding = CodePagesEncodingProvider.Instance.GetEncoding(1251);

        public bool IsExist(string filePath)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filePath);
            return File.Exists(fileName);
        }

        public string GetText(string filePath)
        {
            string fileName = Path.Combine(folderPath, filePath);
            return File.ReadAllText(fileName);
        }
        public string GetText(string filePath, Encoding encoding)
        {
            string text = File.ReadAllText(filePath, encoding);
            return text;
        }
        public void SaveText(string text, string filePath)
        {
            string fileName = Path.Combine(folderPath, filePath);
            File.WriteAllText(fileName, text);
        }
        public void SaveText(string text, string filePath, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }

        public int GetSymbolsCount(string text)
        {
            if (text == null)
            {
                return 0;
            }
            else
            {
                return text.Length;
            }
        }

        public int GetWordsCount(string text)
        {
            if (text == null)
            {
                return 0;
            }
            else
            {
                var words = Regex.Matches(text, @"[a-zA-Zа-яА-Я]+");
                return words.Count;
            }
        }
    }
}
