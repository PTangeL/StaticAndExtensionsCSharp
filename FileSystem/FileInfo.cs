namespace Library.FileSystem
{
    using System.IO;

    public static class FileInfoExtensions
    {
        /// <summary>
        /// Move current instance and rename current instance when needed
        /// </summary>
        /// <example>
        /// <code>
        /// FileInfo fileInfo = new FileInfo(@"c:\test.txt");
        /// File.Create(fileInfo.FullName).Dispose();
        /// fileInfo.MoveTo(@"d:\", true);
        /// </code>
        /// </example>
        /// <param name="fileInfo">Current instance</param>
        /// <param name="destFileName">The Path to move current instance to, which can specify a different file name</param>
        /// <param name="renameWhenExists">Bool to specify if current instance should be renamed when exists</param>
        public static void MoveTo(this FileInfo fileInfo, string destFileName, bool renameWhenExists = false)
        {
            string newFullPath = string.Empty;

            if (renameWhenExists)
            {
                int count = 1;

                string fileNameOnly = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                string extension = Path.GetExtension(fileInfo.FullName);
                newFullPath = Path.Combine(destFileName, fileInfo.Name);

                while (File.Exists(newFullPath))
                {
                    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(destFileName, tempFileName + extension);
                }
            }

            fileInfo.MoveTo(renameWhenExists ? newFullPath : destFileName);
        }
    }
}
