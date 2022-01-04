using Ait.FTSock.Server.Class.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ait.FTSock.Server.Class.Services
{
    public class FtFileService
    {
        public List<FTFile> Files { get; private set; }

        public FtFileService()
        {
            Files = new List<FTFile>();
        }

        public void AddFile(FTFile file)
        {
            bool found = false;
            foreach (FTFile f in Files)
            {
                if (f.Name == file.Name)
                {
                    found = true;
                    f.Name = file.Name;
                    f.Size = file.Size;
                    f.Creation = file.Creation;
                    f.Directory = file.Directory;

                    break;
                }
            }
            if (!found)
            {
                Files.Add(file);
            }

        }
        public void RemoveFile(string fileName)
        {
            foreach (FTFile file in Files)
            {
                if (file.Name == fileName)
                {
                    Files.Remove(file);
                }
            }
        }
    }
}
