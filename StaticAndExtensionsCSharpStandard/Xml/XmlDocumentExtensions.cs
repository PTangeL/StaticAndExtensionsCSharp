using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace StaticAndExtensionsCSharpStandard.Xml
{
    public static class XmlDocumentExtensions
    {
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static Stream ToMemoryStream(this XmlDocument doc)
        {
            var xmlStream = new MemoryStream();
            doc.Save(xmlStream);
            xmlStream.Flush();//Adjust this if you want read your data
            xmlStream.Position = 0;
            return xmlStream;
        }
    }
}