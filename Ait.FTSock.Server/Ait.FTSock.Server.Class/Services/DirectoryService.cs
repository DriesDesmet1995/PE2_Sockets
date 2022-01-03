using Ait.FTSock.Server.Class.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ait.FTSock.Server.Class.Services
{
    public class DirectoryService
    {
        public List<Directory> Directories { get; private set; }

        public DirectoryService()
        {
            Directories = new List<Directory>();
        }

        public void AddPath(Directory directory)
        {
            bool found = false;
            foreach (Directory dir in Directories)
            {
                if (dir.Name == directory.Name)
                {
                    found = true;
                    dir.Name = directory.Name;
                    dir.Path = directory.Path;
                    dir.Parent = directory.Parent;

                    break;
                }
            }
            if (!found)
            {
                Directories.Add(directory);
            }

        }
        public void RemoveFolder(string folderName)
        {
            foreach (Directory directory in Directories)
            {
                if (directory.Name == folderName)
                {
                    Directories.Remove(directory);
                }
            }
        }
        }
}
