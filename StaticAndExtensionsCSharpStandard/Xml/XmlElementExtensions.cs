using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace StaticAndExtensionsCSharpStandard.Xml
{
    public static class XmlElementExtensions
    {
        public static XmlDocument ToXmlDocument(this XElement xElement)
        {
            var sb = new StringBuilder();
            var xws = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
            using (var xw = XmlWriter.Create(sb, xws))
            {
                xElement.WriteTo(xw);
            }
            var doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            return doc;
        }
    }
}