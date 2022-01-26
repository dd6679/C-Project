using System.Collections.ObjectModel;

namespace Binding1
{
    class NamedAge
    {
        public string NameForAge { get; set; }
        public int AgeId { get; set; }
    }

    class NamedAges : ObservableCollection<NamedAge> { }
}
