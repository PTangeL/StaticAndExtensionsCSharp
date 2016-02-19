namespace Library.FileSystem
{
    using System.IO;

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
    }
}
