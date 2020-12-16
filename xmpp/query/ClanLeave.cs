// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ClanLeave
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
  public class ClanLeave : Stanza
  {
    public ClanLeave(Client User, XmlDocument Packet = null)
      : base(User, Packet)
    {
      if (User.Player.Clan == null)
        throw new StanzaException(User, Packet, 2);
      User.Player.Clan.RemovePlayer(User.Player);
      if (User.Player.Clan.ClanMembers.Elements((XName) "clan_member_info").Count<XElement>() == 0)
        User.Player.Clan.DeleteClan();
      else if (User.Player.ClanPlayer.ClanRole == Clan.CLAN_ROLE.LEADER)
      {
        XElement new_leader = User.Player.Clan.ClanMembers.Descendants((XName) "clan_member_info").ToList<XElement>().First<XElement>();
        Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == Convert.ToInt64(new_leader.Attribute((XName) "profile_id").Value)));
        if (client != null)
        {
          client.Player.Clan = User.Player.Clan;
          client.Player.Clan.LeaderName = client.Player.Nickname;
          client.Player.ClanPlayer.ClanRole = Clan.CLAN_ROLE.LEADER;
          client.ShowMessage("@clans_you_are_promoted_to_master", false);
          User.Player.Clan.UpdatePlayer(client.Player);
        }
        else
        {
          Player player = new Player()
          {
            UserID = Convert.ToInt64(new_leader.Attribute((XName) "profile_id").Value)
          };
          player.Clan.ID = User.Player.Clan.ID;
          if (player.Load(true).Result)
          {
            player.Clan = User.Player.Clan;
            player.ClanPlayer.ClanRole = Clan.CLAN_ROLE.LEADER;
            player.Clan.LeaderName = player.Nickname;
            player.Save();
            User.Player.Clan.UpdatePlayer(player);
          }
        }
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
      User.Player.ClanPlayer = new ClanProperties();
      User.Player.Clan = new Clan();
      ClanInfo clanInfo1 = new ClanInfo(User, (XmlDocument) null);
      if (User.Player.RoomPlayer.Room != null)
      {
        if (User.Player.RoomPlayer.Room.Core.RoomType == (byte) 4)
        {
          GameRoom_OnKicked gameRoomOnKicked = new GameRoom_OnKicked(User.Player.RoomPlayer.Room.Players.Users.ToList<Client>().Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == User.Player.UserID)), GameRoom_OnKicked.Reason.KickedByUser);
        }
        else
          User.Player.RoomPlayer.Room.Sync((Client) null);
      }
      User.Player.Save();
      this.Process();
    }

    public override void Process()
    {
      if (this.Type == "result")
        return;
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.User.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "clan_leave");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString());
    }
  }
}
