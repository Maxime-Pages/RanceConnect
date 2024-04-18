using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RanceConnect
{
    public class Log
    {
        private string name;
        DateTime dateAdded;
        public string Name { get => name; set => name = value; }
        public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }

        public Log(string name, DateTime dateAdded)
        {
            Name = name;
            DateAdded = dateAdded;
        }

         public override string ToString()
        {
            return $"Name: {Name}, DateAdded: {DateAdded}";
        }
    }
}
