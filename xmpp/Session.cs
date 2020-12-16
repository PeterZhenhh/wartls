// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.Session
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp
{
  public class Session
  {
    private readonly XNamespace NameSpace = (XNamespace) "urn:ietf:params:xml:ns:xmpp-session";

    public Session(Client User, XmlDocument Packet)
    {
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "id", (object) Packet["iq"].Attributes["id"].InnerText));
      if (User.JID != null)
        xelement1.Add((object) new XAttribute((XName) "to", (object) User.JID));
      XElement xelement2 = new XElement(this.NameSpace + "session");
      xelement1.Add((object) xelement2);
      User.Send(xelement1.ToString(SaveOptions.DisableFormatting));
    }
  }
}
