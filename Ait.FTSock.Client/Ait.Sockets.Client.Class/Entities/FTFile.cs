using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ait.FTSock.Client.Class.Entities
{
    public class FTFile
    {
        public string Name { get; set; }
        public FTFolder Directory { get; set; }
        public byte Size { get; set; }
        public DateTime Creation { get; set; }

        public FTFile()
        {
        }

        public FTFile(string name, FTFolder directory, byte size, DateTime creation)
        {
            Name = name;
            Directory = directory;
            Size = size;
            Creation = creation;
        }
    }
}
