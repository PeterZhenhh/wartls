// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.Stanza
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.CLASSES
{
  public class Stanza
  {
    public static XNamespace NameSpace = (XNamespace) "urn:cryonline:k01";
    protected Client User;
    protected XmlDocument Packet;
    protected XmlNode Query;
    public string To;
    public string Id;
    public string Type;
    public string Name;

    public Stanza(Client User, XmlDocument Packet)
    {
      this.User = User;
      if (Packet == null)
        return;
      if (Packet["message"] == null)
        this.Name = Packet["iq"]["query"].FirstChild.Name;
      try
      {
        this.To = Packet[Packet.FirstChild.Name].Attributes["to"].InnerText;
      }
      catch
      {
      }
      if (Packet[Packet.FirstChild.Name].Attributes["id"] != null)
        this.Id = Packet[Packet.FirstChild.Name].Attributes["id"].InnerText;
      if (Packet[Packet.FirstChild.Name].Attributes["type"] != null)
        this.Type = Packet[Packet.FirstChild.Name].Attributes["type"].InnerText;
      if (Packet["iq"] != null && Packet["iq"]["query"].FirstChild != null)
        this.Query = Packet["iq"]["query"].FirstChild;
      this.Packet = Packet;
    }

    public virtual void Process()
    {
    }

    public void Uncompress(ref XmlDocument Packet)
    {
      XmlDocument xmlDocument = new XmlDocument();
      string xml = Tools.Decode(Packet.LastChild.LastChild.LastChild.Attributes["compressedData"].InnerText);
      Packet.FirstChild.FirstChild.RemoveAll();
      xmlDocument.LoadXml(xml);
      XmlNode xmlNode = Packet.ImportNode((XmlNode) xmlDocument.DocumentElement, true);
      Packet.FirstChild.FirstChild.AppendChild(xmlNode);
      Packet.LastChild.LastChild.ReplaceChild(Packet.LastChild.LastChild.LastChild, xmlNode);
    }

    public void Compress(ref XDocument xDocument)
    {
      XmlDocument xmlDocument1 = new XmlDocument();
      using (XmlReader reader = xDocument.CreateReader())
        xmlDocument1.Load(reader);
      XmlDocument xmlDocument2 = new XmlDocument();
      XmlDocument xmlDocument3 = xmlDocument1;
      XmlNode firstChild = xmlDocument3.FirstChild["query"].FirstChild;
      string name = firstChild.Name;
      string str = Tools.Encode(firstChild.OuterXml);
      int byteCount = Encoding.UTF8.GetByteCount(firstChild.OuterXml);
      xmlDocument2.LoadXml(string.Format("<data query_name='{0}' compressedData='{1}' originalSize='{2}'/>", (object) name, (object) str, (object) byteCount));
      foreach (XmlAttribute attribute1 in (XmlNamedNodeMap) firstChild.Attributes)
      {
        XmlAttribute attribute2 = xmlDocument2.CreateAttribute(attribute1.Name);
        attribute2.InnerText = attribute1.InnerText;
        xmlDocument2.FirstChild.Attributes.Prepend(attribute2);
      }
      XmlNode newChild = xmlDocument3.ImportNode((XmlNode) xmlDocument2.DocumentElement, true);
      xmlDocument3.FirstChild.FirstChild.RemoveAll();
      xmlDocument3.FirstChild.FirstChild.PrependChild(newChild);
      using (XmlNodeReader xmlNodeReader = new XmlNodeReader((XmlNode) xmlDocument1))
      {
        int content = (int) xmlNodeReader.MoveToContent();
        xDocument = XDocument.Load((XmlReader) xmlNodeReader);
      }
    }
  }
}
