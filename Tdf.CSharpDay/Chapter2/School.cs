using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter2
{
    public class School
    {
        public int SchId { get; set; }
        public string SchName { get; set; }

        public School() { }
        public School(int id, string name)
        {
            SchId = id;
            SchName = name;
        }
    }
}
