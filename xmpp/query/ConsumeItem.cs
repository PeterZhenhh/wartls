// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ConsumeItem
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.EXCEPTION;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ConsumeItem : Stanza
  {
    private string Channel;
    private Client Receiver;
    private Item PlayerItem;

    public ConsumeItem(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      if (!User.Dedicated)
        throw new StanzaException(User, Packet, 1006);
      long ID = long.Parse(this.Query.Attributes["profile_id"].InnerText);
      long ItemId = (long) int.Parse(this.Query.Attributes["item_profile_id"].InnerText);
      try
      {
        this.Receiver = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == ID));
        if (this.Receiver != null)
        {
          this.PlayerItem = this.Receiver.Player.Items.Find((Predicate<Item>) (Attribute => Attribute.ID == ItemId));
          --this.PlayerItem.Quantity;
        }
        User.Player.Save();
      }
      catch
      {
      }
      this.Process();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "masterserver@warface/onyx"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "consume_item", new object[4]
      {
        (object) new XAttribute((XName) "profile_id", (object) this.Receiver.Player.UserID),
        (object) new XAttribute((XName) "item_profile_id", (object) this.PlayerItem.ID),
        (object) new XAttribute((XName) "items_consumed", (object) 1),
        (object) new XAttribute((XName) "items_left", (object) this.PlayerItem.Quantity)
      });
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
