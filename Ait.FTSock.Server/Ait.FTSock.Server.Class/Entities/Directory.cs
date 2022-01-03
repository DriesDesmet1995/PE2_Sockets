using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ait.FTSock.Server.Class.Entities
{
    public class Directory
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Directory Parent { get; set; }

        public Directory()
        {
        }

        public Directory(string name, string path, Directory parent)
        {
            Name = name;
            Path = path;
            Parent = parent;
        }
    }
}
