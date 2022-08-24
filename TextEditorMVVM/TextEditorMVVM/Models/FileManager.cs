using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextEditorMVVM.Models
{
    public class FileManager
    {
        public string FileName;
        private string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public IEnumerable<string> GetFilesList()
        {
            return Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
        }
        
        public class File
        {
            public string Value { get; set; }
            public override string ToString()
            {
                return $"{this.Value}";
            }
        }

    }
}
