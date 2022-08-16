using System.IO;
using System.Text;

namespace TextEditorMVVM.Models
{
    public class FileManager
    {
        public string Text { get; set; }
        public string FilePath { get; set; }

        public string IsReadOnly { get; set; }
        //public Encoding Encoding { get; set; }
        //private readonly Encoding _defaultEncoding = CodePagesEncodingProvider.Instance.GetEncoding(1251);

        public bool IsExist(string filePath)
        {
            return File.Exists(filePath);
        }

        //public string GetText(string filePath)
        //{
        //    return GetText(filePath, _defaultEncoding);
        //}
        public string GetText(string filePath, Encoding encoding)
        {
            string text = File.ReadAllText(filePath, encoding);
            return text;
        }

        //public void SaveText(string text, string filePath)
        //{
        //    SaveText(text, filePath, _defaultEncoding);
        //}
        public void SaveText(string text, string filePath, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }

        public int GetSymbolsCount(string text)
        {
            return text.Length;
        }
    }
}
