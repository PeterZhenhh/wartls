// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ToOnlinePlayers
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ToOnlinePlayers : Stanza
  {
    private string Channel;
    private Client Receiver;

    public ToOnlinePlayers(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Receiver = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.JID == this.To));
      this.Process();
    }

    public override void Process()
    {
      if (this.Receiver != null)
      {
        this.Receiver.Send(this.Packet.InnerXml);
      }
      else
      {
        XDocument xdocument = new XDocument();
        XElement xelement1 = new XElement((XName) "iq");
        xelement1.Add((object) new XAttribute((XName) "type", (object) this.Type));
        if (this.To != null)
          xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
        xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
        xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
        XElement xelement2 = new XElement(Stanza.NameSpace + "query");
        XElement xelement3 = new XElement((XName) "error");
        xelement3.Add((object) new XAttribute((XName) "type", (object) "cancel"));
        xelement3.Add((object) new XAttribute((XName) "code", (object) 503));
        xelement3.Add((object) new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "service-unavailable"));
        xelement1.Add((object) xelement2);
        xelement1.Add((object) xelement3);
        xdocument.Add((object) xelement1);
        this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
      }
    }
  }
}
