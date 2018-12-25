using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Utilities
{
    public class DirectoryUtility
    {

        public static string CreateDirectory(String path,String directoryName)
        {
            string dirPath = Path.Combine(path,directoryName);

            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
            DirectoryInfo info = Directory.CreateDirectory(dirPath);
            return info.FullName + "\\";
        }

        public static void DeleteFolder(String path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static void CopyDirectory(string strSource, string strDestination)
        {
            if (!Directory.Exists(strDestination))
            {
                Directory.CreateDirectory(strDestination);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(strSource);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo tempfile in files)
            {
                tempfile.CopyTo(Path.Combine(strDestination, tempfile.Name));
            }

            DirectoryInfo[] directories = dirInfo.GetDirectories();
            foreach (DirectoryInfo tempdir in directories)
            {
                CopyDirectory(Path.Combine(strSource, tempdir.Name), Path.Combine(strDestination, tempdir.Name));
            }

        }
    }
}