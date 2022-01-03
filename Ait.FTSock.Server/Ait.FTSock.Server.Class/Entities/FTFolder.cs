using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ait.FTSock.Server.Class.Entities
{
    public class FTFolder
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public FTFolder Parent { get; set; }

        public FTFolder()
        {
        }

        public FTFolder(string name, string path, FTFolder parent)
        {
            Name = name;
            Path = path;
            Parent = parent;
        }
    }
}
