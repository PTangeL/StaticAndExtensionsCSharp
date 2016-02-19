namespace Library.HTTP
{
    using System.Web;

    public static class HttpResponseExtensions
    {
        public static void ForceDownload(this HttpResponse Response, string fullPathToFile, string outputFileName = "defaultName.pdf")
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + outputFileName);
            Response.WriteFile(fullPathToFile);
            Response.ContentType = string.Empty;
            Response.End();
        }
    }
}
