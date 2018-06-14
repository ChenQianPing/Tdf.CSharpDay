using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter2
{
    public class Company
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public Company() { }
        public Company(string name, List<User> users)
        {
            Name = name;
            Users = users;
        }
    }
}
