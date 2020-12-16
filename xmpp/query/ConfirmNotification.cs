// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ConfirmNotification
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
  public class ConfirmNotification : Stanza
  {
    private string Channel;

    public ConfirmNotification(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      foreach (XmlNode childNode1 in this.Query.ChildNodes)
      {
        foreach (XmlNode childNode2 in User.Player.Notifications["notifications"].ChildNodes)
        {
          if (childNode2.Attributes["id"].InnerText == childNode1.Attributes["id"].InnerText)
          {
            if (childNode2.Attributes["type"].InnerText == "16")
            {
              string ReceivedUser = childNode2["invitation"].Attributes["initiator"].InnerText;
              Client User1 = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == ReceivedUser));
              if (User1 != null && User.Player.Clan.ID == 0L && User1.Player.Clan.ClanMembers.Elements().Count<XElement>() < 50)
              {
                User1.Player.AddResultNotification(User.Player.UserID, User.JID, User.Player.Nickname, User.Status, childNode1["confirmation"].Attributes["location"].InnerText, User.Player.Experience, childNode1["confirmation"].Attributes["result"].InnerText, true);
                new SyncNotification(User1, (XmlDocument) null).Process();
                if (childNode1["confirmation"].Attributes["result"].InnerText == "0")
                {
                  User.Player.Clan = User1.Player.Clan;
                  User.Player.ClanPlayer.InvitationDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                  User.Player.ClanPlayer.ClanRole = Clan.CLAN_ROLE.DEFAULT;
                  User1.Player.Clan.AddPlayer(User.Player);
                  foreach (XElement xelement in User.Player.Clan.ClanMembers.Elements((XName) "clan_member_info").ToList<XElement>())
                  {
                    XElement clanMember = xelement;
                    Client User2 = ArrayList.OnlineUsers.Find((Predicate<Client>) (x => x.Player.UserID == long.Parse(clanMember.Attribute((XName) "profile_id").Value)));
                    if (User2 != null)
                    {
                      ClanInfo clanInfo = new ClanInfo(User2, (XmlDocument) null);
                    }
                  }
                  User.Player.Save();
                }
              }
            }
            if (childNode2.Attributes["type"].InnerText == "64")
            {
              string ReceivedUser2 = childNode2["invitation"].Attributes["initiator"].InnerText;
              Client User1 = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == ReceivedUser2));
              if (User1 != null)
              {
                if (childNode1["confirmation"].Attributes["result"].InnerText == "0")
                {
                  User.Player.AddFriend(User1.Player.UserID.ToString());
                  User1.Player.AddFriend(User.Player.UserID.ToString());
                  User.Player.AddResultNotification(User1.Player.UserID, User1.JID, User1.Player.Nickname, User1.Status, User1.Location, User.Player.Experience, childNode1["confirmation"].Attributes["result"].InnerText, false);
                  new SyncNotification(User, (XmlDocument) null).Process();
                }
                User1.Player.AddResultNotification(User.Player.UserID, User.JID, User.Player.Nickname, User.Status, childNode1["confirmation"].Attributes["location"].InnerText, User.Player.Experience, childNode1["confirmation"].Attributes["result"].InnerText, false);
                new SyncNotification(User1, (XmlDocument) null).Process();
              }
              else
              {
                Player player = new Player()
                {
                  Nickname = ReceivedUser2
                };
                player.Load(true);
                if (childNode1["confirmation"].Attributes["result"].InnerText == "0")
                {
                  User.Player.AddFriend(player.UserID.ToString());
                  player.AddFriend(User.Player.UserID.ToString());
                  User.Player.AddResultNotification(player.UserID, "", player.Nickname, 0, "", player.Experience, childNode1["confirmation"].Attributes["result"].InnerText, false);
                  new SyncNotification(User, (XmlDocument) null).Process();
                }
                player.AddResultNotification(User.Player.UserID, User.JID, User.Player.Nickname, User.Status, childNode1["confirmation"].Attributes["location"].InnerText, User.Player.Experience, childNode1["confirmation"].Attributes["result"].InnerText, false);
                player.Save();
              }
            }
            User.Player.Notifications["notifications"].RemoveChild(childNode2);
            User.Player.Save();
            FriendList friendList = new FriendList(User, (XmlDocument) null);
          }
        }
      }
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
      XElement xelement3 = new XElement((XName) "confirm_notification");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
