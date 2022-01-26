using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class Files
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    class FilesList : ObservableCollection<Files> { }
}
