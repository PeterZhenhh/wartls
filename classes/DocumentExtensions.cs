// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.DocumentExtensions
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Xml;
using System.Xml.Linq;

namespace WARTLS.CLASSES
{
  public static class DocumentExtensions
  {
    public static XmlDocument ToXmlDocument(this XDocument xDocument)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (XmlReader reader = xDocument.CreateReader())
      {
        xmlDocument.Load(reader);
        return xmlDocument;
      }
    }

    public static XDocument ToXDocument(this XmlDocument xmlDocument)
    {
      using (XmlNodeReader xmlNodeReader = new XmlNodeReader((XmlNode) xmlDocument))
      {
        int content = (int) xmlNodeReader.MoveToContent();
        return XDocument.Load((XmlReader) xmlNodeReader);
      }
    }
  }
}
