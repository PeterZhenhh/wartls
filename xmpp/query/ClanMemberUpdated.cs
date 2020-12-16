// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ClanMemberUpdated
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ClanMemberUpdated : Stanza
  {
    private Client AboutOf;
    private Client client;

    public ClanMemberUpdated(Client AboutOf, XmlDocument Packet = null)
      : base(AboutOf, Packet)
    {
      if (this.Type == "result")
        return;
      this.AboutOf = AboutOf;
      foreach (XElement element in AboutOf.Player.Clan.ClanMembers.Elements((XName) "clan_member_info"))
      {
        XElement player = element;
        Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == player.Attribute((XName) "nickname").Value));
        if (client != null && client.Player.Clan.ID != 0L)
        {
          this.client = client;
          this.Process();
        }
      }
      if (Packet == null || !(Packet.Name == "peer_clan_member_update"))
        return;
      this.Process2();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement((XName) "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.client.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.client.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query", (object) new XElement((XName) "clan_members_updated", (object) new XElement((XName) "update", new object[2]
      {
        (object) new XAttribute((XName) "profile_id", (object) this.AboutOf.Player.UserID),
        (object) new XElement((XName) "clan_member_info", new object[8]
        {
          (object) new XAttribute((XName) "nickname", (object) this.AboutOf.Player.Nickname),
          (object) new XAttribute((XName) "profile_id", (object) this.AboutOf.Player.UserID),
          (object) new XAttribute((XName) "experience", (object) this.AboutOf.Player.Experience),
          (object) new XAttribute((XName) "clan_points", (object) this.AboutOf.Player.ClanPlayer.Points),
          (object) new XAttribute((XName) "invite_date", (object) this.AboutOf.Player.ClanPlayer.InvitationDate),
          (object) new XAttribute((XName) "clan_role", (object) (int) this.AboutOf.Player.ClanPlayer.ClanRole),
          (object) new XAttribute((XName) "status", (object) this.AboutOf.Status),
          (object) new XAttribute((XName) "jid", (object) this.AboutOf.JID)
        })
      })));
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.client.Send(xdocument.ToString());
    }

    public void Process2()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement((XName) "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.AboutOf.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query", (object) new XElement((XName) "peer_clan_member_update"));
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.AboutOf.Send(xdocument.ToString());
    }
  }
}
