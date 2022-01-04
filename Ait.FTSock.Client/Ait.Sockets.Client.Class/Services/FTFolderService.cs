using Ait.FTSock.Client.Class.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ait.FTSock.Client.Class.Services
{
    public class FTFolderService
    {
        public List<FTFolder> Directories { get; private set; }

        public FTFolderService()
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
                    dir.FTFiles = directory.FTFiles;

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
