// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetContacts
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetContacts : Stanza
  {
    private string Channel;

    public GetContacts(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Process();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "get_contracts");
      xelement3.Add((object) new XElement((XName) "contract", new object[8]
      {
        (object) new XAttribute((XName) "profile_id", (object) this.User.Player.UserID),
        (object) new XAttribute((XName) "rotation_id", (object) "9"),
        (object) new XAttribute((XName) "contract_name", (object) ""),
        (object) new XAttribute((XName) "current", (object) "0"),
        (object) new XAttribute((XName) "total", (object) "0"),
        (object) new XAttribute((XName) "rotation_time", (object) "6780.903746"),
        (object) new XAttribute((XName) "status", (object) "0"),
        (object) new XAttribute((XName) "is_available", (object) "0")
      }));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
