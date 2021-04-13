using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUserFunction
{
    public static class Class1
    {
        public static Uri setCsvSource(string path)
        {
            string filePath = path;
            Uri pathFile = new Uri(filePath, UriKind.Absolute);
            return pathFile;
        }
    }
}
