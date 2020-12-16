// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.SetClanInfo
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.EXCEPTION;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class SetClanInfo : Stanza
  {
    private Client client;

    public SetClanInfo(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      if (User.Player.Clan.ID == 0L)
        return;
      if (Encoding.UTF8.GetString(Convert.FromBase64String(this.Query.Attributes["description"].InnerText)).Length > 2000)
        throw new StanzaException(User, Packet, 2);
      User.Player.Clan.Description = Encoding.UTF8.GetString(Convert.FromBase64String(this.Query.Attributes["description"].InnerText));
      User.Player.Clan.Save();
      foreach (XElement element in User.Player.Clan.ClanMembers.Elements((XName) "clan_member_info"))
      {
        XElement player = element;
        Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == player.Attribute((XName) "nickname").Value));
        if (client == User)
          this.Process2();
        if (client != null)
        {
          this.client = client;
          this.Process();
        }
      }
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement((XName) "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.client.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.client.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "clan_description_updated", (object) new XAttribute((XName) "description", (object) this.Query.Attributes["description"].InnerText));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.client.Send(xdocument.ToString());
    }

    public void Process2()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement((XName) "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      xelement2.Add((object) this.Query.InnerText);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString());
    }
  }
}
