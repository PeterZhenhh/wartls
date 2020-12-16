// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ClanKick
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.EXCEPTION;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ClanKick : Stanza
  {
    public ClanKick(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      long num = long.Parse(this.Query.Attributes["profile_id"].InnerText);
      if (User.Player.Clan == null || User.Player.ClanPlayer.ClanRole != Clan.CLAN_ROLE.LEADER)
        throw new StanzaException(User, Packet, 4);
      Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == long.Parse(this.Query.Attributes["profile_id"].InnerText)));
      if (client != null)
      {
        User.Player.Clan.RemovePlayer(client.Player);
        client.Player.Clan = new Clan();
        client.Player.ClanPlayer = new ClanProperties();
        client.ShowMessage("@clans_you_was_kicked", false);
        client.Player.Save();
        if (client.Player.RoomPlayer.Room != null)
        {
          if (client.Player.RoomPlayer.Room.Core.RoomType == (byte) 4)
          {
            GameRoom_OnKicked gameRoomOnKicked = new GameRoom_OnKicked(client.Player.RoomPlayer.Room.Players.Users.ToList<Client>().Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == client.Player.UserID)), GameRoom_OnKicked.Reason.KickedByUser);
          }
          else
            client.Player.RoomPlayer.Room.Sync((Client) null);
        }
        ClanInfo clanInfo = new ClanInfo(client, (XmlDocument) null);
      }
      else
      {
        client = new Client();
        client.Player = new Player() { UserID = num };
        User.Player.Clan.RemovePlayer(client.Player);
        if (!client.Player.Load(true).Result)
          throw new StanzaException(User, Packet, 4);
        client.Player.ClanPlayer = new ClanProperties();
        client.Player.Clan = new Clan();
        client.Player.AddCustomMessage("@clans_you_was_kicked");
        client.Player.Save();
      }
      foreach (XElement xelement in User.Player.Clan.ClanMembers.Elements((XName) "clan_member_info").ToList<XElement>())
      {
        XElement clanMember = xelement;
        Client User1 = ArrayList.OnlineUsers.Find((Predicate<Client>) (x => x.Player.UserID == long.Parse(clanMember.Attribute((XName) "profile_id").Value)));
        if (User1 != null)
        {
          ClanInfo clanInfo = new ClanInfo(User1, (XmlDocument) null);
        }
      }
      this.Process();
    }

    public override void Process()
    {
      if (this.Type == "result")
        return;
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement((XName) "iq");
      xelement1.Add((object) new XAttribute((XName) "type", this.Type == "get" ? (object) "result" : (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.User.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query", (object) new XElement((XName) "clan_kick"));
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString());
    }
  }
}
