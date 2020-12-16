// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetExpiredItems
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetExpiredItems : Stanza
  {
    public GetExpiredItems(Client User, XmlDocument Packet)
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
      XElement xelement3 = new XElement(Stanza.NameSpace + "get_expired_items");
      foreach (Item obj in this.User.Player.Items)
      {
        if (obj.ItemType == WARTLS.CLASSES.ItemType.CONSUMABLE)
          xelement3.Add((object) new XElement((XName) "consumable_item", (object) obj.Serialize(false, 0).Attributes().Select<XAttribute, XAttribute>((Func<XAttribute, XAttribute>) (attr => new XAttribute(attr)))));
        if (obj.ItemType == WARTLS.CLASSES.ItemType.PERMANENT)
          xelement3.Add((object) new XElement((XName) "durability_item", (object) obj.Serialize(false, 0).Attributes().Select<XAttribute, XAttribute>((Func<XAttribute, XAttribute>) (attr => new XAttribute(attr)))));
      }
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
