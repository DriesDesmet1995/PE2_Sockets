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
        public List<FTFolder> Directories { get; private set; }

        public DirectoryService()
        {
            Directories = new List<FTFolder>();
        }

        public void AddPath(FTFolder directory)
        {
            bool found = false;
            foreach (FTFolder dir in Directories)
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
            foreach (FTFolder directory in Directories)
            {
                if (directory.Name == folderName)
                {
                    Directories.Remove(directory);
                }
            }
        }
        }
}
