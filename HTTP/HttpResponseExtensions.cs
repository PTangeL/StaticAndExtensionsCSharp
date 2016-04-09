namespace StaticAndExtensionsCSharp.HTTP
{
    using System.Web;

    public static class HttpResponseExtensions
    {
        private const string ForceDownloadOutputFileName = "defaultName.pdf";

        public static void ForceDownload(this HttpResponse Response, string fullPathToFile, string outputFileName = ForceDownloadOutputFileName)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + outputFileName);
            Response.WriteFile(fullPathToFile);
            Response.ContentType = string.Empty;
            Response.End();
        }
    }
}
