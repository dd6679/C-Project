using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileManager
{
    public class Directories
    {
        public string DirectoryName { get; set; }
        public string DirectoryPath { get; set; }
        public DirectoriesList Members { get; set; }
    }

    public class DirectoriesList : ObservableCollection<Directories> { }
}
