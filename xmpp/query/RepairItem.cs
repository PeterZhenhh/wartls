// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.RepairItem
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
  public class RepairItem : Stanza
  {
    private Item Item;

    public RepairItem(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      int int32 = Convert.ToInt32(this.Query.Attributes["repair_cost"].Value);
      if (User.Player.GameMoney < int32)
        throw new StanzaException(User, Packet, 1);
      this.Item = User.Player.Items.Find((Predicate<Item>) (Attribute => Attribute.ID == long.Parse(this.Query.Attributes["item_id"].InnerText)));
      this.Item.DurabilityPoints = this.Item.TotalDurabilityPoints;
      this.Item.RepairCost = 0L;
      User.Player.GameMoney -= int32;
      User.Player.Save();
      this.Process();
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "repair_item");
      xelement3.Add((object) new XAttribute((XName) "accept_repair", (object) "1"));
      xelement3.Add((object) new XAttribute((XName) "total_durability", (object) this.Item.TotalDurabilityPoints));
      xelement3.Add((object) new XAttribute((XName) "durability", (object) this.Item.DurabilityPoints));
      xelement3.Add((object) new XAttribute((XName) "game_money", (object) this.User.Player.GameMoney));
      xelement3.Add((object) new XAttribute((XName) "repair_cost", (object) "0"));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
