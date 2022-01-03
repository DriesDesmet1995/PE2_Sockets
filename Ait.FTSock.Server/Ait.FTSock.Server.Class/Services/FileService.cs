using Ait.FTSock.Server.Class.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ait.FTSock.Server.Class.Services
{
    public class FileService
    {
        public List<File> Files { get; private set; }

        public FileService()
        {
            Files = new List<File>();
        }

        public void AddFile(File file)
        {
            bool found = false;
            foreach (File f in Files)
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
            foreach (File file in Files)
            {
                if (file.Name == fileName)
                {
                    Files.Remove(file);
                }
            }
        }
    }
}
