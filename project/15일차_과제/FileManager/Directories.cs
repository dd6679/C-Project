using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class Directories
    {
        public string DirectoryName { get; set; }
        public string DirectoryPath { get; set; }
    }

    class DirectoriesList : ObservableCollection<Directories> { }
}
