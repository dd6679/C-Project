using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Client.Net.DataStruct
{
    public class DataResult<T>
    {
        public T[] Results { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public long tran { get; set; }
    }
}
