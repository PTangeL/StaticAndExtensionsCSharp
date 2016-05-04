namespace StaticAndExtensionsCSharp.FileSystem
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

    public static class DirectoryExtensions
    {
        /// <summary>
        /// <code>
        /// Recursively create directory.
        /// string path = @"C:\temp\one\two\three";

        /// var dir = new DirectoryInfo(path);
        /// dir.CreateDirectory();
        /// </code>
        /// </summary>
        /// <param name="dirInfo">Folder path to create.</param>
        public static void CreateDirectory(this DirectoryInfo dirInfo)
        {
            if (dirInfo.Parent != null) CreateDirectory(dirInfo.Parent);
            if (!dirInfo.Exists) dirInfo.Create();
        }
        
        /// <summary>
        /// Clean Files in the directory.
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <param name="listOfExceptions"></param>
        public static void CleanFilesInDirectory(this DirectoryInfo dirInfo, IEnumerable<string> listOfExceptions)
        {
            Contract.Requires(dirInfo != null);

            if(dirInfo.Exists)
            {
                var files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

                if (listOfExceptions?.Count() > 0)
                    files = files.Where(file => !listOfExceptions.Contains(file.Name)).ToArray();

                foreach (var file in files)
                    File.Delete(file?.FullName);
            }
        }
    }
}
